using System.Net.Http.Json;
using Application.Features.ExtendedAttributes.Commands.AddEdit;
using Application.Features.ExtendedAttributes.Commands.Queries.Export;
using Application.Features.ExtendedAttributes.Commands.Queries.GetAll;
using Application.Features.ExtendedAttributes.Commands.Queries.GetAllByEntityId;
using Client.Infrastructure.Configurations;
using Client.Infrastructure.Extensions;
using Domain.Contracts;
using Microsoft.Extensions.Options;
using Shared.Wrapper;

namespace Client.Infrastructure.Managers.ExtendedAttribute
{
    public class ExtendedAttributeManager<TId, TEntityId, TEntity, TExtendedAttribute>
        : IExtendedAttributeManager<TId, TEntityId, TEntity, TExtendedAttribute>
            where TEntity : AuditableEntity<TEntityId>, IEntityWithExtendedAttributes<TExtendedAttribute>, IEntity<TEntityId>
            where TExtendedAttribute : AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>, IEntity<TId>
            where TId : IEquatable<TId>
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<UrlConfiguration> _config;

        public ExtendedAttributeManager(HttpClient httpClient, IOptions<UrlConfiguration> config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<IResult<string>> ExportToExcelAsync(ExportExtendedAttributesQuery<TId, TEntityId, TEntity, TExtendedAttribute> request)
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(request.SearchString) && !request.IncludeEntity && !request.OnlyCurrentGroup
                ? Routes.ExtendedAttributesEndpoints.Export(typeof(TEntity).Name, request.EntityId, request.IncludeEntity, request.OnlyCurrentGroup, request.CurrentGroup)
                : Routes.ExtendedAttributesEndpoints.ExportFiltered(typeof(TEntity).Name, request.SearchString, request.EntityId, request.IncludeEntity, request.OnlyCurrentGroup, request.CurrentGroup));
            return await response.ToResult<string>();
        }

        public async Task<IResult<TId>> DeleteAsync(TId id)
        {
            var response = await _httpClient.DeleteAsync($"{_config.Value.Api}/{Routes.ExtendedAttributesEndpoints.Delete(typeof(TEntity).Name)}/{id}");
            return await response.ToResult<TId>();
        }

        public async Task<IResult<List<GetAllExtendedAttributesResponse<TId, TEntityId>>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync($"{_config.Value.Api}/{Routes.ExtendedAttributesEndpoints.GetAll(typeof(TEntity).Name)}");
            return await response.ToResult<List<GetAllExtendedAttributesResponse<TId, TEntityId>>>();
        }

        public async Task<IResult<List<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>>>> GetAllByEntityIdAsync(TEntityId entityId)
        {
            var route = $"{_config.Value.Api}/{Routes.ExtendedAttributesEndpoints.GetAllByEntityId(typeof(TEntity).Name, entityId)}";
            var response = await _httpClient.GetAsync(route);
            return await response.ToResult<List<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>>>();
        }

        public async Task<IResult<TId>> SaveAsync(AddEditExtendedAttributeCommand<TId, TEntityId, TEntity, TExtendedAttribute> request)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_config.Value.Api}/{Routes.ExtendedAttributesEndpoints.Save(typeof(TEntity).Name)}", request);
            return await response.ToResult<TId>();
        }
    }
}
