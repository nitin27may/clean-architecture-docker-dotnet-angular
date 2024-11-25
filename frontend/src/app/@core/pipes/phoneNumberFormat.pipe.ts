import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'phoneNumberFormat',
  standalone: true
})
export class PhoneNumberFormatPipe implements PipeTransform {
  transform(phone: number, countryCode: number): string {
    if (!phone) {
      return '';
    }

    const formattedPhone = phone.toString().replace(/(\d{3})(\d{3})(\d{4})/, '$1-$2-$3');
    return `+${countryCode} ${formattedPhone}`;
  }
}
