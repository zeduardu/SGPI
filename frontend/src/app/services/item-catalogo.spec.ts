import { TestBed } from '@angular/core/testing';

import { ItemCatalogo } from './item-catalogo';

describe('ItemCatalogo', () => {
  let service: ItemCatalogo;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ItemCatalogo);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
