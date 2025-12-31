import { Box, Typography, Button, Grid, Paper } from '@mui/material';
import { Link as RouterLink, useNavigate } from 'react-router-dom';
import SearchIcon from '@mui/icons-material/Search';
import CollectionsIcon from '@mui/icons-material/Collections';
import { CardGrid } from '../components';
import { useRandomCards } from '../hooks';
import type { CardSummaryDto } from '../types';

export function HomePage() {
  const navigate = useNavigate();
  const { data: randomCards, isLoading } = useRandomCards(6);

  const handleCardClick = (card: CardSummaryDto) => {
    navigate(`/cards/${card.id}`);
  };

  return (
    <Box>
      {/* Hero Section */}
      <Box
        sx={{
          textAlign: 'center',
          py: { xs: 4, md: 8 },
          mb: 4,
        }}
      >
        <Typography variant="h2" fontWeight={700} gutterBottom>
          OracleScry
        </Typography>
        <Typography variant="h5" color="text.secondary" sx={{ mb: 4, maxWidth: 600, mx: 'auto' }}>
          Your complete Magic: The Gathering card database
        </Typography>
        <Box sx={{ display: 'flex', gap: 2, justifyContent: 'center', flexWrap: 'wrap' }}>
          <Button
            component={RouterLink}
            to="/search"
            variant="contained"
            size="large"
            startIcon={<SearchIcon />}
          >
            Search Cards
          </Button>
          <Button
            component={RouterLink}
            to="/sets"
            variant="outlined"
            size="large"
            startIcon={<CollectionsIcon />}
          >
            Browse Sets
          </Button>
        </Box>
      </Box>

      {/* Feature Cards */}
      <Grid container spacing={3} sx={{ mb: 6 }}>
        <Grid size={{ xs: 12, md: 4 }}>
          <Paper sx={{ p: 3, height: '100%', textAlign: 'center' }}>
            <SearchIcon sx={{ fontSize: 48, color: 'primary.main', mb: 2 }} />
            <Typography variant="h6" gutterBottom>
              Powerful Search
            </Typography>
            <Typography variant="body2" color="text.secondary">
              Filter by colors, mana cost, format legality, rarity, and more
            </Typography>
          </Paper>
        </Grid>
        <Grid size={{ xs: 12, md: 4 }}>
          <Paper sx={{ p: 3, height: '100%', textAlign: 'center' }}>
            <CollectionsIcon sx={{ fontSize: 48, color: 'primary.main', mb: 2 }} />
            <Typography variant="h6" gutterBottom>
              Complete Database
            </Typography>
            <Typography variant="body2" color="text.secondary">
              Access every Magic card with full details and images
            </Typography>
          </Paper>
        </Grid>
        <Grid size={{ xs: 12, md: 4 }}>
          <Paper sx={{ p: 3, height: '100%', textAlign: 'center' }}>
            <Typography sx={{ fontSize: 48, mb: 2 }}>$</Typography>
            <Typography variant="h6" gutterBottom>
              Price Tracking
            </Typography>
            <Typography variant="body2" color="text.secondary">
              View current market prices from multiple sources
            </Typography>
          </Paper>
        </Grid>
      </Grid>

      {/* Random Cards Section */}
      <Box sx={{ mb: 4 }}>
        <Typography variant="h5" fontWeight={600} gutterBottom>
          Discover Cards
        </Typography>
        <CardGrid
          cards={randomCards || []}
          isLoading={isLoading}
          onCardClick={handleCardClick}
        />
      </Box>
    </Box>
  );
}
