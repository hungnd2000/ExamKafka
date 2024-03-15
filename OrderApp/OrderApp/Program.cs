using Manonero.MessageBus.Kafka.Extensions;
using OrderApp.Application.Repositories;
using OrderApp.Application.Services;
using OrderApp.BackgroundTasks;
using OrderApp.Common;
using OrderApp.Data;
using OrderApp.Repositories;
using OrderApp.Settings;

var builder = WebApplication.CreateBuilder(args);
var appSetting = AppSetting.MapValue(builder.Configuration);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddOracle<OrderDbContext>(builder.Configuration.GetConnectionString("OracleConnection"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddKafkaProducers(producerBuilder =>
{
    producerBuilder.AddProducer(appSetting.GetProducerSetting(Constants.OrderSettingId));
});

builder.Services.AddKafkaConsumers(ConsumerBuilder =>
{
    ConsumerBuilder.AddConsumer<ConsumerBackgroundTask>(appSetting.GetConsumerSetting(Constants.OrderSettingId));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseKafkaMessageBus(messageBus =>
{
    messageBus.RunConsumer(Constants.OrderSettingId);
});

app.Run();
