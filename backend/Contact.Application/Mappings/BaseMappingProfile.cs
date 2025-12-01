using AutoMapper;
using Contact.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Contact.Application.Mappings;

public abstract class BaseMappingProfile : Profile
{
    private readonly IHttpContextAccessor? _httpContextAccessor;

    // Parameterless constructor for AutoMapper
    protected BaseMappingProfile()
    {
    }

    protected BaseMappingProfile(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected void SetAuditFields<TSource, TEntity>(TSource dto, TEntity entity, bool isCreate = true)
        where TEntity : BaseEntity
    {
        var userId = GetCurrentUserId();
        var currentTime = DateTimeOffset.UtcNow;
        
        if (isCreate)
        {
            entity.CreatedOn = currentTime;
            entity.CreatedBy = userId;
        }
        else
        {
            entity.UpdatedOn = currentTime;
            entity.UpdatedBy = userId;
        }
    }

    private Guid GetCurrentUserId() => 
        _httpContextAccessor?.HttpContext?.User?.FindFirst("Id") is { } userIdClaim 
            ? Guid.Parse(userIdClaim.Value) 
            : Guid.Empty;
}
