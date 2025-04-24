import { Component, inject, signal, PLATFORM_ID, OnInit } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatRippleModule } from '@angular/material/core';
import { RouterModule } from '@angular/router';
import { environment } from '@environments/environment';
import { ThemeService } from '@core/services/theme.service';

interface TechnologyCard {
  icon: string;
  iconType: 'material';
  title: string;
  description: string;
}

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    CommonModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule,
    MatRippleModule,
    RouterModule
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {
  private platformId = inject(PLATFORM_ID);
  private themeService = inject(ThemeService);

  name = signal('Contacts Portal');
  isDarkMode = signal(false);

  // Technology cards data with proper Material icons
  technologies = signal<TechnologyCard[]>([
    {
      icon: 'storage',
      iconType: 'material',
      title: 'PostgreSQL',
      description: 'PostgreSQL used as relational database for storing contacts and user data securely.'
    },
    {
      icon: 'code',
      iconType: 'material',
      title: '.NET 9',
      description: 'Authentication and all REST services are developed using .NET 9 with Clean Architecture.'
    },
    {
      icon: 'web',
      iconType: 'material',
      title: 'Angular 19',
      description: 'Front end interfaces built with Angular 19 using signals and functional components.'
    },
    {
      icon: 'palette',
      iconType: 'material',
      title: 'Material Design',
      description: 'UI components from Angular Material with custom theming for consistent look and feel.'
    },
    {
      icon: 'integration_instructions',
      iconType: 'material',
      title: 'Docker',
      description: 'Containerized application for consistent development, testing, and production environments.'
    },
    {
      icon: 'published_with_changes',
      iconType: 'material',
      title: 'CI/CD',
      description: 'Automated testing, building, and deployment of the application using GitHub Actions.'
    }
  ]);

  ngOnInit() {
    if (isPlatformBrowser(this.platformId)) {
      // Check for dark mode
      this.isDarkMode.set(document.documentElement.classList.contains('dark'));

      // Listen for changes in theme
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

  openGitHub() {
    if (isPlatformBrowser(this.platformId)) {
      window.open(environment.repoLink, '_blank', 'noopener,noreferrer');
    }
  }
  openDemo() {
    if (isPlatformBrowser(this.platformId)) {
      window.open("https://github.com/nitin27may/clean-architecture-docker-dotnet-angular/blob/main/docs/clean-architecture-demo.gif", '_blank', 'noopener,noreferrer');
    }
  }

  openDocumentation() { 
    if (isPlatformBrowser(this.platformId)) {
      window.open("https://nitinksingh.com/clean-architecture-docker-dotnet-angular/", '_blank', 'noopener,noreferrer');
    }
  }
}
