# .NET 8+ Best Practices Instructions

# GitHub Copilot Instructions for .NET 8+

Generate code following these modern .NET 8+ best practices:

## C# Language and Syntax
- Always maintain semicolons after namespace declarations
- Use C# 12+ features including primary constructors, required properties, and file-scoped namespaces
- Implement pattern matching for complex conditionals
- Use nullable reference types with proper null checks everywhere
- Use records for DTOs and immutable data structures
- Use init-only properties for immutable objects
- Use collection expressions where appropriate

## Logging
- Include ILogger in all classes using constructor injection
- Follow proper log level usage:
  - LogDebug: Detailed information for debugging
  - LogInformation: General operational information and successful operations
  - LogWarning: Non-critical issues or unexpected behavior
  - LogError: Errors and exceptions that prevent normal operation
  - LogCritical: Critical failures requiring immediate attention
- Use structured logging with proper context:
  - Always include correlation IDs in logs
  - Use semantic logging with named parameters (e.g., `_logger.LogInformation("User {UserId} performed {Action}", userId, action)`)
- Add logging for all major operations and exception paths

## API Endpoints and Controllers
- Use minimal APIs for simple CRUD operations
- Use controller-based APIs for complex business logic
- Apply consistent HTTP status codes based on operation outcomes
- Use proper route patterns with versioning support
- Always use attribute routing
- Return ActionResult<T> from controller methods
- Apply authorization and custom attributes to each endpoint

## Custom Attributes
- Apply ActivityLog attribute to all action methods to capture user activities:
  - Format: `[ActivityLog("Descriptive action message")]`
- Apply AuthorizePermission attribute for fine-grained authorization:
  - Format: `[AuthorizePermission("Resource.Operation")]`
- Place custom attributes before standard HTTP method attributes

## Input Validation
- Use FluentValidation for complex validation rules
- Implement proper model validation with consistent error responses
- Return ProblemDetails for validation errors
- Validate request models early in the pipeline

## Exception Handling
- Implement global exception handling middleware
- Use custom exception types for different business scenarios
- Return appropriate status codes based on exception types
- Include correlation IDs in error responses
- Never expose sensitive information in error messages

## Authentication & Authorization
- Use JWT authentication with proper token validation
- Implement role and policy-based authorization
- Apply AuthorizePermission attributes consistently
- Use scoped authorization policies
- Implement proper refresh token mechanisms

## Performance
- Use asynchronous methods throughout the application
- Implement appropriate caching strategies
- Use output caching for GET endpoints
- Implement rate limiting and request throttling
- Use minimal serialization options

## API Documentation
- Include comprehensive Swagger/OpenAPI documentation
- Add proper XML comments for all public APIs
- Provide examples for request/response models
- Document all possible response codes

## Sample Controller Implementation

```
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

```