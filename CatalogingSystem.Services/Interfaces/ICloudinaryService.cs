namespace CatalogingSystem.Services.Interfaces;

using Microsoft.AspNetCore.Http;

public interface ICloudinaryService
{
    Task<List<string>> UploadImagesAsync(List<IFormFile> images, string publicIdPrefix);
}