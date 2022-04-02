using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities.TimeStamp;
using MediatR;
using Shared.Wrapper;

namespace Application.Features.Stamps.Queries.GetById
{
    public class GetStampByIdQuery : IRequest<Result<GetStampByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetStampByIdQueryHandler : IRequestHandler<GetStampByIdQuery, Result<GetStampByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetStampByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetStampByIdResponse>> Handle(GetStampByIdQuery query, CancellationToken cancellationToken)
        {
            var stamp = await _unitOfWork.Repository<Stamp>().GetByIdAsync(query.Id);
            var mappedStamp = _mapper.Map<GetStampByIdResponse>(stamp);
            return await Result<GetStampByIdResponse>.SuccessAsync(mappedStamp);
        }
    }
}
