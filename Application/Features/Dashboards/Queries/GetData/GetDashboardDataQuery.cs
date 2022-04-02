using Application.Interfaces.Repositories;
using Application.Interfaces.Services.Identity;
using Domain.Entities.ExtendedAttributes;
using Domain.Entities.Misc;
using Domain.Entities.TimeStamp;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Wrapper;

namespace Application.Features.Dashboards.Queries.GetData
{
    public class GetDashboardDataQuery : IRequest<Result<DashboardDataResponse>>
    {

    }

    internal class GetDashboardDataQueryHandler : IRequestHandler<GetDashboardDataQuery, Result<DashboardDataResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public GetDashboardDataQueryHandler(IUnitOfWork<int> unitOfWork, IUserService userService, IRoleService roleService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _roleService = roleService;
        }

        public async Task<Result<DashboardDataResponse>> Handle(GetDashboardDataQuery query, CancellationToken cancellationToken)
        {
            var response = new DashboardDataResponse
            {
                ProductCount = 21,// await _unitOfWork.Repository<Product>().Entities.CountAsync(cancellationToken),
                StampCount = await _unitOfWork.Repository<Stamp>().Entities.CountAsync(cancellationToken),
                DocumentCount = await _unitOfWork.Repository<Document>().Entities.CountAsync(cancellationToken),
                DocumentTypeCount = await _unitOfWork.Repository<DocumentType>().Entities.CountAsync(cancellationToken),
                DocumentExtendedAttributeCount = await _unitOfWork.Repository<DocumentExtendedAttribute>().Entities.CountAsync(cancellationToken),
                UserCount = await _userService.GetCountAsync(),
                RoleCount = await _roleService.GetCountAsync()
            };

            var selectedYear = DateTime.Now.Year;
            double[] productsFigure = new double[13];
            double[] stampsFigure = new double[13];
            double[] documentsFigure = new double[13];
            double[] documentTypesFigure = new double[13];
            double[] documentExtendedAttributesFigure = new double[13];
            for (int i = 1; i <= 12; i++)
            {
                var month = i;
                var filterStartDate = new DateTime(selectedYear, month, 01);
                var filterEndDate = new DateTime(selectedYear, month, DateTime.DaysInMonth(selectedYear, month), 23, 59, 59); // Monthly Based

                productsFigure[i - 1] = 21;// await _unitOfWork.Repository<Product>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                stampsFigure[i - 1] = await _unitOfWork.Repository<Stamp>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                documentsFigure[i - 1] = 21;//await _unitOfWork.Repository<Document>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                documentTypesFigure[i - 1] = 21;// await _unitOfWork.Repository<DocumentType>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
                documentExtendedAttributesFigure[i - 1] = 21;// await _unitOfWork.Repository<DocumentExtendedAttribute>().Entities.Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate).CountAsync(cancellationToken);
            }

            response.DataEnterBarChart.Add(new ChartSeries { Name = "Products", Data = productsFigure });
            response.DataEnterBarChart.Add(new ChartSeries { Name = "Stamps", Data = stampsFigure });
            response.DataEnterBarChart.Add(new ChartSeries { Name = "Documents", Data = documentsFigure });
            response.DataEnterBarChart.Add(new ChartSeries { Name = "Document Types", Data = documentTypesFigure });
            response.DataEnterBarChart.Add(new ChartSeries { Name = "Document Extended Attributes", Data = documentExtendedAttributesFigure });

            return await Result<DashboardDataResponse>.SuccessAsync(response);
        }
    }
}
