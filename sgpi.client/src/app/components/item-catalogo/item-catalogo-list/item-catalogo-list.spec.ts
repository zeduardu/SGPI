import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ItemCatalogoList } from './item-catalogo-list';

describe('ItemCatalogoList', () => {
  let component: ItemCatalogoList;
  let fixture: ComponentFixture<ItemCatalogoList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ItemCatalogoList]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ItemCatalogoList);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
