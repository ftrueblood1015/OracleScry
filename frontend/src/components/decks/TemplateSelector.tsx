import { useState, useMemo } from 'react';
import {
  Box,
  Grid,
  TextField,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Typography,
  Skeleton,
  Alert,
} from '@mui/material';
import { useDeckTemplates } from '../../hooks/useDeckTemplates';
import { TemplateCard } from './TemplateCard';
import { DECK_FORMAT_OPTIONS } from '../../types/deck';
import type { DeckTemplateSummaryDto } from '../../types/deckTemplate';

interface TemplateSelectorProps {
  onSelect: (template: DeckTemplateSummaryDto) => void;
  selectedId?: string;
}

export function TemplateSelector({ onSelect, selectedId }: TemplateSelectorProps) {
  const [format, setFormat] = useState<string>('');
  const [searchInput, setSearchInput] = useState('');
  const [debouncedSearch, setDebouncedSearch] = useState('');

  // Debounce search input
  useMemo(() => {
    const timer = setTimeout(() => {
      setDebouncedSearch(searchInput);
    }, 300);
    return () => clearTimeout(timer);
  }, [searchInput]);

  const { data: templates, isLoading, error } = useDeckTemplates(
    format || undefined,
    debouncedSearch || undefined
  );

  if (error) {
    return (
      <Alert severity="error" sx={{ mt: 2 }}>
        Failed to load templates. Please try again.
      </Alert>
    );
  }

  return (
    <Box>
      {/* Filters */}
      <Box sx={{ display: 'flex', gap: 2, mb: 3 }}>
        <TextField
          label="Search templates"
          variant="outlined"
          size="small"
          value={searchInput}
          onChange={(e) => setSearchInput(e.target.value)}
          sx={{ flex: 1 }}
          placeholder="Search by name, description, or set..."
        />
        <FormControl size="small" sx={{ minWidth: 150 }}>
          <InputLabel>Format</InputLabel>
          <Select
            value={format}
            label="Format"
            onChange={(e) => setFormat(e.target.value)}
          >
            <MenuItem value="">All Formats</MenuItem>
            {DECK_FORMAT_OPTIONS.map((option) => (
              <MenuItem key={option.value} value={option.value}>
                {option.label}
              </MenuItem>
            ))}
          </Select>
        </FormControl>
      </Box>

      {/* Loading state */}
      {isLoading && (
        <Grid container spacing={2}>
          {[1, 2, 3, 4, 5, 6].map((i) => (
            <Grid item xs={12} sm={6} md={4} key={i}>
              <Skeleton variant="rectangular" height={280} sx={{ borderRadius: 1 }} />
            </Grid>
          ))}
        </Grid>
      )}

      {/* Empty state */}
      {!isLoading && templates?.length === 0 && (
        <Box sx={{ textAlign: 'center', py: 4 }}>
          <Typography color="text.secondary">
            {debouncedSearch || format
              ? 'No templates found matching your criteria.'
              : 'No deck templates available yet.'}
          </Typography>
        </Box>
      )}

      {/* Template grid */}
      {!isLoading && templates && templates.length > 0 && (
        <Grid container spacing={2}>
          {templates.map((template) => (
            <Grid item xs={12} sm={6} md={4} key={template.id}>
              <TemplateCard
                template={template}
                onClick={() => onSelect(template)}
                selected={template.id === selectedId}
              />
            </Grid>
          ))}
        </Grid>
      )}
    </Box>
  );
}
