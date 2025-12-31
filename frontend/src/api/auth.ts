import client, { setTokens, clearTokens, getRefreshToken } from './client';
import type {
  AuthResponse,
  LoginRequest,
  RegisterRequest,
  RefreshTokenRequest,
  UserDto,
} from '../types';

/**
 * Register a new user
 */
export const register = async (request: RegisterRequest): Promise<AuthResponse> => {
  const response = await client.post<AuthResponse>('/auth/register', request);
  if (response.data.success && response.data.accessToken && response.data.refreshToken) {
    setTokens(response.data.accessToken, response.data.refreshToken);
  }
  return response.data;
};

/**
 * Login with email and password
 */
export const login = async (request: LoginRequest): Promise<AuthResponse> => {
  const response = await client.post<AuthResponse>('/auth/login', request);
  if (response.data.success && response.data.accessToken && response.data.refreshToken) {
    setTokens(response.data.accessToken, response.data.refreshToken);
  }
  return response.data;
};

/**
 * Refresh the access token
 */
export const refreshToken = async (): Promise<AuthResponse> => {
  const token = getRefreshToken();
  if (!token) {
    return { success: false, errors: ['No refresh token available'] };
  }
  const request: RefreshTokenRequest = { refreshToken: token };
  const response = await client.post<AuthResponse>('/auth/refresh', request);
  if (response.data.success && response.data.accessToken && response.data.refreshToken) {
    setTokens(response.data.accessToken, response.data.refreshToken);
  }
  return response.data;
};

/**
 * Revoke the current refresh token (logout)
 */
export const revokeToken = async (): Promise<void> => {
  const token = getRefreshToken();
  if (token) {
    await client.post('/auth/revoke', { refreshToken: token });
  }
  clearTokens();
};

/**
 * Get current authenticated user
 */
export const getCurrentUser = async (): Promise<UserDto | null> => {
  try {
    const response = await client.get<UserDto>('/auth/me');
    return response.data;
  } catch {
    return null;
  }
};

/**
 * Logout - revoke token and clear storage
 */
export const logout = async (): Promise<void> => {
  try {
    await revokeToken();
  } catch {
    // Clear tokens even if revoke fails
    clearTokens();
  }
};
