using Application.Requests;

namespace Application.Interfaces.Services
{
    public interface IBlobService
    {
        Task<string> UploadDocAsync(UploadRequest uploadRequest);
        Task<string> UploadPicAsync(UploadRequest uploadRequest);
    }
}
