import { inject } from '@angular/core';
import { ContactService } from "./contact.service";
import { ResolveFn } from '@angular/router';

export const ContactDetailsResolver: ResolveFn<any> = (route, state) => {
  let contactService = inject(ContactService);
  // Only ever registered on routes with :contactId in the path (see contact.routes.ts),
  // so the param is always present when this resolver runs.
  return contactService.getById(route.paramMap.get('contactId')!);
};
