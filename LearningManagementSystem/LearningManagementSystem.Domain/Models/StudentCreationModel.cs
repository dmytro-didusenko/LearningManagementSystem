using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.Models
{
    public class StudentCreationModel
    {
        public Guid UserId { get; set; }
        public string ContractNumber { get; set; } = string.Empty;

    }
}
