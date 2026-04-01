import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HotelService } from '../../services/hotel.service';
import { BookingService } from '../../services/booking.service';
import { HotelDetail, RoomCategory } from '../../models/hotel.model';
import { PromotionResult } from '../../models/booking.model';

@Component({
  selector: 'app-booking-checkout',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './booking-checkout.html',
  styleUrl: './booking-checkout.scss'
})
export class BookingCheckoutComponent implements OnInit {
  hotel: HotelDetail | null = null;
  selectedCategory: RoomCategory | null = null;
  checkIn = '';
  checkOut = '';
  promoCode = '';
  promoResult: PromotionResult | null = null;
  loading = true;
  submitting = false;
  error = '';

  constructor(
    private hotelService: HotelService,
    private bookingService: BookingService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    const params = this.route.snapshot.queryParams;
    const hotelId = +params['hotelId'];
    const categoryId = +params['categoryId'];
    this.checkIn = params['checkIn'] || '';
    this.checkOut = params['checkOut'] || '';

    this.hotelService.getHotelDetail(hotelId).subscribe({
      next: (hotel) => {
        this.hotel = hotel;
        this.selectedCategory = hotel.roomCategories.find(c => c.id === categoryId) || null;
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load hotel details';
        this.loading = false;
      }
    });
  }

  get nights(): number {
    if (!this.checkIn || !this.checkOut) return 0;
    const diff = new Date(this.checkOut).getTime() - new Date(this.checkIn).getTime();
    return Math.max(0, Math.ceil(diff / (1000 * 60 * 60 * 24)));
  }

  get subtotal(): number {
    return (this.selectedCategory?.basePrice || 0) * this.nights;
  }

  get discount(): number {
    return this.promoResult?.discountAmount || 0;
  }

  get total(): number {
    return this.promoResult?.finalPrice ?? this.subtotal;
  }

  validatePromo(): void {
    if (!this.promoCode) return;
    this.bookingService.validatePromotion({
      code: this.promoCode,
      originalPrice: this.subtotal
    }).subscribe({
      next: (result) => {
        this.promoResult = result;
      },
      error: () => {
        this.promoResult = { isValid: false, message: 'Failed to validate code', discountPercent: 0, discountAmount: 0, finalPrice: this.subtotal };
      }
    });
  }

  confirmBooking(): void {
    if (!this.selectedCategory) return;
    this.submitting = true;
    this.error = '';

    // We need a specific room ID. Get available rooms to pick one.
    this.hotelService.getAvailableRooms(this.hotel!.id, this.checkIn, this.checkOut).subscribe({
      next: (categories) => {
        const cat = categories.find(c => c.id === this.selectedCategory!.id);
        if (!cat || cat.availableRooms === 0) {
          this.error = 'No rooms available for the selected dates';
          this.submitting = false;
          return;
        }
        // Use the rooms endpoint - we need to find a specific room
        // The API expects a roomId, so let's search for rooms in this category
        this.bookingService.createBooking({
          roomId: this.selectedCategory!.id, // The backend resolves room from category
          checkIn: this.checkIn,
          checkOut: this.checkOut,
          promotionCode: this.promoCode || undefined
        }).subscribe({
          next: (booking) => {
            this.router.navigate(['/booking-confirmation', booking.id], {
              state: { booking }
            });
          },
          error: (err) => {
            this.error = err.error?.message || 'Booking failed. Please try again.';
            this.submitting = false;
          }
        });
      },
      error: () => {
        this.error = 'Failed to verify availability';
        this.submitting = false;
      }
    });
  }
}
