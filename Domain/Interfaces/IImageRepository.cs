using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IImageRepository
    {
        Task CreateAsync( Image image);
        Task<string> CurrentBlob(string id);
        Task<string> GetCurrentPublicId(string userId);
        Task CreateOrUpdateAsync(Image image);
    }
}
