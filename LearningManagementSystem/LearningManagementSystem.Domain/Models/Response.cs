using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.Models
{
    public class Response<T> 
    {
        public bool IsSuccessful { get; set; }
        public T? Data { get; set; } = default!;
        public string? Error { get; set; }
    }

    public class Response
    {
        public bool IsSuccessful { get; set; }
        public string? Error { get; set; }
    }
}
