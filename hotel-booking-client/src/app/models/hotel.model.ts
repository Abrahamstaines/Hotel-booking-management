export interface HotelList {
  id: number;
  name: string;
  city: string;
  starRating: number;
  imageUrl: string;
  startingPrice: number;
  amenities: string[];
}

export interface HotelDetail {
  id: number;
  name: string;
  description: string;
  city: string;
  address: string;
  starRating: number;
  imageUrl: string;
  contactEmail: string;
  contactPhone: string;
  amenities: Amenity[];
  roomCategories: RoomCategory[];
}

export interface Amenity {
  name: string;
  icon: string;
}

export interface RoomCategory {
  id: number;
  name: string;
  description: string;
  maxOccupancy: number;
  basePrice: number;
  imageUrl: string;
  availableRooms: number;
}

export interface Room {
  id: number;
  roomNumber: string;
  floor: number;
  isAvailable: boolean;
  categoryName: string;
  price: number;
  hotelName: string;
}

export interface HotelSearchParams {
  city?: string;
  checkIn?: string;
  checkOut?: string;
  minPrice?: number;
  maxPrice?: number;
  starRating?: number;
  guests?: number;
  amenity?: string;
}
