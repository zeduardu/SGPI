import { TestBed } from '@angular/core/testing';

import { OrdemCompra } from './ordem-compra';

describe('OrdemCompra', () => {
  let service: OrdemCompra;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(OrdemCompra);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
