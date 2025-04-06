import { Component, DestroyRef, effect, inject, input, OnInit, output, signal, PLATFORM_ID } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatToolbar } from '@angular/material/toolbar';
import { MatMenuModule } from '@angular/material/menu';
import { Router } from '@angular/router';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { ThemeService } from '@core/services/theme.service';
import { AuthStateService } from '@core/services/auth-state.service';
import { LoginService } from "@features/user/login/login.service";

interface UserProfile {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
}

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, MatToolbar, MatIcon, MatButtonModule, MatMenuModule],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  private authState = inject(AuthStateService);
  private loginService = inject(LoginService);
  private themeService = inject(ThemeService);
  router = inject(Router);
  private destroyRef = inject(DestroyRef);
  private platformId = inject(PLATFORM_ID);

  // Updated to use input/output using the Angular 19 signals API
  collapsed = input.required<boolean>();
  toggleCollapsed = output<void>();

  user = this.authState.getCurrentUser();
  isDarkMode = this.themeService.isDarkMode;

  // Toggle dark mode using the theme service
  toggleDarkMode() {
    this.themeService.toggleTheme();
  }

  // Method to emit the toggle event
  onToggleCollapsed() {
    this.toggleCollapsed.emit();
  }

  ngOnInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      const savedUser = localStorage.getItem('currentUser');
      if (savedUser) {
        this.authState.initializeFromStorage();
      }
    }
  }

  logout() {
    this.loginService.logout();
  }

  navigateToProfile() {
    this.router.navigate(['/profile']);
  }
}
