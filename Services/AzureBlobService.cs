using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace TestTask.Services
{
    public class AzureBlobService
    {
        BlobServiceClient _blobClient;
        BlobContainerClient _containerClient;
        string azureConnectionString = "DefaultEndpointsProtocol=https;AccountName=testtaskstorage7777;AccountKey=K0qBAvD/cLJzGg3acpXGOyIPm/ljEc61M9SvxCa3vA/sfYNDBwu0n9BJjN4S3Eh88gd4K2ItsnTW+ASt2c+uSw==;EndpointSuffix=core.windows.net";
        public AzureBlobService()
        {
            _blobClient = new BlobServiceClient(azureConnectionString);
            _containerClient = _blobClient.GetBlobContainerClient("azurestoragecontainer");
        }

        public async Task<List<Azure.Response<BlobContentInfo>>> UploadFiles(List<IFormFile> files)
        {

            var azureResponse = new List<Azure.Response<BlobContentInfo>>();
            foreach (var file in files)
            {
                string fileName = file.FileName;
                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    memoryStream.Position = 0;
                    var client = await _containerClient.UploadBlobAsync(fileName, memoryStream, default);
                    azureResponse.Add(client);
                }
            };

            return azureResponse;
        }

        public async Task<List<BlobItem>> GetUploadedBlobs()
        {
            var items = new List<BlobItem>();
            var uploadedFiles = _containerClient.GetBlobsAsync();
            await foreach (BlobItem file in uploadedFiles)
            {
                items.Add(file);
            }

            return items;
        }

        internal Task AddMetadataToFile()
        {
            throw new NotImplementedException();
        }

        internal async Task AddMetadataToFile(string blobName, string email)
        {
            try
            {
                var blob = _containerClient.GetBlobClient(blobName);

                IDictionary<string, string> metadata =
                   new Dictionary<string, string>();

                metadata["email"] = email;

                await blob.SetMetadataAsync(metadata);
            }
            catch (RequestFailedException e)
            {
                throw;
            }
        }
    }
}
