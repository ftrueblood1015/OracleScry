import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { login, register, getCurrentUser } from '../api';
import { useAuthStore } from '../stores';
import type { LoginRequest, RegisterRequest } from '../types';

export const useCurrentUser = () => {
  const { setUser, setLoading } = useAuthStore();

  return useQuery({
    queryKey: ['currentUser'],
    queryFn: async () => {
      const user = await getCurrentUser();
      setUser(user);
      setLoading(false);
      return user;
    },
    retry: false,
    staleTime: 5 * 60 * 1000, // 5 minutes
  });
};

export const useLogin = () => {
  const queryClient = useQueryClient();
  const { setUser } = useAuthStore();

  return useMutation({
    mutationFn: (request: LoginRequest) => login(request),
    onSuccess: async (data) => {
      if (data.success) {
        const user = await getCurrentUser();
        setUser(user);
        queryClient.invalidateQueries({ queryKey: ['currentUser'] });
      }
    },
  });
};

export const useRegister = () => {
  const queryClient = useQueryClient();
  const { setUser } = useAuthStore();

  return useMutation({
    mutationFn: (request: RegisterRequest) => register(request),
    onSuccess: async (data) => {
      if (data.success) {
        const user = await getCurrentUser();
        setUser(user);
        queryClient.invalidateQueries({ queryKey: ['currentUser'] });
      }
    },
  });
};

export const useLogout = () => {
  const queryClient = useQueryClient();
  const { logout } = useAuthStore();

  return useMutation({
    mutationFn: logout,
    onSuccess: () => {
      queryClient.clear();
    },
  });
};
