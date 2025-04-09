import { Component, computed, inject, input } from '@angular/core';
import { NgClass, CommonModule } from '@angular/common';
import { AuthStateService } from '@core/services/auth-state.service';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-sidenav-header',
  standalone: true,
  imports: [NgClass, MatIconModule, CommonModule],
  template: `
    <div class="pt-6 flex flex-col items-center">
      <div
        class="rounded-full mb-3 aspect-square flex items-center justify-center bg-primary text-white overflow-hidden transition-all duration-500"
        [style.width.px]="profilePicSize()"
        [style.height.px]="profilePicSize()"
      >
        @if (currentUser()?.firstName && currentUser()?.lastName) {
          <span [class.text-xl]="!collapsed()" [class.text-xs]="collapsed()">
            {{currentUser()?.firstName[0]}}{{currentUser()?.lastName[0]}}
          </span>
        } @else {
          <span [class.text-xl]="!collapsed()" [class.text-xs]="collapsed()">
            U
          </span>
        }
      </div>
      <div
        class="text-center mb-2 transition-all duration-500 overflow-hidden"
        [ngClass]="collapsed() ? 'h-0 opacity-0' : 'h-[3rem] opacity-100'"
      >
        <h2 class="text-lg font-medium">{{ currentUser()?.firstName }} {{ currentUser()?.lastName }}</h2>
        <p class="text-sm opacity-80">{{ currentUser()?.email }}</p>
      </div>
    </div>
  `,
  styles: `
    :host * {
      transition-property: width, height, opacity, transform, background-color;
      transition-duration: 500ms;
      transition-timing-function: ease-in-out;
    }
  `,
})
export default class SidenavHeaderComponent {
  // Update to use the new signal-based input API
  collapsed = input(false);

  private authState = inject(AuthStateService);
  currentUser = this.authState.getCurrentUser();

  profilePicSize = computed(() => (this.collapsed() ? 32 : 100));
}
