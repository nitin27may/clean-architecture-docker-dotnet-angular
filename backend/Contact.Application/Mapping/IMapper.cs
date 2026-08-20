namespace Contact.Application.Mapping;

/// <summary>
/// Minimal object-to-object mapping abstraction. Replaces AutoMapper's IMapper for this
/// project — see docs/adr/0001-permissive-license-dependency-policy.md for why. Every
/// concrete conversion is generated at compile time by the Mapperly [Mapper] partial
/// classes in this folder; ObjectMapper only dispatches by (source type, destination
/// type) and applies audit-field stamping where AutoMapper used to do it via AfterMap.
/// </summary>
public interface IMapper
{
    TDestination Map<TDestination>(object? source);
}
