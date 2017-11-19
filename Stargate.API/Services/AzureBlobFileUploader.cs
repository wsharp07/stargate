using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Stargate.API.Models;

namespace Stargate.API.Services
{
    public class AzureBlobFileUploader : IFileUploader
    {
        private readonly string AZURE_KEY = "stargate:azureStorageAccount";
        private readonly string CONTAINER_NAME = "stargate-files";

        private readonly IConfiguration _configuration;
        public AzureBlobFileUploader(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<FileReference> UploadAsync(IFormFile file)
        {
            var response = new FileReference();
            var storageAccountKey = _configuration[AZURE_KEY];

            if (storageAccountKey == null)
            {
                throw new ArgumentNullException(nameof(storageAccountKey), $"You must set an environment variable for {AZURE_KEY}");
            }

            //Connect to Azure
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageAccountKey);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(CONTAINER_NAME);

            // Make sure that container is there
            await container.CreateIfNotExistsAsync(
                BlobContainerPublicAccessType.Blob, 
                blobClient.DefaultRequestOptions, 
                new OperationContext());

            // Retrieve reference to a named blob
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(file.FileName);

            //Upload the file to Azure.
            Stream fileStream = file.OpenReadStream();
            await blockBlob.UploadFromStreamAsync(fileStream);
            fileStream.Dispose();

            response.FileName = file.FileName;
            response.Uri = blockBlob.Uri.AbsoluteUri;
            response.Success = true;
            return response;
        }
    }
}
