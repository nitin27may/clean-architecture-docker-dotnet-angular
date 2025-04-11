using Contact.Application.Interfaces;
using Contact.Application.UseCases.Pages;
using Contact.Domain.Entities;
using Contact.Domain.Interfaces;
using Contact.Application.UseCases.Pages;
using AutoMapper;

namespace Contact.Application.Services;

public class PageService :GenericService<Page, PageResponse, CreatePage, UpdatePage>, IPageService
{
    public PageService(IGenericRepository<Page> repository, IMapper mapper, IUnitOfWork unitOfWork)
        : base(repository, mapper, unitOfWork)
    {
    }

    
}
