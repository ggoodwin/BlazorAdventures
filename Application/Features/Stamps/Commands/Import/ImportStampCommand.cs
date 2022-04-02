using System.Data;
using Application.Features.Stamps.Commands.AddEdit;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Requests;
using AutoMapper;
using Domain.Entities.TimeStamp;
using FluentValidation;
using MediatR;
using Shared.Constants.Application;
using Shared.Wrapper;

namespace Application.Features.Stamps.Commands.Import
{
    public partial class ImportStampCommand : IRequest<Result<int>>
    {
        public UploadRequest UploadRequest { get; set; }
    }

    internal class ImportStampCommandHandler : IRequestHandler<ImportStampCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IExcelService _excelService;
        private readonly IMapper _mapper;
        private readonly IValidator<AddEditStampCommand> _addApiKeyValidator;

        public ImportStampCommandHandler(
            IUnitOfWork<int> unitOfWork,
            IExcelService excelService,
            IMapper mapper,
            IValidator<AddEditStampCommand> addApiKeyValidator)
        {
            _unitOfWork = unitOfWork;
            _excelService = excelService;
            _mapper = mapper;
            _addApiKeyValidator = addApiKeyValidator;
        }

        public async Task<Result<int>> Handle(ImportStampCommand request, CancellationToken cancellationToken)
        {
            var stream = new MemoryStream(request.UploadRequest.Data);
            var result = (await _excelService.ImportAsync(stream, mappers: new Dictionary<string, Func<DataRow, Stamp, object>>
            {
                { "TheStamp", (row,item) => item.TheStamp = Convert.ToDateTime(row["TheStamp"]) }
            }, "Stamps"));

            if (result.Succeeded)
            {
                var importedStamps = result.Data;
                var errors = new List<string>();
                var errorsOccurred = false;
                foreach (var stamp in importedStamps)
                {
                    var validationResult = await _addApiKeyValidator.ValidateAsync(_mapper.Map<AddEditStampCommand>(stamp), cancellationToken);
                    if (validationResult.IsValid)
                    {
                        await _unitOfWork.Repository<Stamp>().AddAsync(stamp);
                    }
                    else
                    {
                        errorsOccurred = true;
                        errors.AddRange(validationResult.Errors.Select(e => $"{(stamp.TheStamp != DateTime.MinValue ? $"{stamp.TheStamp} - " : string.Empty)}{e.ErrorMessage}"));
                    }
                }

                if (errorsOccurred)
                {
                    return await Result<int>.FailAsync(errors);
                }

                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllStampsCacheKey);
                return await Result<int>.SuccessAsync(result.Data.Count(), result.Messages[0]);
            }
            else
            {
                return await Result<int>.FailAsync(result.Messages);
            }
        }
    }
}
