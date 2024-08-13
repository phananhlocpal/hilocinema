using EmployeeService.Models;
using MessageBrokerService;

namespace EmployeeService.Services
{
    public class EmployeePublisher : BaseMessageBroker
    {
        private readonly ILogger _logger;
        public EmployeePublisher(ILogger<EmployeePublisher> logger) : base(logger)
        {
            _logger = logger;
            DeclareQueue("employee_authen");
        }

        public void CreateCustomerPubSub(Employee employee)
        {
            var queueName = "employee_authen";
            var message = employee;
            PublishMessage(queueName, message);
            _logger.LogInformation("Message published successfully.");
        }
    }
}
