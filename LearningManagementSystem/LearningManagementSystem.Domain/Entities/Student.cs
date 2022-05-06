using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.Entities
{
    public class Student : User
    {
        public Guid GroupId { get; set; }
        public Group Group { get; set; } = null!;
    }
}
