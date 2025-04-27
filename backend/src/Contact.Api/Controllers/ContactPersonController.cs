using Contact.Api.Core.Attributes;
using Contact.Application.Interfaces;
using Contact.Application.UseCases.ContactPerson;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contact.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ContactPersonController(IContactPersonService contactPersonService) : ControllerBase
{
    [HttpPost]
    [ActivityLog("Creating new Contact")]
    [AuthorizePermission("Contacts.Create")]
    public async Task<IActionResult> Add(CreateContactPerson createContactPerson)
    {
        var createdContactPerson = await contactPersonService.Add(createContactPerson);
        return CreatedAtAction(nameof(GetById), new { id = createdContactPerson.Id }, createdContactPerson);
    }

    [HttpPut("{id}")]
    [ActivityLog("Updating Contact")]
    [AuthorizePermission("Contacts.Update")]
    public async Task<IActionResult> Update(Guid id, UpdateContactPerson updateContactPerson)
    {
        var contactPerson = await contactPersonService.FindByID(id);
        if (contactPerson is null) return NotFound();
        
        var updatedContactPerson = await contactPersonService.Update(updateContactPerson);
        return Ok(updatedContactPerson);
    }

    [HttpDelete("{id}")]
    [ActivityLog("Deleting Contact")]
    [AuthorizePermission("Contacts.Delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await contactPersonService.Delete(id);
        return deleted ? NoContent() : NotFound();
    }

    [HttpGet("{id}")]
    [ActivityLog("Reading Contact By id")]
    [AuthorizePermission("Contacts.Read")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var contactPerson = await contactPersonService.FindByID(id);
        return contactPerson is null ? NotFound() : Ok(contactPerson);
    }

    [HttpGet]
    [ActivityLog("Reading All Contacts")]
    [AuthorizePermission("Contacts.Read")]
    public async Task<IActionResult> GetAll()
    {
        var contactPersons = await contactPersonService.FindAll();
        return Ok(contactPersons);
    }
}
