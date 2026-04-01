import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './home.html',
  styleUrl: './home.scss'
})
export class HomeComponent {
  city = '';
  checkIn = '';
  checkOut = '';
  guests = 1;

  constructor(private router: Router) {}

  search(): void {
    const params: any = {};
    if (this.city) params.city = this.city;
    if (this.checkIn) params.checkIn = this.checkIn;
    if (this.checkOut) params.checkOut = this.checkOut;
    if (this.guests > 1) params.guests = this.guests;
    this.router.navigate(['/hotels'], { queryParams: params });
  }
}
