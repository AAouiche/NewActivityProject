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
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;
        private readonly string _storageAccount = "activityappimages";
        private readonly string _sasToken = "?sp=racwdl&st=2023-10-29T20:31:43Z&se=2023-11-01T04:31:43Z&spr=https&sv=2022-11-02&sr=c&sig=stqU5J1QdhKiTkWoF0t8nDCp5XsFLcpzVWTbCoGvmOc%3D";
        private readonly BlobContainerClient _blobContainerClient;

        public BlobStorageService(IConfiguration configuration)
        {
            var blobConnectionString = configuration.GetConnectionString("AzureBlobStorage");
            _blobServiceClient = new BlobServiceClient(blobConnectionString);
            _containerName = "testcontainer";
            //_containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            _blobContainerClient = new BlobContainerClient(new Uri($"https://{_storageAccount}.blob.core.windows.net/{_containerName}{_sasToken}"));
        }

        public async Task ListBlobContainersAsync()
        {
            var containers = _blobServiceClient.GetBlobContainersAsync();

            await foreach (var container in containers)
            {
                Debug.WriteLine(container.Name);
            }
        }

        public async Task<string> UploadImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                throw new ArgumentException("Image file is missing");
            }

            // Create a BlobContainerClient with the provided SAS token.
            var blobContainerClient = new BlobContainerClient(new Uri($"https://{_storageAccount}.blob.core.windows.net/{_containerName}{_sasToken}"));

            var blobClient = blobContainerClient.GetBlobClient(imageFile.FileName);
            using var stream = new MemoryStream();
            await imageFile.CopyToAsync(stream);
            stream.Position = 0; 

            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = imageFile.ContentType });

            return blobClient.Uri.ToString();
        }
        public async Task<BlobInfoDTO> GetBlobAsync(string name)
        {
            var blobClient = _blobContainerClient.GetBlobClient(name);
            var blobDownloadInfo = await blobClient.DownloadAsync();

            return new BlobInfoDTO(blobDownloadInfo.Value.Content,blobDownloadInfo.Value.ContentType);
        }
    }
}

