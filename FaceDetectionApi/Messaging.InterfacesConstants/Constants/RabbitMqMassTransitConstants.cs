using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.InterfacesConstants.Constants
{
    public class RabbitMqMassTransitConstants
    {
        public const string RabbitMqURI = "rabbitmq://rabbitmq/";
        public const string UserName = "guest";
        public const string Password = "guest";
        public const string RegisterOrderCommandQueue = "register.order.command";
    }
}
