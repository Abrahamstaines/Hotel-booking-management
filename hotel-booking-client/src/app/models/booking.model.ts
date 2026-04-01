export interface CreateBooking {
  roomId: number;
  checkIn: string;
  checkOut: string;
  promotionCode?: string;
}

export interface BookingResponse {
  id: number;
  bookingReference: string;
  hotelName: string;
  roomCategory: string;
  roomNumber: string;
  checkIn: string;
  checkOut: string;
  totalPrice: number;
  discountAmount: number;
  promotionCode?: string;
  status: string;
  createdAt: string;
}

export interface PromotionValidate {
  code: string;
  originalPrice: number;
}

export interface PromotionResult {
  isValid: boolean;
  message: string;
  discountPercent: number;
  discountAmount: number;
  finalPrice: number;
}
