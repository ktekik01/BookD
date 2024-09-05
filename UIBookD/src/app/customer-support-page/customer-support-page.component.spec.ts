import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerSupportPageComponent } from './customer-support-page.component';

describe('CustomerSupportPageComponent', () => {
  let component: CustomerSupportPageComponent;
  let fixture: ComponentFixture<CustomerSupportPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomerSupportPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CustomerSupportPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
