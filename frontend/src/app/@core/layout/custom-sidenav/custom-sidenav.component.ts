import { Component, inject, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MenuItemComponent } from './menu-item/menu-item.component';
import { menuItems } from './menu-items';
import SidenavHeaderComponent from './sidenav-header/sidenav-header.component';

@Component({
  selector: 'app-custom-sidenav',
  template: `
    <app-sidenav-header [collapsed]="collapsed()" />
    <mat-nav-list class="[--mat-list-active-indicator-shape:0px] mb-6">
      @for (item of menuItems; track item.label) {
        <app-menu-item [item]="item" [collapsed]="collapsed()" />
      }
    </mat-nav-list>
  `,
  styles: [
    `
      :host * {
        transition-property: width, height, opacity;
        transition-duration: 500ms;
        transition-timing-function: ease-in-out;
      }
    `,
  ],
  imports: [
    CommonModule,
    MatSidenavModule,
    MatListModule,
    RouterModule,
    MatIconModule,
    MenuItemComponent,
    SidenavHeaderComponent,
  ],
  standalone: true,
})
export class CustomSidenavComponent {
  // Use the newer input API for signal-based inputs
  collapsed = input<boolean>(false);

  menuItems = menuItems;
}
