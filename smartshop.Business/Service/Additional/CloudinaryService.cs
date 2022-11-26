using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using smartshop.Business.IServices.Additional;
using smartshop.Common.Configs;

namespace smartshop.Business.Service.Additional
{
    internal class CloudinaryService : ICloudinaryService
    {
        private string _baseFolder;
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinaryConfig> options)
        {
            var cloudinaryConfig = options.Value;
            _baseFolder = cloudinaryConfig.BaseFolder;

            var account = new Account(cloudinaryConfig.CloudName, cloudinaryConfig.ApiKey, cloudinaryConfig.ApiSecret);
           
            _cloudinary = new Cloudinary(account);
        }

        public DelResResult DeleteResources(List<string> publicIds, ResourceType resourceType = ResourceType.Image)
        {
            return _cloudinary.DeleteResources(resourceType, publicIds.ToArray());
        }

        public async Task<ImageUploadResult> UploadImage(Stream file, string folder)
        {
            var transformation = new Transformation();
            transformation.Width(200).Height(200).Crop("thumb").Gravity("face");

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Guid.NewGuid().ToString(), file),
                Folder = _baseFolder + folder,
                Transformation = transformation
            };

            return await _cloudinary.UploadAsync(uploadParams);
        }

        public async Task<ImageUploadResult> UploadImage(Stream file, string folder, bool IsRaw)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Guid.NewGuid().ToString(), file),
                Folder = _baseFolder + folder
            };

            return await _cloudinary.UploadAsync(uploadParams);
        }

        public async Task<RawUploadResult> UploadRaw(Stream file, string folder)
        {
            var uploadParams = new RawUploadParams()
            {
                File = new FileDescription(Guid.NewGuid().ToString(), file),
                Folder = _baseFolder + folder,
            };

            return await _cloudinary.UploadAsync(uploadParams);
        }
    }
}
