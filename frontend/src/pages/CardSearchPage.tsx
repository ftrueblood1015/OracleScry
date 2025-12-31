import { useState, useCallback, useEffect } from 'react';
import { Box, Typography } from '@mui/material';
import { useNavigate, useSearchParams } from 'react-router-dom';
import { CardSearchBar, CardFilters, CardGrid, Pagination } from '../components';
import { useCardSearch, useSets } from '../hooks';
import type { CardFilterDto, CardSummaryDto } from '../types';

const defaultFilters: CardFilterDto = {
  page: 1,
  pageSize: 24,
  sortBy: 'name',
  sortDescending: false,
};

export function CardSearchPage() {
  const navigate = useNavigate();
  const [searchParams, setSearchParams] = useSearchParams();

  // Parse filters from URL
  const [filters, setFilters] = useState<CardFilterDto>(() => ({
    ...defaultFilters,
    search: searchParams.get('search') || undefined,
    set: searchParams.get('set') || undefined,
    rarity: searchParams.get('rarity') || undefined,
    format: searchParams.get('format') || undefined,
    colors: searchParams.getAll('colors') || undefined,
    page: parseInt(searchParams.get('page') || '1'),
    pageSize: parseInt(searchParams.get('pageSize') || '24'),
  }));

  const { data: sets } = useSets();
  const { data, isLoading } = useCardSearch(filters);

  // Update URL when filters change
  useEffect(() => {
    const params = new URLSearchParams();
    if (filters.search) params.set('search', filters.search);
    if (filters.set) params.set('set', filters.set);
    if (filters.rarity) params.set('rarity', filters.rarity);
    if (filters.format) params.set('format', filters.format);
    if (filters.colors?.length) filters.colors.forEach((c) => params.append('colors', c));
    if (filters.page && filters.page > 1) params.set('page', filters.page.toString());
    if (filters.pageSize && filters.pageSize !== 24) params.set('pageSize', filters.pageSize.toString());
    setSearchParams(params, { replace: true });
  }, [filters, setSearchParams]);

  const handleSearchChange = useCallback((search: string) => {
    setFilters((prev) => ({ ...prev, search: search || undefined, page: 1 }));
  }, []);

  const handleFilterChange = useCallback((updates: Partial<CardFilterDto>) => {
    setFilters((prev) => ({ ...prev, ...updates, page: 1 }));
  }, []);

  const handleClearFilters = useCallback(() => {
    setFilters({ ...defaultFilters, search: filters.search });
  }, [filters.search]);

  const handlePageChange = useCallback((page: number) => {
    setFilters((prev) => ({ ...prev, page }));
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }, []);

  const handlePageSizeChange = useCallback((pageSize: number) => {
    setFilters((prev) => ({ ...prev, pageSize, page: 1 }));
  }, []);

  const handleCardClick = useCallback(
    (card: CardSummaryDto) => {
      navigate(`/cards/${card.id}`);
    },
    [navigate]
  );

  return (
    <Box>
      <Typography variant="h4" fontWeight={700} gutterBottom>
        Search Cards
      </Typography>

      <Box sx={{ mb: 3 }}>
        <CardSearchBar
          value={filters.search || ''}
          onChange={handleSearchChange}
          autoFocus
        />
      </Box>

      <CardFilters
        filters={filters}
        onFilterChange={handleFilterChange}
        sets={sets}
        onClear={handleClearFilters}
      />

      <CardGrid
        cards={data?.items || []}
        isLoading={isLoading}
        onCardClick={handleCardClick}
        emptyMessage={
          filters.search
            ? `No cards found for "${filters.search}"`
            : 'No cards found with the selected filters'
        }
      />

      {data && data.totalPages > 1 && (
        <Pagination
          page={data.page}
          totalPages={data.totalPages}
          totalCount={data.totalCount}
          pageSize={filters.pageSize || 24}
          onPageChange={handlePageChange}
          onPageSizeChange={handlePageSizeChange}
        />
      )}
    </Box>
  );
}
