namespace CatalogingSystem.Services.Implementations;

using CatalogingSystem.Services.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using CatalogingSystem.Core.Configurations;
using Microsoft.Extensions.Options;

public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IOptions<CloudinarySettings> settings)
    {
        var cloudinarySettings = settings.Value;
        Account cloudinaryAccount = new Account(
            cloudinarySettings.CloudName,
            cloudinarySettings.ApiKey,
            cloudinarySettings.ApiSecret
        );
        _cloudinary = new Cloudinary(cloudinaryAccount);
    }

    public async Task<List<string>> UploadImagesAsync(List<IFormFile> images, string publicIdPrefix)
    {
        var imageUrls = new List<string>();

        if (images != null && images.Any())
        {
            foreach (var image in images)
            {
                using var stream = image.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(image.FileName, stream),
                    PublicId = $"{publicIdPrefix}_{Guid.NewGuid()}"
                };
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                imageUrls.Add(uploadResult.SecureUrl.ToString());
            }
        }

        return imageUrls;
    }
}