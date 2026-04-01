import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HotelService } from '../../services/hotel.service';
import { HotelList, HotelSearchParams } from '../../models/hotel.model';

@Component({
  selector: 'app-hotel-list',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './hotel-list.html',
  styleUrl: './hotel-list.scss'
})
export class HotelListComponent implements OnInit {
  hotels: HotelList[] = [];
  loading = false;
  searchParams: HotelSearchParams = {};

  constructor(
    private hotelService: HotelService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.searchParams = {
        city: params['city'] || '',
        checkIn: params['checkIn'] || '',
        checkOut: params['checkOut'] || '',
        minPrice: params['minPrice'] ? +params['minPrice'] : undefined,
        maxPrice: params['maxPrice'] ? +params['maxPrice'] : undefined,
        starRating: params['starRating'] ? +params['starRating'] : undefined,
        guests: params['guests'] ? +params['guests'] : undefined,
        amenity: params['amenity'] || ''
      };
      this.searchHotels();
    });
  }

  searchHotels(): void {
    this.loading = true;
    console.log('Searching hotels with params:', this.searchParams);
    this.hotelService.searchHotels(this.searchParams).subscribe({
      next: (data) => {
        console.log('Hotels received:', data);
        this.hotels = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Hotel search error:', err);
        this.loading = false;
      }
    });
  }

  applyFilters(): void {
    const params: any = {};
    Object.entries(this.searchParams).forEach(([key, value]) => {
      if (value !== undefined && value !== null && value !== '') {
        params[key] = value;
      }
    });
    this.router.navigate(['/hotels'], { queryParams: params });
  }

  getStars(rating: number): number[] {
    return Array(rating).fill(0);
  }
}
