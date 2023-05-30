import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OutsiteHeaderComponent } from './outsite-header.component';

describe('OutsiteHeaderComponent', () => {
  let component: OutsiteHeaderComponent;
  let fixture: ComponentFixture<OutsiteHeaderComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OutsiteHeaderComponent]
    });
    fixture = TestBed.createComponent(OutsiteHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
