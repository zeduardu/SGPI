import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MessageContainer } from './message-container';

describe('MessageContainer', () => {
  let component: MessageContainer;
  let fixture: ComponentFixture<MessageContainer>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MessageContainer]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MessageContainer);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
