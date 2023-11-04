using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public string MessageBody { get; set; }
        public DateTime Created { get; set; }
        public string Username { get; set; }
        public string? Image { get; set; }
        public string? DisplayName { get; set; }
    }
}
