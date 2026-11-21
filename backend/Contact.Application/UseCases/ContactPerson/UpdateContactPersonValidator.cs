namespace Contact.Application.UseCases.ContactPerson;

/// <summary>Same field rules as create. <see cref="UpdateContactPerson.Id"/> comes from the route, not the body.</summary>
public class UpdateContactPersonValidator : ContactPersonDetailsValidator<UpdateContactPerson>;
