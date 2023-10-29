using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class BlobInfoDTO
    {
        public Stream Content { get; set; }
        public string ContentType { get; set; }

        public BlobInfoDTO(Stream content, string contentType)
        {
            Content = content;
            ContentType = contentType;
        }
    }
}
