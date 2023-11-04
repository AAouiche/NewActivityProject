using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Domain.DTO;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;
        private readonly string _storageAccount = "activityappimages";
        private readonly string _sasToken = "?sp=racwdl&st=2023-10-31T11:51:21Z&se=2025-04-01T18:51:21Z&spr=https&sv=2022-11-02&sr=c&sig=TWzVBgaLGfIaIj7QCesUfWEckxx9%2FfmFXbFM6SUO%2BDY%3D";
        private readonly BlobContainerClient _blobContainerClient;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IImageRepository _imageRepository;

        public BlobStorageService(IConfiguration configuration, IHttpContextAccessor HttpContextAccessor, IImageRepository imageRepository)
        {
            _imageRepository= imageRepository;
            _HttpContextAccessor = HttpContextAccessor;
            var blobConnectionString = configuration.GetConnectionString("AzureBlobStorage");
            _blobServiceClient = new BlobServiceClient(blobConnectionString);
            _containerName = "testcontainer";
            //_containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            _blobContainerClient = new BlobContainerClient(new Uri($"https://activityappimages.blob.core.windows.net/testcontainer?sp=racwdl&st=2023-11-01T12:09:45Z&se=2023-11-17T20:09:45Z&spr=https&sv=2022-11-02&sr=c&sig=%2Bc9LPYPYV0GeK0cyeJLud3ZQS0ruaTt0HAaZLunpkSQ%3D"));
        }

        public async Task ListBlobContainersAsync()
        {
            var containers = _blobServiceClient.GetBlobContainersAsync();

            await foreach (var container in containers)
            {
                Debug.WriteLine(container.Name);
            }
        }

        public async Task<(string imageUrl, string blobName)> UploadImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                throw new ArgumentException("Image file is missing");
            }

            var userId = _HttpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var uniqueBlobName = $"{userId}_{Guid.NewGuid()}";

            
            var blobContainerClient = new BlobContainerClient(new Uri($"https://{_storageAccount}.blob.core.windows.net/{_containerName}{_sasToken}"));

            var currentBlobName = await _imageRepository.CurrentBlob(userId);
            if (!string.IsNullOrEmpty(currentBlobName))
            {
                var oldBlobClient = blobContainerClient.GetBlobClient(currentBlobName);
                if (await oldBlobClient.ExistsAsync())
                {
                    await oldBlobClient.DeleteAsync();
                }
            }

            var newBlobClient = blobContainerClient.GetBlobClient(uniqueBlobName);
            using var stream = new MemoryStream();
            await imageFile.CopyToAsync(stream);
            stream.Position = 0;

            await newBlobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = imageFile.ContentType });

            // Update the Image entity in your database with the new blob name and other relevant data here.

            return (newBlobClient.Uri.ToString(),uniqueBlobName);
        }
        public async Task<BlobInfoDTO> GetBlobAsync(string name)
        {
            var blobClient = _blobContainerClient.GetBlobClient(name);
            var blobDownloadInfo = await blobClient.DownloadAsync();

            return new BlobInfoDTO(blobDownloadInfo.Value.Content,blobDownloadInfo.Value.ContentType);
        }
    }
}

