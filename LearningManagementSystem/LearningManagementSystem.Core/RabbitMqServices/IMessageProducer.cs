using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Core.RabbitMqServices
{
    public interface IMessageProducer
    {
        public void SendMessage<T>(string queue, T data);
    }
}
