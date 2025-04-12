import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { OperationService } from '@core/services/operation.service';
import { Operation } from '@core/models/operation.interface';

@Component({
  selector: 'app-operations',
  templateUrl: './operations.component.html',
  styleUrls: ['./operations.component.scss']
})
export class OperationsComponent implements OnInit {
  private fb = inject(FormBuilder);
  private operationService = inject(OperationService);
  private snackBar = inject(MatSnackBar);
  private dialog = inject(MatDialog);

  operations = signal<Operation[]>([]);
  selectedOperation = signal<Operation | null>(null);
  operationForm: FormGroup;

  constructor() {
    this.operationForm = this.fb.group({
      name: ['', Validators.required],
      description: ['']
    });
  }

  ngOnInit(): void {
    this.loadOperations();
  }

  loadOperations(): void {
    this.operationService.getOperations().subscribe({
      next: (operations) => this.operations.set(operations),
      error: (err) => this.snackBar.open('Failed to load operations', 'Close', { duration: 3000 })
    });
  }

  selectOperation(operation: Operation): void {
    this.selectedOperation.set(operation);
    this.operationForm.patchValue(operation);
  }

  saveOperation(): void {
    if (this.operationForm.invalid) {
      return;
    }

    const operationData = this.operationForm.value as Operation;
    if (this.selectedOperation()) {
      this.operationService.updateOperation(this.selectedOperation()!.id, operationData).subscribe({
        next: () => {
          this.snackBar.open('Operation updated successfully', 'Close', { duration: 3000 });
          this.loadOperations();
          this.resetForm();
        },
        error: (err) => this.snackBar.open('Failed to update operation', 'Close', { duration: 3000 })
      });
    } else {
      this.operationService.createOperation(operationData).subscribe({
        next: () => {
          this.snackBar.open('Operation created successfully', 'Close', { duration: 3000 });
          this.loadOperations();
          this.resetForm();
        },
        error: (err) => this.snackBar.open('Failed to create operation', 'Close', { duration: 3000 })
      });
    }
  }

  deleteOperation(operation: Operation): void {
    this.operationService.deleteOperation(operation.id).subscribe({
      next: () => {
        this.snackBar.open('Operation deleted successfully', 'Close', { duration: 3000 });
        this.loadOperations();
      },
      error: (err) => this.snackBar.open('Failed to delete operation', 'Close', { duration: 3000 })
    });
  }

  resetForm(): void {
    this.selectedOperation.set(null);
    this.operationForm.reset();
  }
}
