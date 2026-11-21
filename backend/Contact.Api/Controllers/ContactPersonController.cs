using Contact.Api.Core.Attributes;
using Contact.Application.Abstractions.Messaging;
using Contact.Application.UseCases.ContactPerson;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contact.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ContactPersonController(ISender sender) : ControllerBase
{
    [HttpPost]
    [ActivityLog("Creating new Contact")]
    [AuthorizePermission("Contacts.Create")]
    public async Task<IActionResult> Add(CreateContactPerson createContactPerson, CancellationToken cancellationToken)
    {
        var createdContactPerson = await sender.Send(createContactPerson, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = createdContactPerson.Id }, createdContactPerson);
    }

    [HttpPut("{id}")]
    [ActivityLog("Updating Contact")]
    [AuthorizePermission("Contacts.Update")]
    public async Task<IActionResult> Update(Guid id, UpdateContactPerson updateContactPerson, CancellationToken cancellationToken)
    {
        // The route decides which contact is updated. Previously the existence check used the
        // route id while the update used the body's Id, so a mismatched body updated another record.
        updateContactPerson.Id = id;

        var updatedContactPerson = await sender.Send(updateContactPerson, cancellationToken);
        return updatedContactPerson is null ? NotFound() : Ok(updatedContactPerson);
    }

    [HttpDelete("{id}")]
    [ActivityLog("Deleting Contact")]
    [AuthorizePermission("Contacts.Delete")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await sender.Send(new DeleteContactPerson(id), cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    [HttpGet("{id}")]
    [ActivityLog("Reading Contact By id")]
    [AuthorizePermission("Contacts.Read")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var contactPerson = await sender.Send(new GetContactPersonById(id), cancellationToken);
        return contactPerson is null ? NotFound() : Ok(contactPerson);
    }

    [HttpGet]
    [ActivityLog("Reading All Contacts")]
    [AuthorizePermission("Contacts.Read")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var contactPersons = await sender.Send(new GetAllContactPersons(), cancellationToken);
        return Ok(contactPersons);
    }
}
