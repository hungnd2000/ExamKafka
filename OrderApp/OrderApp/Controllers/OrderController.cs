using Confluent.Kafka;
using Manonero.MessageBus.Kafka.Abstractions;
using Microsoft.AspNetCore.Mvc;
using OrderApp.Application.Services;
using OrderApp.DTOs;
using OrderApp.Entities;
using System.Text.Json;

namespace OrderApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger; 
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        public readonly IKafkaProducerManager _producerManager;

        public OrderController(
            IOrderService orderService, 
            ILogger<OrderController> logger,
            IConfiguration configuration,
            HttpClient httpClient,
            IKafkaProducerManager producerManager)
        {
            _orderService = orderService;
            _logger = logger;
            _configuration = configuration;
            _httpClient = httpClient;
            _producerManager = producerManager;
        }

        [HttpGet]
        public async Task<List<Order>> GetOrders() {
            try
            {
                return await _orderService.GetALlOrderAsync();
            }catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderInput orderInput)
        {
            try
            {
                // Lấy thông tin sản phẩm từ Product api
                var apiGetProductId = _configuration["HttpGetProduct"] + "/" + orderInput.ProductId;

                HttpResponseMessage responseMessage = new HttpResponseMessage();
                responseMessage = await _httpClient.GetAsync(apiGetProductId);


                // Kiểm tra xem sản phẩm có tồn tại không
                if (responseMessage.IsSuccessStatusCode)
                {
                    if (responseMessage.Content.Headers.ContentLength != 0)
                    {
                        var orderAdd = new Order(orderInput.ProductId, orderInput.Amount);
                        var checkProductAmount = new CheckProductAmount()
                        {
                            RefId = orderAdd.Id,
                            BusinessType = 1,
                            ProductId = orderInput.ProductId,
                            Amount = orderAdd.Amount
                        };

                        ProduceCheckProductAmount(checkProductAmount);
                        var result = await _orderService.CreateOrderAsync(orderAdd);
                        return Ok("Order vừa được tạo");
                    }
                    else
                        return NotFound("Sản phẩm không tồn tại");
                }
                else
                    return NotFound("Sản phẩm không tồn tại");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        private void ProduceCheckProductAmount(CheckProductAmount checkProductAmount)
        {
            var json = JsonSerializer.Serialize(checkProductAmount);
            var message = new Message<string, string> 
            { 
                Key = checkProductAmount.RefId,
                Value = json 
            };

            var kafkaProducer = _producerManager.GetProducer<string, string>("Order");
            kafkaProducer.Produce(message);
            _logger.LogInformation($"Produce Success message: {message}");
        }

    }
}
