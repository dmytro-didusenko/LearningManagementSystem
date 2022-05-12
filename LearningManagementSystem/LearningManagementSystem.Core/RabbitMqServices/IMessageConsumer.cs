using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Core.RabbitMqServices
{
    public interface IMessageConsumer
    {
        public void ReceiveMessage<T>(string queue, Action<T> onMessage);
    }
}
