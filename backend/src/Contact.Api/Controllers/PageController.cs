using Contact.Application.Interfaces;
using Contact.Application.UseCases.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contact.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PageController : ControllerBase
{
    private readonly IPageService _pageService;

    public PageController(IPageService pageService)
    {
        _pageService = pageService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddPage(CreatePage createPage)
    {
        var response = await _pageService.Add(createPage);
        return Ok(response);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdatePage(Guid id, UpdatePage updatePage)
    {
        updatePage.Id = id;
        var response = await _pageService.Update(updatePage);
        return Ok(response);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetPages()
    {
        var response = await _pageService.FindAll();
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeletePage(Guid id)
    {
        var response = await _pageService.Delete(id);
        return Ok(response);
    }
}
