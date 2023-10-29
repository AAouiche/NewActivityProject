using Domain.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IBlobStorageService
    {
        Task<string> UploadImageAsync(IFormFile imageFile);
        Task<BlobInfoDTO> GetBlobAsync(string name);
    }
}
