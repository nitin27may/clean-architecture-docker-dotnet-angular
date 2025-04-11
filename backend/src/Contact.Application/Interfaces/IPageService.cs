using Contact.Application.UseCases.Pages;
using Contact.Domain.Entities;

namespace Contact.Application.Interfaces;

public interface IPageService
{
    Task<Page> AddPage(CreatePage createPage);
    Task<Page> UpdatePage(Guid id, UpdatePage updatePage);
    Task<IEnumerable<Page>> GetPages();
    Task<bool> DeletePage(Guid id);
}
