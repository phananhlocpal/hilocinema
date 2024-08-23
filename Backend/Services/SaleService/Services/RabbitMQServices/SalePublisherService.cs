﻿using MessageBrokerService;
using SaleService.OtherModels;
using System.Text.Json;

namespace SaleService.Service.RabbitMQServices
{
    public class SalePublisherService : BaseMessageBroker
    {
        private readonly ILogger<SalePublisherService> _logger;
        private const string QueueName = "sale_schedule";

        public SalePublisherService(ILogger<SalePublisherService> logger) : base(logger)
        {
            _logger = logger;
            DeclareQueue(QueueName);
        }

        public void UpdateInvoiceIdInSchedule(List<OriginalSchedule> schedules)
        {
            foreach (var schedule in schedules)
            {
                // Serialize schedule object to JSON
                var message = JsonSerializer.Serialize(schedule);

                // Publish message to RabbitMQ
                PublishMessage(QueueName, message);
                _logger.LogInformation("Published message to RabbitMQ for schedule with seatId {SeatId}.", schedule.SeatId);
            }
        }
    }
}
