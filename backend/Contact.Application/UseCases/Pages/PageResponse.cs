namespace Contact.Application.UseCases.Pages;

public class PageResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public int PageOrder { get; set; }
}
