import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HotelService } from '../../services/hotel.service';
import { HotelDetail, RoomCategory } from '../../models/hotel.model';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-hotel-detail',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './hotel-detail.html',
  styleUrl: './hotel-detail.scss'
})
export class HotelDetailComponent implements OnInit {
  hotel: HotelDetail | null = null;
  rooms: RoomCategory[] = [];
  loading = true;
  checkIn = '';
  checkOut = '';

  constructor(
    private hotelService: HotelService,
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    const id = +this.route.snapshot.params['id'];
    this.route.queryParams.subscribe(params => {
      this.checkIn = params['checkIn'] || '';
      this.checkOut = params['checkOut'] || '';
    });

    this.hotelService.getHotelDetail(id).subscribe({
      next: (hotel) => {
        this.hotel = hotel;
        this.rooms = hotel.roomCategories;
        this.loading = false;
        if (this.checkIn && this.checkOut) {
          this.checkAvailability();
        }
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  checkAvailability(): void {
    if (!this.hotel) return;
    this.hotelService.getAvailableRooms(this.hotel.id, this.checkIn, this.checkOut).subscribe({
      next: (rooms) => {
        this.rooms = rooms;
      }
    });
  }

  bookRoom(roomCategoryId: number): void {
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }
    this.router.navigate(['/checkout'], {
      queryParams: {
        hotelId: this.hotel?.id,
        categoryId: roomCategoryId,
        checkIn: this.checkIn,
        checkOut: this.checkOut
      }
    });
  }

  getStars(rating: number): number[] {
    return Array(rating).fill(0);
  }
}
