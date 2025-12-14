import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrdemCompraDetails } from './ordem-compra-details';

describe('OrdemCompraDetails', () => {
  let component: OrdemCompraDetails;
  let fixture: ComponentFixture<OrdemCompraDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OrdemCompraDetails]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OrdemCompraDetails);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
