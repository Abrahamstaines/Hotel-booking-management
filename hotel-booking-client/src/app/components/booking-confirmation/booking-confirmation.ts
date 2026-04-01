import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';
import { BookingResponse } from '../../models/booking.model';

@Component({
  selector: 'app-booking-confirmation',
  standalone: true,
  imports: [RouterLink, DatePipe],
  templateUrl: './booking-confirmation.html',
  styleUrl: './booking-confirmation.scss'
})
export class BookingConfirmationComponent implements OnInit {
  booking: BookingResponse | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    const nav = this.router.getCurrentNavigation();
    this.booking = nav?.extras?.state?.['booking'] || history.state?.booking || null;

    if (!this.booking) {
      this.router.navigate(['/my-bookings']);
    }
  }
}
