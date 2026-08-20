using System.Data;
using Dapper;

namespace Contact.Infrastructure.Persistence.Helper;

/// <summary>
/// Dapper has no built-in support for System.DateOnly — ADO.NET's DbType enum predates
/// it, so passing a DateOnly parameter throws NotSupportedException at the driver level.
/// Npgsql maps PostgreSQL's "date" column type to DateOnly on read without issue; only
/// the outbound parameter direction needs this handler. Registered once at startup via
/// InfrastructureServiceCollectionExtensions.AddInfrastrcutureServices.
/// </summary>
public sealed class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.DbType = DbType.Date;
        parameter.Value = value.ToDateTime(TimeOnly.MinValue);
    }

    public override DateOnly Parse(object value) => value switch
    {
        DateOnly dateOnly => dateOnly,
        DateTime dateTime => DateOnly.FromDateTime(dateTime),
        _ => throw new NotSupportedException($"Cannot convert {value.GetType()} to DateOnly."),
    };
}
