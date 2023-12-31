﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ICloudinaryService
    {
        Task<(string imageUrl, string publicId)> UploadImageAsync(IFormFile imageFile);
    }
}
