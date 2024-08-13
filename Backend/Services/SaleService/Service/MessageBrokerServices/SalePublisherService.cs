using SaleService.OtherModels;
using SaleService.Service.MessageBrokerServices;

namespace SaleService.Service.MessageBrokerService
{
    public class SalePublisherService : BaseMessageService
    {
        public SalePublisherService(ILogger<SalePublisherService> logger) : base(logger)
        {
            DeclareQueue("sale_schedule");
        }

        public void UpdateInvoiceIdInSchedule(IEnumerable<Schedule> schedules, int invoiceId)
        {
            var queueName = "sale_schedule";

            foreach (var schedule in schedules)
            {
                var updateMessage = new
                {
                    ScheduleId = schedule.Id,
                    InvoiceId = invoiceId
                };

                // Xuất bản tin nhắn đến RabbitMQ
                PublishMessage(queueName, updateMessage);
            }
        }
    }
}
