using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyHostAPI.Business.Interfaces;
using MyHostAPI.Common.Configurations;
using MyHostAPI.Domain;
using MyHostAPI.Models;

namespace MyHostAPI.Business.Services
{
    public class MediaService : IMediaService
    {
        private readonly StorageAccountSection _storageAccountSection;
        private readonly ILogger<MediaService> _logger;

        public MediaService(IOptions<StorageAccountSection> storageOptions, ILogger<MediaService> logger)
        {
            _storageAccountSection = storageOptions.Value;
            _logger = logger;
        }

        public async Task<string> UploadImage(IFormFile image)
        {
            if (image.Length == 0)
            {
                _logger.LogError("Unable to upload image. File is invalid");
                throw new FileLoadException("File is invalid");
            }

            try
            {
                BlobServiceClient blobServiceClient = new(_storageAccountSection.StorageConnectionString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_storageAccountSection.FullImagesContainerNameOption);

                var fileName = $"{Guid.NewGuid()}{Path.GetFileName(image.FileName)}";
                var fullPath = $"{containerClient.Uri.OriginalString}/{fileName}";
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    var blobClient = containerClient.GetBlobClient(fileName);
                    await blobClient.UploadAsync(memoryStream, true);

                    _logger.LogInformation("Image uploaded successfully.");
                }

                return fullPath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Azure.RequestFailedException("File upload failed.", ex.InnerException);
            }
        }

        public async Task DeleteImage(string imageUrl)
        {
            try
            {
                var blobName = imageUrl.Substring(imageUrl.LastIndexOf('/') + 1);

                BlobClient blobClient = new(_storageAccountSection.StorageConnectionString, _storageAccountSection.FullImagesContainerNameOption, blobName);

                await blobClient.DeleteAsync();

                _logger.LogInformation("Image deleted successfully.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Azure.RequestFailedException("File deletion failed.", ex.InnerException);
            }
        }

        public async Task<List<Image>> UpdateImages(List<ImageModel> imagesModel, List<Image> dbImages)
        {
            foreach (var image in imagesModel)
            {
                if (image.Order == -1)
                {
                    await DeleteImage(image.Path);
                    var imageToRemove = dbImages.FirstOrDefault(x => x.Path == image.Path);
                    dbImages.Remove(imageToRemove);
                }
                else if (image.ImageFile != null)
                {
                    var path = await UploadImage(image.ImageFile);
                    var imageToAdd = new Image { Order = image.Order, Path = path };
                    dbImages.Add(imageToAdd);
                }
                else
                {
                    var premiseImage = dbImages.Where(x => x.Path == image.Path).FirstOrDefault();
                    premiseImage.Order = image.Order;
                }
            }

            _logger.LogInformation("Successfull images update.");

            return dbImages;
        }
    }
}
