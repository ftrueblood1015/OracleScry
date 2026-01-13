import {
  Box,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Chip,
  OutlinedInput,
  Typography,
  Slider,
  Button,
  Collapse,
} from '@mui/material';
import type { SelectChangeEvent } from '@mui/material';
import FilterListIcon from '@mui/icons-material/FilterList';
import { useState } from 'react';
import type { CardFilterDto, MtgColor, CardPurposeSummaryDto } from '../../types';

interface CardFiltersProps {
  filters: CardFilterDto;
  onFilterChange: (filters: Partial<CardFilterDto>) => void;
  sets?: string[];
  purposes?: CardPurposeSummaryDto[];
  onClear?: () => void;
}

const colorOptions: { value: MtgColor; label: string; color: string }[] = [
  { value: 'W', label: 'White', color: '#f9fafb' },
  { value: 'U', label: 'Blue', color: '#3b82f6' },
  { value: 'B', label: 'Black', color: '#1f2937' },
  { value: 'R', label: 'Red', color: '#ef4444' },
  { value: 'G', label: 'Green', color: '#22c55e' },
];

const rarityOptions = [
  { value: 'common', label: 'Common' },
  { value: 'uncommon', label: 'Uncommon' },
  { value: 'rare', label: 'Rare' },
  { value: 'mythic', label: 'Mythic' },
];

const formatOptions = [
  { value: 'standard', label: 'Standard' },
  { value: 'pioneer', label: 'Pioneer' },
  { value: 'modern', label: 'Modern' },
  { value: 'legacy', label: 'Legacy' },
  { value: 'vintage', label: 'Vintage' },
  { value: 'commander', label: 'Commander' },
  { value: 'pauper', label: 'Pauper' },
  { value: 'historic', label: 'Historic' },
];

const sortOptions = [
  { value: 'name', label: 'Name' },
  { value: 'cmc', label: 'Mana Value' },
  { value: 'rarity', label: 'Rarity' },
  { value: 'releasedAt', label: 'Release Date' },
  { value: 'usd', label: 'Price (USD)' },
];

export function CardFilters({ filters, onFilterChange, sets = [], purposes = [], onClear }: CardFiltersProps) {
  const [expanded, setExpanded] = useState(false);

  const handleColorChange = (event: SelectChangeEvent<string[]>) => {
    const value = event.target.value;
    onFilterChange({ colors: typeof value === 'string' ? value.split(',') : value });
  };

  const handlePurposeChange = (event: SelectChangeEvent<string[]>) => {
    const value = event.target.value;
    onFilterChange({ purposes: typeof value === 'string' ? value.split(',') : value });
  };

  const handleCmcChange = (_: Event, value: number | number[]) => {
    if (Array.isArray(value)) {
      onFilterChange({ minCmc: value[0], maxCmc: value[1] });
    }
  };

  const hasActiveFilters =
    filters.colors?.length ||
    filters.rarity ||
    filters.set ||
    filters.format ||
    filters.purposes?.length ||
    filters.minCmc ||
    filters.maxCmc;

  return (
    <Box>
      <Box sx={{ display: 'flex', alignItems: 'center', gap: 2, mb: 2 }}>
        <Button
          variant={expanded ? 'contained' : 'outlined'}
          startIcon={<FilterListIcon />}
          onClick={() => setExpanded(!expanded)}
          size="small"
        >
          Filters {hasActiveFilters && `(${Object.keys(filters).filter((k) => filters[k as keyof CardFilterDto]).length})`}
        </Button>
        {hasActiveFilters && (
          <Button variant="text" size="small" onClick={onClear}>
            Clear All
          </Button>
        )}
      </Box>

      <Collapse in={expanded}>
        <Box
          sx={{
            display: 'grid',
            gridTemplateColumns: { xs: '1fr', sm: '1fr 1fr', md: '1fr 1fr 1fr 1fr' },
            gap: 2,
            p: 2,
            bgcolor: 'background.paper',
            borderRadius: 1,
            mb: 2,
          }}
        >
          {/* Colors */}
          <FormControl size="small" fullWidth>
            <InputLabel>Colors</InputLabel>
            <Select
              multiple
              value={filters.colors || []}
              onChange={handleColorChange}
              input={<OutlinedInput label="Colors" />}
              renderValue={(selected) => (
                <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 0.5 }}>
                  {selected.map((value) => {
                    const color = colorOptions.find((c) => c.value === value);
                    return (
                      <Chip
                        key={value}
                        label={value}
                        size="small"
                        sx={{ bgcolor: color?.color, color: value === 'B' ? 'white' : 'black' }}
                      />
                    );
                  })}
                </Box>
              )}
            >
              {colorOptions.map((color) => (
                <MenuItem key={color.value} value={color.value}>
                  <Chip
                    label={color.label}
                    size="small"
                    sx={{
                      bgcolor: color.color,
                      color: color.value === 'B' ? 'white' : 'black',
                      mr: 1,
                    }}
                  />
                </MenuItem>
              ))}
            </Select>
          </FormControl>

          {/* Rarity */}
          <FormControl size="small" fullWidth>
            <InputLabel>Rarity</InputLabel>
            <Select
              value={filters.rarity || ''}
              onChange={(e) => onFilterChange({ rarity: e.target.value || undefined })}
              label="Rarity"
            >
              <MenuItem value="">Any</MenuItem>
              {rarityOptions.map((r) => (
                <MenuItem key={r.value} value={r.value}>
                  {r.label}
                </MenuItem>
              ))}
            </Select>
          </FormControl>

          {/* Set */}
          <FormControl size="small" fullWidth>
            <InputLabel>Set</InputLabel>
            <Select
              value={filters.set || ''}
              onChange={(e) => onFilterChange({ set: e.target.value || undefined })}
              label="Set"
            >
              <MenuItem value="">Any</MenuItem>
              {sets.map((set) => (
                <MenuItem key={set} value={set}>
                  {set.toUpperCase()}
                </MenuItem>
              ))}
            </Select>
          </FormControl>

          {/* Format */}
          <FormControl size="small" fullWidth>
            <InputLabel>Format</InputLabel>
            <Select
              value={filters.format || ''}
              onChange={(e) => onFilterChange({ format: e.target.value || undefined })}
              label="Format"
            >
              <MenuItem value="">Any</MenuItem>
              {formatOptions.map((f) => (
                <MenuItem key={f.value} value={f.value}>
                  {f.label}
                </MenuItem>
              ))}
            </Select>
          </FormControl>

          {/* Purpose */}
          <FormControl size="small" fullWidth>
            <InputLabel>Purpose</InputLabel>
            <Select
              multiple
              value={filters.purposes || []}
              onChange={handlePurposeChange}
              input={<OutlinedInput label="Purpose" />}
              renderValue={(selected) => (
                <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 0.5 }}>
                  {selected.map((id) => {
                    const purpose = purposes.find((p) => p.id === id);
                    return (
                      <Chip
                        key={id}
                        label={purpose?.name || id}
                        size="small"
                      />
                    );
                  })}
                </Box>
              )}
            >
              {purposes.map((purpose) => (
                <MenuItem key={purpose.id} value={purpose.id}>
                  {purpose.name}
                </MenuItem>
              ))}
            </Select>
          </FormControl>

          {/* CMC Range */}
          <Box sx={{ gridColumn: { sm: 'span 2' }, px: 1 }}>
            <Typography variant="body2" color="text.secondary" gutterBottom>
              Mana Value: {filters.minCmc || 0} - {filters.maxCmc || 16}+
            </Typography>
            <Slider
              value={[filters.minCmc || 0, filters.maxCmc || 16]}
              onChange={handleCmcChange}
              min={0}
              max={16}
              marks={[
                { value: 0, label: '0' },
                { value: 4, label: '4' },
                { value: 8, label: '8' },
                { value: 12, label: '12' },
                { value: 16, label: '16+' },
              ]}
              valueLabelDisplay="auto"
            />
          </Box>

          {/* Sort */}
          <FormControl size="small" fullWidth>
            <InputLabel>Sort By</InputLabel>
            <Select
              value={filters.sortBy || 'name'}
              onChange={(e) => onFilterChange({ sortBy: e.target.value })}
              label="Sort By"
            >
              {sortOptions.map((s) => (
                <MenuItem key={s.value} value={s.value}>
                  {s.label}
                </MenuItem>
              ))}
            </Select>
          </FormControl>

          <FormControl size="small" fullWidth>
            <InputLabel>Order</InputLabel>
            <Select
              value={filters.sortDescending ? 'desc' : 'asc'}
              onChange={(e) => onFilterChange({ sortDescending: e.target.value === 'desc' })}
              label="Order"
            >
              <MenuItem value="asc">Ascending</MenuItem>
              <MenuItem value="desc">Descending</MenuItem>
            </Select>
          </FormControl>
        </Box>
      </Collapse>
    </Box>
  );
}
