import { Component, computed, inject, signal, PLATFORM_ID, HostListener, ViewChild } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSidenav } from '@angular/material/sidenav';
import { HeaderComponent } from '@core/layout/header/header.component';
import { CustomSidenavComponent } from '@core/layout/custom-sidenav/custom-sidenav.component';
import { FooterComponent } from '@core/layout/footer/footer.component';
import { CommonModule, isPlatformBrowser } from '@angular/common';
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
  @ViewChild(MatSidenav) sidenav?: MatSidenav;

  private themeService = inject(ThemeService);
  private platformId = inject(PLATFORM_ID);

  collapsed = signal<boolean>(true); // Set default to true to keep menu closed
  loading = signal<boolean>(false);
  isDarkMode = signal<boolean>(false);
  isMobile = signal<boolean>(false);

  sidenavWidth = computed(() => {
    if (this.isMobile() && this.collapsed()) {
      return '0px';
    }
    return this.collapsed() ? '65px' : '250px';
  });

  @HostListener('window:resize', ['$event'])
  onResize() {
    this.checkScreenSize();
  }

  private checkScreenSize() {
    if (isPlatformBrowser(this.platformId)) {
      this.isMobile.set(window.innerWidth < 768);
      if (this.isMobile()) {
        this.collapsed.set(true);
      }
    }
  }

  toggleCollapsed() {
    this.collapsed.update(value => !value);
    if (this.isMobile()) {
      if (this.collapsed()) {
        this.sidenav?.close();
      }
    }
  }

  ngOnInit() {
    this.checkScreenSize();
    // Ensure menu starts collapsed on mobile
    if (this.isMobile()) {
      this.collapsed.set(true);
      if (this.sidenav?.opened) {
        this.sidenav.close();
      }
    }
    if (isPlatformBrowser(this.platformId)) {
      this.isDarkMode.set(document.documentElement.classList.contains('dark'));

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

  ngAfterViewInit() {
    // Ensure sidenav is closed on mobile after view init
    if (this.isMobile() && this.sidenav?.opened) {
      this.sidenav.close();
    }
  }
}
