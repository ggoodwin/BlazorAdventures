using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities.TimeStamp;
using LazyCache;
using MediatR;
using Shared.Constants.Application;
using Shared.Wrapper;

namespace Application.Features.Stamps.Queries.GetAll
{
    public class GetAllStampsQuery : IRequest<Result<List<GetAllStampsResponse>>>
    {
        public GetAllStampsQuery()
        {
        }
    }

    internal class GetAllCustomersCachedQueryHandler : IRequestHandler<GetAllStampsQuery, Result<List<GetAllStampsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllCustomersCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllStampsResponse>>> Handle(GetAllStampsQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Stamp>>> getAllStamps = () => _unitOfWork.Repository<Stamp>().GetAllAsync();
            var stampList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllStampsCacheKey, getAllStamps);
            var mappedStamps = _mapper.Map<List<GetAllStampsResponse>>(stampList);
            return await Result<List<GetAllStampsResponse>>.SuccessAsync(mappedStamps);
        }
    }
}
