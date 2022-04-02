using Application.Interfaces.Repositories;
using Domain.Entities.TimeStamp;
using MediatR;
using Shared.Constants.Application;
using Shared.Wrapper;

namespace Application.Features.Stamps.Commands.Delete
{
    public class DeleteStampCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteStampCommandHandler : IRequestHandler<DeleteStampCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteStampCommandHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(DeleteStampCommand command, CancellationToken cancellationToken)
        {
            var stamp = await _unitOfWork.Repository<Stamp>().GetByIdAsync(command.Id);
            if (stamp != null)
            {
                await _unitOfWork.Repository<Stamp>().DeleteAsync(stamp);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllStampsCacheKey);
                return await Result<int>.SuccessAsync(stamp.Id, "Stamp Deleted");
            }
            else
            {
                return await Result<int>.FailAsync("Stamp Not Found!");
            }
        }
    }
}
