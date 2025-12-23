import { TestBed } from '@angular/core/testing';

import { CotacaoItem } from './cotacao-item';

describe('CotacaoItem', () => {
  let service: CotacaoItem;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CotacaoItem);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
