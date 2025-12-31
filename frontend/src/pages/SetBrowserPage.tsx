import { useState } from 'react';
import {
  Box,
  Typography,
  Grid,
  Card,
  CardContent,
  CardActionArea,
  Skeleton,
  TextField,
  InputAdornment,
} from '@mui/material';
import SearchIcon from '@mui/icons-material/Search';
import { useNavigate } from 'react-router-dom';
import { useSets } from '../hooks';

export function SetBrowserPage() {
  const navigate = useNavigate();
  const { data: sets, isLoading } = useSets();
  const [search, setSearch] = useState('');

  const filteredSets = sets?.filter((set) =>
    set.toLowerCase().includes(search.toLowerCase())
  );

  const handleSetClick = (setCode: string) => {
    navigate(`/search?set=${setCode}`);
  };

  if (isLoading) {
    return (
      <Box>
        <Typography variant="h4" fontWeight={700} gutterBottom>
          Browse Sets
        </Typography>
        <Grid container spacing={2}>
          {Array.from({ length: 24 }).map((_, i) => (
            <Grid key={i} size={{ xs: 6, sm: 4, md: 3, lg: 2 }}>
              <Skeleton variant="rectangular" height={80} sx={{ borderRadius: 1 }} />
            </Grid>
          ))}
        </Grid>
      </Box>
    );
  }

  return (
    <Box>
      <Typography variant="h4" fontWeight={700} gutterBottom>
        Browse Sets
      </Typography>

      <TextField
        fullWidth
        value={search}
        onChange={(e) => setSearch(e.target.value)}
        placeholder="Filter sets..."
        slotProps={{
          input: {
            startAdornment: (
              <InputAdornment position="start">
                <SearchIcon color="action" />
              </InputAdornment>
            ),
          },
        }}
        sx={{ mb: 3 }}
      />

      <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
        {filteredSets?.length || 0} sets available
      </Typography>

      <Grid container spacing={2}>
        {filteredSets?.map((set) => (
          <Grid key={set} size={{ xs: 6, sm: 4, md: 3, lg: 2 }}>
            <Card>
              <CardActionArea onClick={() => handleSetClick(set)}>
                <CardContent sx={{ textAlign: 'center', py: 3 }}>
                  <Typography variant="h6" fontWeight={600}>
                    {set.toUpperCase()}
                  </Typography>
                </CardContent>
              </CardActionArea>
            </Card>
          </Grid>
        ))}
      </Grid>

      {filteredSets?.length === 0 && (
        <Typography variant="body1" color="text.secondary" textAlign="center" sx={{ py: 4 }}>
          No sets found matching "{search}"
        </Typography>
      )}
    </Box>
  );
}
