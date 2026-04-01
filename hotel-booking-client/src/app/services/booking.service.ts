import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BookingResponse, CreateBooking, PromotionResult, PromotionValidate } from '../models/booking.model';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class BookingService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  createBooking(dto: CreateBooking): Observable<BookingResponse> {
    return this.http.post<BookingResponse>(`${this.apiUrl}/bookings`, dto);
  }

  getUserBookings(): Observable<BookingResponse[]> {
    return this.http.get<BookingResponse[]>(`${this.apiUrl}/bookings`);
  }

  cancelBooking(id: number): Observable<BookingResponse> {
    return this.http.put<BookingResponse>(`${this.apiUrl}/bookings/${id}/cancel`, {});
  }

  rebookBooking(id: number, checkIn: string, checkOut: string): Observable<BookingResponse> {
    return this.http.post<BookingResponse>(
      `${this.apiUrl}/bookings/${id}/rebook?checkIn=${checkIn}&checkOut=${checkOut}`, {}
    );
  }

  validatePromotion(dto: PromotionValidate): Observable<PromotionResult> {
    return this.http.post<PromotionResult>(`${this.apiUrl}/promotions/validate`, dto);
  }
}
