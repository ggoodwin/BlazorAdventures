using Application.Configurations;
using Application.Interfaces.Services;
using Application.Requests;
using Azure.Identity;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services
{
    public class BlobService : IBlobService
    {
        private readonly AzureConfiguration _config;
        private readonly ILogger<BlobService> _logger;

        public BlobService(IOptions<AzureConfiguration> config, ILogger<BlobService> logger)
        {
            _config = config.Value;
            _logger = logger;
        }

        public async Task<string> UploadDocAsync(UploadRequest uploadRequest)
        {
            var container = new BlobContainerClient(_config.ConnectionString, _config.DocContainer);
            if (uploadRequest.Data != null)
                await container.UploadBlobAsync(uploadRequest.FileName, BinaryData.FromBytes(uploadRequest.Data));
            //var blob = container.GetBlobClient(uploadRequest.FileName);
            //if (uploadRequest.Data != null) blob.Upload(new MemoryStream(uploadRequest.Data));
            var filePath = $"{_config.BaseUrl}/docs/{uploadRequest.FileName}";
            return filePath;
        }

        public async Task<string> UploadPicAsync(UploadRequest uploadRequest)
        {
            var container = new BlobContainerClient(_config.ConnectionString, _config.ProfileContainer);
            if (uploadRequest.Data != null)
                await container.UploadBlobAsync(uploadRequest.FileName, BinaryData.FromBytes(uploadRequest.Data));
            //var blob = container.GetBlobClient(uploadRequest.FileName);
            //if (uploadRequest.Data != null) blob.Upload(new MemoryStream(uploadRequest.Data));
            var filePath = $"{_config.BaseUrl}/profileimages/{uploadRequest.FileName}";
            return filePath;
        }
    }
}
