namespace Contact.Infrastructure;

public class AppSettings
{
    public required ConnectionString ConnectionStrings { get; set; }
    public required string Secret { get; set; }
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
}

public class ConnectionString
{
    public required string DefaultConnection { get; set; }
}
