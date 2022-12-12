using Microsoft.AspNetCore.Http;
using MyHostAPI.Domain;
using MyHostAPI.Models;

namespace MyHostAPI.Business.Interfaces
{
    public interface IMediaService
    {
        Task<string> UploadImage(IFormFile image);
        Task DeleteImage(string imageName);
        Task<List<Image>> UpdateImages(List<ImageModel> imagesModel, List<Image> dbImages);
    }
}
