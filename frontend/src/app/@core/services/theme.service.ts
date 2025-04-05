import { DOCUMENT } from '@angular/common';
import { Injectable, computed, effect, inject, signal } from '@angular/core';

export type ThemeName = 'light' | 'dark';
export type ThemeColor = 'azure' | 'indigo' | 'purple' | 'teal' | 'green' | 'amber' | 'red';

export interface AppTheme {
  name: ThemeName;
  icon: string;
}

export interface ThemeColorOption {
  name: ThemeColor;
  color: string;
  primaryHex: string;
}

@Injectable({
  providedIn: 'root',
})
export class ThemeService {
  private document = inject(DOCUMENT);

  // Create signal for the theme state
  private appTheme = signal<ThemeName>('light');
  private themeColor = signal<ThemeColor>('azure');

  private themes: AppTheme[] = [
    { name: 'light', icon: 'light_mode' },
    { name: 'dark', icon: 'dark_mode' },
  ];

  private themeColors: ThemeColorOption[] = [
    { name: 'azure', color: 'Azure Blue', primaryHex: '#365E9D' },
    { name: 'indigo', color: 'Indigo', primaryHex: '#3F51B5' },
    { name: 'purple', color: 'Purple', primaryHex: '#9C27B0' },
    { name: 'teal', color: 'Teal', primaryHex: '#009688' },
    { name: 'green', color: 'Green', primaryHex: '#4CAF50' },
    { name: 'amber', color: 'Amber', primaryHex: '#FFC107' },
    { name: 'red', color: 'Red', primaryHex: '#F44336' }
  ];

  // Create computed signals for easier use
  selectedTheme = computed(() => this.themes.find((t) => t.name === this.appTheme()));
  selectedColor = computed(() => this.themeColors.find((t) => t.name === this.themeColor()));
  isDarkMode = computed(() => this.appTheme() === 'dark');

  // Effect to apply theme changes to the DOM
  private applyThemeEffect = effect(() => {
    const theme = this.appTheme();
    if (typeof document !== 'undefined') {
      if (theme === 'dark') {
        this.document.documentElement.classList.add('dark');
      } else {
        this.document.documentElement.classList.remove('dark');
      }

      // Apply root color scheme
      this.document.documentElement.style.colorScheme = theme;
    }
  });

  getThemes() {
    return this.themes;
  }

  getThemeColors() {
    return this.themeColors;
  }

  setTheme(theme: ThemeName) {
    this.appTheme.set(theme);

    // Save to localStorage
    if (typeof localStorage !== 'undefined') {
      try {
        localStorage.setItem('theme', theme);
      } catch (error) {
        console.error('Error saving theme to localStorage:', error);
      }
    }
  }

  toggleTheme() {
    const newTheme = this.appTheme() === 'dark' ? 'light' : 'dark';
    this.setTheme(newTheme);
    return newTheme;
  }

  setThemeColor(color: ThemeColor) {
    this.themeColor.set(color);

    // Save to localStorage
    if (typeof localStorage !== 'undefined') {
      try {
        localStorage.setItem('themeColor', color);
      } catch (error) {
        console.error('Error saving theme color to localStorage:', error);
      }
    }
  }

  constructor() {
    // Initialize theme based on:
    // 1. User's saved preference
    // 2. System preference
    // 3. Default to light mode

    if (typeof window !== 'undefined') {
      try {
        const savedTheme = localStorage.getItem('theme');
        const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
        const savedColor = localStorage.getItem('themeColor');

        if (savedTheme) {
          this.setTheme(savedTheme as ThemeName);
        } else if (prefersDark) {
          this.setTheme('dark');
        }

        if (savedColor) {
          this.themeColor.set(savedColor as ThemeColor);
        }

        // Listen for system preference changes
        window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', (e) => {
          // Only update if user hasn't explicitly set a preference
          if (!localStorage.getItem('theme')) {
            this.setTheme(e.matches ? 'dark' : 'light');
          }
        });
      } catch (error) {
        console.error('Error initializing theme:', error);
      }
    }
  }
}
