using Application.Features.Stamps.Commands.AddEdit;
using Application.Features.Stamps.Commands.Import;
using Application.Features.Stamps.Queries.GetAll;
using Shared.Wrapper;

namespace Client.Infrastructure.Managers.Stamp
{
    public interface IStampManager : IManager
    {
        Task<IResult<List<GetAllStampsResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditStampCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");

        Task<IResult<int>> ImportAsync(ImportStampCommand request);
    }
}
