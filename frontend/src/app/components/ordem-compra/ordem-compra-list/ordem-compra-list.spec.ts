import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrdemCompraList } from './ordem-compra-list';

describe('OrdemCompraList', () => {
  let component: OrdemCompraList;
  let fixture: ComponentFixture<OrdemCompraList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OrdemCompraList]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OrdemCompraList);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
