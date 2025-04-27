using Contact.Api.Core.Attributes;
using Contact.Application.Interfaces;
using Contact.Application.UseCases.Operations;
using Microsoft.AspNetCore.Mvc;

namespace Contact.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OperationsController(IOperationService operationService) : ControllerBase
{
    [HttpPost]
    [AuthorizePermission("Operations.Create")]
    [ActivityLog("Creating new Operation")]
    public async Task<IActionResult> AddOperation(CreateOperation createOperation) =>
        Ok(await operationService.Add(createOperation));

    [HttpPut("{id}")]
    [AuthorizePermission("Operations.Update")]
    [ActivityLog("Updating Operation")]
    public async Task<IActionResult> UpdateOperation(Guid id, UpdateOperation updateOperation)
    {
        updateOperation.Id = id;
        return Ok(await operationService.Update(updateOperation));
    }

    [HttpGet]
    [AuthorizePermission("Operations.Read")]
    [ActivityLog("Fetching all Operations")]
    public async Task<IActionResult> GetOperations() =>
        Ok(await operationService.FindAll());

    [HttpDelete("{id}")]
    [AuthorizePermission("Operations.Delete")]
    [ActivityLog("Deleting Operation")]
    public async Task<IActionResult> DeleteOperation(Guid id) =>
        Ok(await operationService.Delete(id));
}
