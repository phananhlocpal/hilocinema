using RabbitMQ.Client.Events;
using ScheduleService.Models;
using ScheduleService.Repositories.ScheduleRepository;
using System.Text;
using System.Text.Json;

namespace ScheduleService.Service.MessagerBrokerServices
{
    public class ScheduleConsumerService : BaseMessageService
    {
        private readonly IScheduleRepo _repository; 
        private const string QueueName = "invoice_schedule";

        public ScheduleConsumerService(
            ILogger<ScheduleConsumerService> logger,
            IScheduleRepo scheduleRepo) : base(logger)
        {
            _repository = scheduleRepo;
            DeclareQueue(QueueName);
            ConsumeMessage(QueueName, HandleMessage);
        }

        private async void HandleMessage(object sender, BasicDeliverEventArgs e)
        {
            try
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                // Deserialize the message
                var scheduleUpdateMessage = JsonSerializer.Deserialize<IEnumerable<Schedule>>(message);

                if (scheduleUpdateMessage != null)
                {
                    // Process the schedule update
                    await ProcessScheduleUpdate(scheduleUpdateMessage);
                }
                else
                {
                    // Handle invalid message
                    _logger.LogWarning("Received an invalid schedule update message.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing schedule update message.");
            }
        }

        private async Task ProcessScheduleUpdate(IEnumerable<Schedule> Schedules)
        {
            foreach (Schedule schedule in Schedules)
            {
                _repository.UpdateScheduleAsync(schedule);
            }
        }
    }
}
