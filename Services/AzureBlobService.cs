using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace TestTask.Services
{
    public class AzureBlobService
    {
        BlobServiceClient _blobClient;
        BlobContainerClient _containerClient;
        string azureConnectionString = "DefaultEndpointsProtocol=https;AccountName=projectfiel;AccountKey=uljUYTtOSvtczpcHuxllvQBvwB8dfGe3Q8DWrPi3MFQAtwu0mWDAMrHKPZwqP/Suww39FLHllUdB+AStKmSBOg==;EndpointSuffix=core.windows.net";
        public AzureBlobService()
        {
            _blobClient = new BlobServiceClient(azureConnectionString);
            _containerClient = _blobClient.GetBlobContainerClient("filescontainer");
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
    }
}
