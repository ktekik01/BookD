import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChattingPageComponent } from './chatting-page.component';

describe('ChattingPageComponent', () => {
  let component: ChattingPageComponent;
  let fixture: ComponentFixture<ChattingPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ChattingPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChattingPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
