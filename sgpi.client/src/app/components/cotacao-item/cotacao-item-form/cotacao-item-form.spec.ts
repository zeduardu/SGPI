import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CotacaoItemForm } from './cotacao-item-form';

describe('CotacaoItemForm', () => {
  let component: CotacaoItemForm;
  let fixture: ComponentFixture<CotacaoItemForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CotacaoItemForm]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CotacaoItemForm);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
