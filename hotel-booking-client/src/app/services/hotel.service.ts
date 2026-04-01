import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HotelDetail, HotelList, HotelSearchParams, Room, RoomCategory } from '../models/hotel.model';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class HotelService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  searchHotels(params: HotelSearchParams): Observable<HotelList[]> {
    let httpParams = new HttpParams();
    Object.entries(params).forEach(([key, value]) => {
      if (value !== undefined && value !== null && value !== '') {
        httpParams = httpParams.set(key, value.toString());
      }
    });
    return this.http.get<HotelList[]>(`${this.apiUrl}/hotels`, { params: httpParams });
  }

  getHotelDetail(id: number): Observable<HotelDetail> {
    return this.http.get<HotelDetail>(`${this.apiUrl}/hotels/${id}`);
  }

  getAvailableRooms(hotelId: number, checkIn?: string, checkOut?: string): Observable<RoomCategory[]> {
    let params = new HttpParams();
    if (checkIn) params = params.set('checkIn', checkIn);
    if (checkOut) params = params.set('checkOut', checkOut);
    return this.http.get<RoomCategory[]>(`${this.apiUrl}/hotels/${hotelId}/rooms`, { params });
  }

  getRoom(roomId: number): Observable<Room> {
    return this.http.get<Room>(`${this.apiUrl}/rooms/${roomId}`);
  }
}
