namespace Contact.Application.UseCases.Roles;

public class UpdateRole
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid UpdatedBy { get; set; }
}
