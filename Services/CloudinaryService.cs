using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using review.Common.Models;


namespace review.Services
{
    public interface ICloudinaryService
    {
        Task<ImageUploadResult> UploadImage(IFormFile file);

        Task<DeletionResult> DeleteImage(string id);
    }
    public class CloudinaryService : ICloudinaryService
    {
        private readonly CloudinaryOptionsModel _options;
        public CloudinaryService(IOptions<CloudinaryOptionsModel> options)
        {
            _options = options.Value;
        }
        public async Task<ImageUploadResult> UploadImage (IFormFile file)
        {
            Account account = new Account(_options.CloudName, _options.Key, _options.SecretKey);
            Cloudinary cloudinary = new Cloudinary(account);
            cloudinary.Api.Secure = true;
            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "UserAvatar"
            };
            var uploadResult = await cloudinary.UploadAsync(uploadParams);
            return uploadResult;
        }

        public async Task<DeletionResult>DeleteImage (string id)
        {
            Account account = new Account(_options.CloudName, _options.Key, _options.SecretKey);
            Cloudinary cloudinary = new Cloudinary(account);
            cloudinary.Api.Secure = true;
            var deleteParams = new DeletionParams(id)
            {
                ResourceType = ResourceType.Image,
                
            };
            var deleteResult = await cloudinary.DestroyAsync(deleteParams);
            return deleteResult;
        }
    }
}
