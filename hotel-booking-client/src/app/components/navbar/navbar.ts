import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AsyncPipe } from '@angular/common';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, AsyncPipe],
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss'
})
export class NavbarComponent {
  private authService = inject(AuthService);
  private router = inject(Router);
  isLoggedIn$ = this.authService.isLoggedIn$;

  get userName(): string {
    return this.authService.getUser()?.fullName || '';
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/']);
  }
}
