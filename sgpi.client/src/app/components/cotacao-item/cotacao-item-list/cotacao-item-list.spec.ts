import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CotacaoItemList } from './cotacao-item-list';

describe('CotacaoItemList', () => {
  let component: CotacaoItemList;
  let fixture: ComponentFixture<CotacaoItemList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CotacaoItemList]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CotacaoItemList);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
