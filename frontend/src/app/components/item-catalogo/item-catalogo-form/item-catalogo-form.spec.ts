import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ItemCatalogoForm } from './item-catalogo-form';

describe('ItemCatalogoForm', () => {
  let component: ItemCatalogoForm;
  let fixture: ComponentFixture<ItemCatalogoForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ItemCatalogoForm]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ItemCatalogoForm);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
