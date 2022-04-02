using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities.TimeStamp;
using MediatR;
using Shared.Constants.Application;
using Shared.Wrapper;

namespace Application.Features.Stamps.Commands.AddEdit
{
    public partial class AddEditStampCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public DateTime TheStamp { get; set; }
    }

    internal class AddEditStampCommandHandler : IRequestHandler<AddEditStampCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditStampCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(AddEditStampCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var stamp = _mapper.Map<Stamp>(command);
                await _unitOfWork.Repository<Stamp>().AddAsync(stamp);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllStampsCacheKey);
                return await Result<int>.SuccessAsync(stamp.Id, "Stamp Saved");
            }
            else
            {
                var stamp = await _unitOfWork.Repository<Stamp>().GetByIdAsync(command.Id);
                if (stamp != null)
                {
                    stamp.TheStamp = command.TheStamp;
                    await _unitOfWork.Repository<Stamp>().UpdateAsync(stamp);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllStampsCacheKey);
                    return await Result<int>.SuccessAsync(stamp.Id, "Stamp Updated");
                }
                else
                {
                    return await Result<int>.FailAsync("Stamp Not Found!");
                }
            }
        }
    }
}
