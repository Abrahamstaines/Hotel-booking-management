export interface LoginDto {
  email: string;
  password: string;
}

export interface RegisterDto {
  email: string;
  password: string;
  fullName: string;
  phone: string;
}

export interface AuthResponse {
  token: string;
  email: string;
  fullName: string;
  expiration: string;
}
