import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SaidaForm } from './saida-form';

describe('SaidaForm', () => {
  let component: SaidaForm;
  let fixture: ComponentFixture<SaidaForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SaidaForm]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SaidaForm);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
