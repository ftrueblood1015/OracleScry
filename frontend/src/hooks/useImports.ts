import { useQuery } from '@tanstack/react-query';
import {
  getImportHistory,
  getImportById,
  getLatestImport,
  getImportStats,
  getImportStatus,
} from '../api/import';

/**
 * Hook for paginated import history
 */
export const useImportHistory = (page = 1, pageSize = 20, enabled = true) => {
  return useQuery({
    queryKey: ['imports', 'history', page, pageSize],
    queryFn: () => getImportHistory(page, pageSize),
    enabled,
    staleTime: 60 * 1000, // 1 minute - imports don't change often
  });
};

/**
 * Hook for single import details with errors
 */
export const useImport = (id: string, enabled = true) => {
  return useQuery({
    queryKey: ['imports', 'detail', id],
    queryFn: () => getImportById(id),
    enabled: enabled && !!id,
    staleTime: 60 * 1000,
  });
};

/**
 * Hook for latest import
 */
export const useLatestImport = (enabled = true) => {
  return useQuery({
    queryKey: ['imports', 'latest'],
    queryFn: getLatestImport,
    enabled,
    staleTime: 60 * 1000,
  });
};

/**
 * Hook for aggregated import statistics
 */
export const useImportStats = (enabled = true) => {
  return useQuery({
    queryKey: ['imports', 'stats'],
    queryFn: getImportStats,
    enabled,
    staleTime: 60 * 1000,
  });
};

/**
 * Hook for checking if import is running
 */
export const useImportStatus = (enabled = true) => {
  return useQuery({
    queryKey: ['imports', 'status'],
    queryFn: getImportStatus,
    enabled,
    refetchInterval: 30 * 1000, // Poll every 30 seconds
    staleTime: 10 * 1000,
  });
};
