import { useQuery, useInfiniteQuery } from '@tanstack/react-query';
import {
  searchCards,
  getCardById,
  getCardByScryfallId,
  getSets,
  getCardsBySet,
  getRandomCards,
  getAllPurposes,
} from '../api';
import type { CardFilterDto } from '../types';

export const useCardSearch = (filter: CardFilterDto, enabled = true) => {
  return useQuery({
    queryKey: ['cards', 'search', filter],
    queryFn: () => searchCards(filter),
    enabled,
    staleTime: 5 * 60 * 1000, // 5 minutes
  });
};

export const useInfiniteCardSearch = (filter: Omit<CardFilterDto, 'page'>) => {
  return useInfiniteQuery({
    queryKey: ['cards', 'infinite', filter],
    queryFn: ({ pageParam = 1 }) => searchCards({ ...filter, page: pageParam }),
    getNextPageParam: (lastPage) => (lastPage.hasNextPage ? lastPage.page + 1 : undefined),
    initialPageParam: 1,
    staleTime: 5 * 60 * 1000,
  });
};

export const useCard = (id: string, enabled = true) => {
  return useQuery({
    queryKey: ['cards', 'detail', id],
    queryFn: () => getCardById(id),
    enabled: enabled && !!id,
    staleTime: 10 * 60 * 1000, // 10 minutes
  });
};

export const useCardByScryfallId = (scryfallId: string, enabled = true) => {
  return useQuery({
    queryKey: ['cards', 'scryfall', scryfallId],
    queryFn: () => getCardByScryfallId(scryfallId),
    enabled: enabled && !!scryfallId,
    staleTime: 10 * 60 * 1000,
  });
};

export const useSets = () => {
  return useQuery({
    queryKey: ['sets'],
    queryFn: getSets,
    staleTime: 30 * 60 * 1000, // 30 minutes - sets don't change often
  });
};

export const useSetCards = (setCode: string, enabled = true) => {
  return useQuery({
    queryKey: ['cards', 'set', setCode],
    queryFn: () => getCardsBySet(setCode),
    enabled: enabled && !!setCode,
    staleTime: 10 * 60 * 1000,
  });
};

export const useRandomCards = (count = 10) => {
  return useQuery({
    queryKey: ['cards', 'random', count],
    queryFn: () => getRandomCards(count),
    staleTime: 0, // Always fetch fresh random cards
    refetchOnWindowFocus: false,
  });
};

export const usePurposes = () => {
  return useQuery({
    queryKey: ['purposes'],
    queryFn: getAllPurposes,
    staleTime: 30 * 60 * 1000, // 30 minutes - purposes don't change often
  });
};
