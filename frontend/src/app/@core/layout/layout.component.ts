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

  // Set default to false for expanded menu on desktop
  collapsed = signal<boolean>(false);
  loading = signal<boolean>(false);
  isDarkMode = signal<boolean>(false);
  isMobile = signal<boolean>(false);

  private resizeTimeout: any;

  sidenavWidth = computed(() => {
    if (this.isMobile() && this.collapsed()) {
      return '0px';
    }
    return this.collapsed() ? '65px' : '250px';
  });

  @HostListener('window:resize', ['$event'])
  onResize() {
    if (isPlatformBrowser(this.platformId)) {
      // Add debounce to avoid multiple rapid calls
      clearTimeout(this.resizeTimeout);
      this.resizeTimeout = setTimeout(() => {
        this.checkScreenSize();
      }, 100);
    }
  }

  private checkScreenSize() {
    if (isPlatformBrowser(this.platformId)) {
      const newIsMobile = window.innerWidth < 768;
      
      // Force close sidenav immediately if switching to mobile
      if (newIsMobile && !this.isMobile()) {
        this.sidenav?.close();
        this.collapsed.set(true);
      }
      
      this.isMobile.set(newIsMobile);

      if (!newIsMobile) {
        this.collapsed.set(false);
        this.sidenav?.open();
      }
    }
  }

  toggleCollapsed() {
    this.collapsed.update(value => !value);
    if (this.isMobile()) {
      if (this.collapsed()) {
        this.sidenav?.close();
      } else {
        this.sidenav?.open();
      }
    }
  }

  ngOnInit() {
    if (isPlatformBrowser(this.platformId)) {
      // Initial screen size check
      this.checkScreenSize();
      
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
    // Initial sidenav state based on screen size
    if (this.isMobile()) {
      this.collapsed.set(true);
      setTimeout(() => this.sidenav?.close(), 0);
    } else {
      this.collapsed.set(false);
      setTimeout(() => this.sidenav?.open(), 0);
    }
  }

  ngOnDestroy() {
    if (this.resizeTimeout) {
      clearTimeout(this.resizeTimeout);
    }
  }
}
