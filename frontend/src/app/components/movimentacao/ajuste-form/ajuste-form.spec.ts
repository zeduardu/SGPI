import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AjusteForm } from './ajuste-form';

describe('AjusteForm', () => {
  let component: AjusteForm;
  let fixture: ComponentFixture<AjusteForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AjusteForm]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AjusteForm);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
