using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.MassTransitModels
{
    public enum MessageType
    {
        Information, 
        Error
    }

    public enum DeliveryMethod
    {
        Email,
        Phone
    }
}
