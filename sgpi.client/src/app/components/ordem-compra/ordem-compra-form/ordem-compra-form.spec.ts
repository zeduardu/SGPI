import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrdemCompraForm } from './ordem-compra-form';

describe('OrdemCompraForm', () => {
  let component: OrdemCompraForm;
  let fixture: ComponentFixture<OrdemCompraForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OrdemCompraForm]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OrdemCompraForm);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
