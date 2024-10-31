using Contact.Api.Core.Attributes;
using Contact.Application.Interfaces;
using Contact.Application.UseCases.ContactPerson;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contact.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ContactPersonController : ControllerBase
{
    private readonly IContactPersonService _contactPersonService;

    public ContactPersonController(IContactPersonService contactPersonService)
    {
        _contactPersonService = contactPersonService;
    }

    [HttpPost]
    [ActivityLog("Creating new Contact")]
    [AuthorizePermission("Contacts.Create")]
    public async Task<IActionResult> Add(CreateContactPerson createContactPerson)
    {
        var createdContactPerson = await _contactPersonService.Add(createContactPerson);
        return CreatedAtAction(nameof(GetById), new { id = createdContactPerson.Id }, createdContactPerson);
    }

    [HttpPut("{id}")]
    [ActivityLog("Updating Contact")]
    [AuthorizePermission("Contacts.Update")]
    public async Task<IActionResult> Update(UpdateContactPerson updateContactPerson)
    {
        var updatedContactPerson = await _contactPersonService.Update(updateContactPerson);
        return Ok(updatedContactPerson);
    }

    [HttpDelete("{id}")]
    [ActivityLog("Deleting Contact")]
    [AuthorizePermission("Contacts.Delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _contactPersonService.Delete(id);
        if (!deleted) return NotFound();
        return NoContent();
    }

    [HttpGet("{id}")]
    [ActivityLog("Reading Contact By id")]
    [AuthorizePermission("Contacts.Read")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var contactPerson = await _contactPersonService.FindByID(id);
        if (contactPerson == null) return NotFound();
        return Ok(contactPerson);
    }

    [HttpGet]
    [ActivityLog("Reading All Contacts")]
    [AuthorizePermission("Contacts.Read")]
    public async Task<IActionResult> GetAll()
    {
        var contactPersons = await _contactPersonService.FindAll();
        return Ok(contactPersons);
    }
}
