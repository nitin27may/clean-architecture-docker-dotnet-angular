using Contact.Application.UseCases.Pages;
using Contact.Domain.Entities;

namespace Contact.Application.Interfaces;

public interface IPageService: IGenericService<Page, PageResponse, CreatePage, UpdatePage>
{
    
}
