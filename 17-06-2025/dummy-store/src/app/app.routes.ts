import { Routes } from '@angular/router';
import { Products } from './pages/products/products';
import { ProductDetail } from './pages/product-detail/product-detail';
import { Login } from './pages/login/login';
import { AuthGuard } from './guards/auth-guard';

export const routes: Routes = [
  { path: '', redirectTo: 'products', pathMatch: 'full' },
  { path: 'login', component: Login },
  {
    path: 'products',
    canActivate: [AuthGuard],
    children: [
      { path: '', component: Products },
      { path: ':id', component: ProductDetail }
    ]
  }
];
