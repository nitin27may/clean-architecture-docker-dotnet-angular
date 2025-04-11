using Contact.Application.Interfaces;
using Contact.Application.UseCases.Pages;
using Contact.Domain.Entities;
using Contact.Domain.Interfaces;

namespace Contact.Application.Services;

public class PageService : IPageService
{
    private readonly IPageRepository _pageRepository;

    public PageService(IPageRepository pageRepository)
    {
        _pageRepository = pageRepository;
    }

    public async Task<Page> AddPage(CreatePage createPage)
    {
        var page = new Page
        {
            Name = createPage.Name,
            Url = createPage.Url,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = createPage.CreatedBy
        };

        return await _pageRepository.Add(page);
    }

    public async Task<Page> UpdatePage(Guid id, UpdatePage updatePage)
    {
        var page = await _pageRepository.FindByID(id);
        if (page == null)
        {
            throw new Exception("Page not found");
        }

        page.Name = updatePage.Name;
        page.Url = updatePage.Url;
        page.UpdatedOn = DateTime.UtcNow;
        page.UpdatedBy = updatePage.UpdatedBy;

        return await _pageRepository.Update(page);
    }

    public async Task<IEnumerable<Page>> GetPages()
    {
        return await _pageRepository.FindAll();
    }

    public async Task<bool> DeletePage(Guid id)
    {
        return await _pageRepository.Delete(id);
    }
}
