/**
 * User registration request
 */
export interface RegisterRequest {
  email: string;
  password: string;
  confirmPassword: string;
  displayName?: string;
}

/**
 * User login request
 */
export interface LoginRequest {
  email: string;
  password: string;
}

/**
 * Refresh token request
 */
export interface RefreshTokenRequest {
  refreshToken: string;
}

/**
 * Authentication response (login/register/refresh)
 */
export interface AuthResponse {
  success: boolean;
  accessToken?: string;
  refreshToken?: string;
  expiresAt?: string;
  errors: string[];
}

/**
 * Current user information
 */
export interface UserDto {
  id: string;
  email: string;
  displayName: string;
}
