using CloudinaryDotNet.Actions;

namespace smartshop.Business.IServices.Additional
{
    public interface ICloudinaryService
    {
        Task<ImageUploadResult> UploadImage(Stream file, string folder);

        Task<ImageUploadResult> UploadImage(Stream file, string folder, bool IsRaw);

        Task<RawUploadResult> UploadRaw(Stream file, string folder);

        DelResResult DeleteResources(List<string> publicIds, ResourceType resourceType = ResourceType.Image);
    }
}
