using Contact.Application.Interfaces;
using Contact.Application.UseCases.Operations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contact.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OperationController : ControllerBase
{
    private readonly IOperationService _operationService;

    public OperationController(IOperationService operationService)
    {
        _operationService = operationService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddOperation(CreateOperation createOperation)
    {
        var response = await _operationService.Add(createOperation);
        return Ok(response);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateOperation(Guid id, UpdateOperation updateOperation)
    {
        updateOperation.Id = id;
        var response = await _operationService.Update(updateOperation);
        return Ok(response);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetOperations()
    {
        var response = await _operationService.FindAll();
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteOperation(Guid id)
    {
        var response = await _operationService.Delete(id);
        return Ok(response);
    }
}
