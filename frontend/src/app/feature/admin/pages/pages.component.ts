import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { PageService } from './page.service';
import { Page } from "@core/models/page.interface";
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-pages',
  templateUrl: './pages.component.html',
  styleUrls: ['./pages.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatTableModule,
    MatIconModule
  ]
})
export class PagesComponent implements OnInit {
  private fb = inject(FormBuilder);
  private pageService = inject(PageService);
  private snackBar = inject(MatSnackBar);
  private dialog = inject(MatDialog);

  pages = signal<Page[]>([]);
  selectedPage = signal<Page | null>(null);
  pageForm = this.fb.group({
    name: ['', Validators.required],
    url: ['', Validators.required]
  });
  
  displayedColumns: string[] = ['name', 'url', 'actions'];

  ngOnInit(): void {
    this.loadPages();
  }

  loadPages(): void {
    this.pageService.getPages().subscribe({
      next: (pages) => this.pages.set(pages),
      error: (err) => this.snackBar.open('Failed to load pages', 'Close', { duration: 3000 })
    });
  }

  selectPage(page: Page): void {
    this.selectedPage.set(page);
    this.pageForm.patchValue(page);
  }

  savePage(): void {
    if (this.pageForm.invalid) {
      return;
    }

    const pageData = this.pageForm.value as Page;
    if (this.selectedPage()) {
      this.pageService.updatePage(this.selectedPage()!.id, pageData).subscribe({
        next: () => {
          this.snackBar.open('Page updated successfully', 'Close', { duration: 3000 });
          this.loadPages();
          this.resetForm();
        },
        error: (err) => this.snackBar.open('Failed to update page', 'Close', { duration: 3000 })
      });
    } else {
      this.pageService.createPage(pageData).subscribe({
        next: () => {
          this.snackBar.open('Page created successfully', 'Close', { duration: 3000 });
          this.loadPages();
          this.resetForm();
        },
        error: (err) => this.snackBar.open('Failed to create page', 'Close', { duration: 3000 })
      });
    }
  }

  deletePage(page: Page): void {
    this.pageService.deletePage(page.id).subscribe({
      next: () => {
        this.snackBar.open('Page deleted successfully', 'Close', { duration: 3000 });
        this.loadPages();
      },
      error: (err) => this.snackBar.open('Failed to delete page', 'Close', { duration: 3000 })
    });
  }

  resetForm(): void {
    this.selectedPage.set(null);
    this.pageForm.reset();
  }
}
