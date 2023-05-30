import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InsiteHeaderComponent } from './insite-header.component';

describe('InsiteHeaderComponent', () => {
  let component: InsiteHeaderComponent;
  let fixture: ComponentFixture<InsiteHeaderComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [InsiteHeaderComponent]
    });
    fixture = TestBed.createComponent(InsiteHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
