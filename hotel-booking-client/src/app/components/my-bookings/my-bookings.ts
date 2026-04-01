import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { BookingService } from '../../services/booking.service';
import { BookingResponse } from '../../models/booking.model';

@Component({
  selector: 'app-my-bookings',
  standalone: true,
  imports: [RouterLink, FormsModule, DatePipe],
  templateUrl: './my-bookings.html',
  styleUrl: './my-bookings.scss'
})
export class MyBookingsComponent implements OnInit {
  bookings: BookingResponse[] = [];
  loading = true;
  rebookingId: number | null = null;
  newCheckIn = '';
  newCheckOut = '';
  error = '';

  constructor(
    private bookingService: BookingService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadBookings();
  }

  loadBookings(): void {
    this.loading = true;
    this.bookingService.getUserBookings().subscribe({
      next: (data) => {
        this.bookings = data;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  cancelBooking(id: number): void {
    if (!confirm('Are you sure you want to cancel this booking?')) return;
    this.bookingService.cancelBooking(id).subscribe({
      next: () => {
        this.loadBookings();
      },
      error: (err) => {
        this.error = err.error?.message || 'Failed to cancel booking';
      }
    });
  }

  startRebook(booking: BookingResponse): void {
    this.rebookingId = booking.id;
    this.newCheckIn = '';
    this.newCheckOut = '';
  }

  confirmRebook(id: number): void {
    if (!this.newCheckIn || !this.newCheckOut) return;
    this.bookingService.rebookBooking(id, this.newCheckIn, this.newCheckOut).subscribe({
      next: () => {
        this.rebookingId = null;
        this.loadBookings();
      },
      error: (err) => {
        this.error = err.error?.message || 'Failed to rebook';
      }
    });
  }

  cancelRebook(): void {
    this.rebookingId = null;
  }

  getStatusClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'confirmed': return 'bg-success';
      case 'cancelled': return 'bg-danger';
      case 'completed': return 'bg-secondary';
      default: return 'bg-info';
    }
  }
}
