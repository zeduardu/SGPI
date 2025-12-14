import { Routes } from '@angular/router';
import { Dashboard } from './dashboard/dashboard';
import { Login } from './login/login';
import { CategoriaListComponent } from './components/categoria/categoria-list/categoria-list';
import { CategoriaFormComponent } from './components/categoria/categoria-form/categoria-form';
import { FornecedorListComponent } from './components/fornecedor/fornecedor-list/fornecedor-list';
import { FornecedorFormComponent } from './components/fornecedor/fornecedor-form/fornecedor-form';
import { ItemCatalogoListComponent } from './components/item-catalogo/item-catalogo-list/item-catalogo-list';
import { ItemCatalogoFormComponent } from './components/item-catalogo/item-catalogo-form/item-catalogo-form';
import { EstoqueListComponent } from './components/estoque/estoque-list/estoque-list';
import { EstoqueFormComponent } from './components/estoque/estoque-form/estoque-form';
import { CotacaoItemListComponent } from './components/cotacao-item/cotacao-item-list/cotacao-item-list';
import { CotacaoItemFormComponent } from './components/cotacao-item/cotacao-item-form/cotacao-item-form';
import { UserListComponent } from './components/user/user-list/user-list';
import { UserFormComponent } from './components/user/user-form/user-form';
import { SaidaFormComponent } from './components/movimentacao/saida-form/saida-form';
import { AjusteFormComponent } from './components/movimentacao/ajuste-form/ajuste-form';
import { OrdemCompraListComponent } from './components/ordem-compra/ordem-compra-list/ordem-compra-list';
import { OrdemCompraFormComponent } from './components/ordem-compra/ordem-compra-form/ordem-compra-form';
import { OrdemCompraDetailsComponent } from './components/ordem-compra/ordem-compra-details/ordem-compra-details';
import { adminGuard } from './auth/admin.guard';
import { authGuard } from './auth/auth.guard';

export const routes: Routes = [
  {
    path: 'reports/movements',
    loadComponent: () =>
      import('./components/reports/report-movements/report-movements').then(
        (m) => m.ReportMovementsComponent
      ),
    canActivate: [authGuard],
  },
  {
    path: 'reports/orders',
    loadComponent: () =>
      import('./components/reports/report-orders/report-orders').then(
        (m) => m.ReportOrdersComponent
      ),
    canActivate: [authGuard],
  },
  {
    path: 'shopping-list',
    loadComponent: () =>
      import('./components/shopping-list/shopping-list').then((m) => m.ShoppingListComponent),
    canActivate: [authGuard],
  },
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: Dashboard },
  { path: 'login', component: Login },
  { path: 'categorias', component: CategoriaListComponent },
  { path: 'categorias/new', component: CategoriaFormComponent },
  { path: 'categorias/:id', component: CategoriaFormComponent },
  { path: 'fornecedores', component: FornecedorListComponent },
  { path: 'fornecedores/new', component: FornecedorFormComponent },
  { path: 'fornecedores/:id', component: FornecedorFormComponent },
  { path: 'itens-catalogo', component: ItemCatalogoListComponent },
  { path: 'itens-catalogo/new', component: ItemCatalogoFormComponent },
  { path: 'itens-catalogo/:id', component: ItemCatalogoFormComponent },
  { path: 'estoque', component: EstoqueListComponent },
  { path: 'estoque/new', component: EstoqueFormComponent },
  { path: 'estoque/:id', component: EstoqueFormComponent },
  { path: 'cotacoes', component: CotacaoItemListComponent },
  { path: 'cotacoes/new', component: CotacaoItemFormComponent },
  { path: 'cotacoes/:id', component: CotacaoItemFormComponent },
  { path: 'users', component: UserListComponent, canActivate: [adminGuard] },
  { path: 'users/new', component: UserFormComponent, canActivate: [adminGuard] },
  { path: 'users/:id', component: UserFormComponent, canActivate: [adminGuard] },
  { path: 'ordem-compra', component: OrdemCompraListComponent },
  { path: 'ordem-compra/novo', component: OrdemCompraFormComponent },
  { path: 'ordem-compra/:id', component: OrdemCompraDetailsComponent },
  { path: 'movimentacao/saida', component: SaidaFormComponent },
  { path: 'movimentacao/ajuste', component: AjusteFormComponent },
];
