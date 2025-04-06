import { Component, computed, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { HeaderComponent } from '@core/layout/header/header.component';
import { CustomSidenavComponent } from '@core/layout/custom-sidenav/custom-sidenav.component';
import { FooterComponent } from '@core/layout/footer/footer.component';
import { CommonModule } from '@angular/common';
import { ThemeService } from '@core/services/theme.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    MatSidenavModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatProgressBarModule,
    HeaderComponent,
    CustomSidenavComponent,
    FooterComponent
  ]
})
export default class LayoutComponent {
  private themeService = inject(ThemeService);

  // Use a signal for collapsed state
  collapsed = signal<boolean>(false);
  loading = signal<boolean>(false);
  isDarkMode = signal<boolean>(false);

  // Computed property for sidenav width based on collapsed state
  sidenavWidth = computed(() => (this.collapsed() ? '65px' : '250px'));

  // Toggle collapsed state
  toggleCollapsed() {
    this.collapsed.update(value => !value);
  }

  ngOnInit() {
    // Get initial dark mode state
    this.isDarkMode.set(document.documentElement.classList.contains('dark'));

    // Listen for theme changes
    const observer = new MutationObserver((mutations) => {
      mutations.forEach((mutation) => {
        if (mutation.attributeName === 'class') {
          this.isDarkMode.set(document.documentElement.classList.contains('dark'));
        }
      });
    });

    observer.observe(document.documentElement, { attributes: true });
  }
}
