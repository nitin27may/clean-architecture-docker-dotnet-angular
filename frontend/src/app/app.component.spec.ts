import { TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';

describe('AppComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AppComponent],
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  // The 'title' property and rendered <h1> tested here previously were leftover
  // Angular CLI scaffold boilerplate — AppComponent is just a <router-outlet /> shell
  // (see app.component.ts) and never had either. Removed rather than reintroduced,
  // since adding a fake title/h1 to production code purely to satisfy a stale test
  // would be backwards.
});
