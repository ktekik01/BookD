import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AskToAIComponent } from './ask-to-ai.component';

describe('AskToAIComponent', () => {
  let component: AskToAIComponent;
  let fixture: ComponentFixture<AskToAIComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AskToAIComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AskToAIComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
