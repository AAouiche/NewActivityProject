using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string MessageBody { get; set; }
        public Activity Activity { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public ApplicationUser User { get; set; }
    }
}
