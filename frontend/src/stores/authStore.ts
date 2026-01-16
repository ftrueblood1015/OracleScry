import { create } from 'zustand';
import { persist } from 'zustand/middleware';
import type { UserDto } from '../types';
import { getCurrentUser, logout as apiLogout, getAccessToken } from '../api';

interface AuthState {
  user: UserDto | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  setUser: (user: UserDto | null) => void;
  setLoading: (loading: boolean) => void;
  logout: () => Promise<void>;
  checkAuth: () => Promise<void>;
  isAdmin: () => boolean;
  hasRole: (role: string) => boolean;
}

export const useAuthStore = create<AuthState>()(
  persist(
    (set, get) => ({
      user: null,
      isAuthenticated: false,
      isLoading: true,

      setUser: (user) =>
        set({
          user,
          isAuthenticated: !!user,
        }),

      setLoading: (loading) => set({ isLoading: loading }),

      logout: async () => {
        await apiLogout();
        set({ user: null, isAuthenticated: false });
      },

      checkAuth: async () => {
        set({ isLoading: true });
        const token = getAccessToken();
        if (!token) {
          set({ user: null, isAuthenticated: false, isLoading: false });
          return;
        }
        const user = await getCurrentUser();
        set({
          user,
          isAuthenticated: !!user,
          isLoading: false,
        });
      },

      isAdmin: () => get().user?.roles?.includes('Admin') ?? false,

      hasRole: (role: string) => get().user?.roles?.includes(role) ?? false,
    }),
    {
      name: 'oraclescry-auth',
      partialize: (state) => ({ user: state.user, isAuthenticated: state.isAuthenticated }),
    }
  )
);
