using Application.Features.Dashboards.Queries.GetData;
using Shared.Wrapper;

namespace Client.Infrastructure.Managers.Dashboard
{
    public interface IDashboardManager : IManager
    {
        Task<IResult<DashboardDataResponse>> GetDataAsync();
    }
}
