using Contact.Api.Core.Attributes;
using Contact.Application.Interfaces;
using Contact.Application.UseCases.Pages;
using Microsoft.AspNetCore.Mvc;

namespace Contact.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PagesController(IPageService pageService) : ControllerBase
{
    [HttpPost]
    [AuthorizePermission("Pages.Create")]
    [ActivityLog("Creating new Page")]
    public async Task<IActionResult> AddPage(CreatePage createPage) =>
        Ok(await pageService.Add(createPage));

    [HttpPut("{id}")]
    [AuthorizePermission("Pages.Update")]
    [ActivityLog("Updating Page")]
    public async Task<IActionResult> UpdatePage(Guid id, UpdatePage updatePage)
    {
        updatePage.Id = id;
        return Ok(await pageService.Update(updatePage));
    }

    [HttpGet]
    [AuthorizePermission("Pages.Read")]
    [ActivityLog("Fetching all Pages")]
    public async Task<IActionResult> GetPages() =>
        Ok(await pageService.FindAll());

    [HttpDelete("{id}")]
    [AuthorizePermission("pages.Delete")]
    [ActivityLog("Deleting Page")]
    public async Task<IActionResult> DeletePage(Guid id) =>
        Ok(await pageService.Delete(id));
}
