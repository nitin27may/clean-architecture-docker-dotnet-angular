namespace Contact.Domain.Mappings
{
    public class PermissionsByRoleMappings
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public List<PageOperationMappings> Pages { get; set; }
    }

    public class PageOperationMappings
    {
        public Guid PageId { get; set; }
        public string PageName { get; set; }
        public List<OperationMappings> Operations { get; set; }
    }

    public class OperationMappings
    {
        public Guid OperationId { get; set; }
        public string OperationName { get; set; }
        public bool IsSelected { get; set; }
    }
}
