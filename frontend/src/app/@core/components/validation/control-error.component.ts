import { CommonModule } from '@angular/common';
import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  ElementRef,
  TemplateRef,
  inject,
} from '@angular/core';
import { ValidationErrors } from '@angular/forms';

export type ErrorComponentTemplate = TemplateRef<{ $implicit: ValidationErrors; text: string }>;

export interface ControlErrorComponent {
  customClass: string | string[];
  text: string | null;
  createTemplate?(tpl: ErrorComponentTemplate, error: ValidationErrors, text: string): void;
}

@Component({
  selector: 'control-error',
  standalone: true,
  imports: [CommonModule],
  template: `
    @if (!errorTemplate && text) {
      <div class="error-message" [class.hide-control]="hideError">{{ text }}</div>
    }
    <ng-template *ngTemplateOutlet="errorTemplate; context: errorContext"></ng-template>
  `,
  styles: [`
    :host {
      display: block;
      position: absolute;
      left: 0;
      bottom: -20px;
      width: 100%;
      z-index: 1;
    }

    .error-message {
      color: var(--mat-sys-error, #f44336);
      font-size: 0.75rem;
      line-height: 1.2;
      text-align: left;
      padding: 0.25rem 0;
    }

    .hide-control {
      display: none !important;
    }
  `],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DefaultControlErrorComponent implements ControlErrorComponent {
  errorTemplate: ErrorComponentTemplate | undefined;
  errorContext: { $implicit: ValidationErrors; text: string };
  hideError = true;

  private cdr = inject(ChangeDetectorRef);
  private host = inject(ElementRef<HTMLElement>);
  private _addClasses: string[] = [];
  private _text: string | null = null;

  createTemplate(tpl: ErrorComponentTemplate, error: ValidationErrors, text: string) {
    this.errorTemplate = tpl;
    this.errorContext = { $implicit: error, text };
    this.cdr.markForCheck();
  }

  set customClass(classes: string | string[]) {
    if (!this.hideError) {
      this._addClasses = Array.isArray(classes) ? classes : classes.split(/\s/);
      this.host.nativeElement.classList.add(...this._addClasses);
    }
  }

  set text(value: string | null) {
    if (value !== this._text) {
      this._text = value;
      this.hideError = !value;

      if (this.hideError) {
        this.host.nativeElement.classList.remove(...this._addClasses);
      }
      this.cdr.markForCheck();
    }
  }

  get text(): string | null {
    return this._text;
  }
}
