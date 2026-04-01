import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', loadComponent: () => import('./components/home/home').then(m => m.HomeComponent) },
  { path: 'hotels', loadComponent: () => import('./components/hotel-list/hotel-list').then(m => m.HotelListComponent) },
  { path: 'hotels/:id', loadComponent: () => import('./components/hotel-detail/hotel-detail').then(m => m.HotelDetailComponent) },
  { path: 'login', loadComponent: () => import('./components/login/login').then(m => m.LoginComponent) },
  { path: 'register', loadComponent: () => import('./components/register/register').then(m => m.RegisterComponent) },
  { path: 'checkout', loadComponent: () => import('./components/booking-checkout/booking-checkout').then(m => m.BookingCheckoutComponent), canActivate: [authGuard] },
  { path: 'booking-confirmation/:id', loadComponent: () => import('./components/booking-confirmation/booking-confirmation').then(m => m.BookingConfirmationComponent), canActivate: [authGuard] },
  { path: 'my-bookings', loadComponent: () => import('./components/my-bookings/my-bookings').then(m => m.MyBookingsComponent), canActivate: [authGuard] },
  { path: '**', redirectTo: '' }
];
