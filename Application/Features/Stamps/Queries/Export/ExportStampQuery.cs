using Application.Extensions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Specifications.Stamps;
using Domain.Entities.TimeStamp;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Wrapper;

namespace Application.Features.Stamps.Queries.Export
{
    public class ExportStampQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportStampQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportStampsQueryHandler : IRequestHandler<ExportStampQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;

        public ExportStampsQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(ExportStampQuery request, CancellationToken cancellationToken)
        {
            var stampFilterSpec = new StampFilterSpecification(request.SearchString);
            var stamps = await _unitOfWork.Repository<Stamp>().Entities
                .Specify(stampFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(stamps, mappers: new Dictionary<string, Func<Stamp, object>>
            {
                { "Id", item => item.Id },
                { "TheStamp", item => item.TheStamp }
            }, sheetName: "Stamps");

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
