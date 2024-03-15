using Confluent.Kafka;
using Manonero.MessageBus.Kafka.Abstractions;
using OrderApp.Application.Repositories;
using OrderApp.DTOs;
using OrderApp.Repositories;
using System.Text.Json;

namespace OrderApp.BackgroundTasks
{
    public class ConsumerBackgroundTask : IConsumingTask<string, string>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ConsumerBackgroundTask> _logger;

        public ConsumerBackgroundTask(ILogger<ConsumerBackgroundTask> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public void Execute(ConsumeResult<string, string> result)
        {
            try
            {
                var messageValue = result.Message.Value;
                ConsumerCallbackAsync(messageValue);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error consuming message: {ex}");
            }
        }


        public async void ConsumerCallbackAsync(string messageValue)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                    var kafkaManager = scope.ServiceProvider.GetRequiredService<IKafkaProducerManager>();
                    var orderStatusMessage = JsonSerializer.Deserialize<OutputMessageValue>(messageValue);
                    var order = await orderRepository.GetOrderByIdAsync(orderStatusMessage.RefId);
                    if(orderStatusMessage.BusinessType == 1)
                    {
                        if (order != null)
                        {
                            if (orderStatusMessage.ErrorCode == 0)
                            {
                                order.Status = "Accepted";
                            }
                            else
                            {
                                order.Status = "Rejected";
                                order.ErrorCode = orderStatusMessage.ErrorCode;
                                order.ErrorMessage = orderStatusMessage.ErrorMessage;
                            }

                            var releaseHoldAmount = new ReleaseHoldAmount()
                            {
                                RefId = order.Id,
                                BusinessType = 2,
                                ProductId = order.ProductId,
                            };
                            var json = JsonSerializer.Serialize(releaseHoldAmount);
                            var releaseHoldAmountMessage = new Message<string, string>
                            {
                                Key = releaseHoldAmount.RefId,
                                Value = json
                            };

                            var kafkaProducer = kafkaManager.GetProducer<string, string>("Order");
                            kafkaProducer.Produce(releaseHoldAmountMessage);
                        }
                    }
                    _logger.LogInformation("Success 100%");

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }

    }
}
