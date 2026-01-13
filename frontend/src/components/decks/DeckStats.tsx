import { Box, Paper, Typography, Grid, Chip, LinearProgress, CircularProgress, Alert } from '@mui/material';
import type { DeckStatsDto } from '../../types';

interface DeckStatsProps {
  stats: DeckStatsDto | undefined;
  isLoading: boolean;
  error?: Error | null;
}

const COLOR_MAP: Record<string, string> = {
  W: '#f9fafb',
  U: '#3b82f6',
  B: '#1f2937',
  R: '#ef4444',
  G: '#22c55e',
  C: '#9ca3af',
};

const COLOR_LABELS: Record<string, string> = {
  W: 'White',
  U: 'Blue',
  B: 'Black',
  R: 'Red',
  G: 'Green',
  C: 'Colorless',
};

export function DeckStats({ stats, isLoading, error }: DeckStatsProps) {
  if (isLoading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', py: 4 }}>
        <CircularProgress />
      </Box>
    );
  }

  if (error) {
    return (
      <Alert severity="error" sx={{ my: 2 }}>
        Failed to load stats: {error.message}
      </Alert>
    );
  }

  if (!stats) {
    return null;
  }

  const maxManaCurve = Math.max(...Object.values(stats.manaCurve), 1);
  const maxPurpose = Math.max(...Object.values(stats.purposeBreakdown), 1);

  return (
    <Grid container spacing={2}>
      {/* Overview */}
      <Grid size={{ xs: 12 }}>
        <Paper sx={{ p: 2 }}>
          <Typography variant="h6" gutterBottom>
            Overview
          </Typography>
          <Grid container spacing={2}>
            <Grid size={{ xs: 6, sm: 3 }}>
              <Typography variant="body2" color="text.secondary">
                Total Cards
              </Typography>
              <Typography variant="h5">{stats.totalCards}</Typography>
            </Grid>
            <Grid size={{ xs: 6, sm: 3 }}>
              <Typography variant="body2" color="text.secondary">
                Mainboard
              </Typography>
              <Typography variant="h5">{stats.mainboardCount}</Typography>
            </Grid>
            <Grid size={{ xs: 6, sm: 3 }}>
              <Typography variant="body2" color="text.secondary">
                Sideboard
              </Typography>
              <Typography variant="h5">{stats.sideboardCount}</Typography>
            </Grid>
            <Grid size={{ xs: 6, sm: 3 }}>
              <Typography variant="body2" color="text.secondary">
                Avg. Mana Value
              </Typography>
              <Typography variant="h5">{stats.averageCmc.toFixed(2)}</Typography>
            </Grid>
          </Grid>
          {stats.estimatedPrice && (
            <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
              Estimated Price: ${stats.estimatedPrice.toFixed(2)}
            </Typography>
          )}
        </Paper>
      </Grid>

      {/* Mana Curve */}
      <Grid size={{ xs: 12, md: 6 }}>
        <Paper sx={{ p: 2 }}>
          <Typography variant="h6" gutterBottom>
            Mana Curve
          </Typography>
          <Box sx={{ display: 'flex', alignItems: 'flex-end', gap: 1, height: 120 }}>
            {[0, 1, 2, 3, 4, 5, 6, 7].map((cmc) => {
              const count = stats.manaCurve[cmc] || 0;
              const height = (count / maxManaCurve) * 100;
              return (
                <Box
                  key={cmc}
                  sx={{
                    flex: 1,
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                  }}
                >
                  <Typography variant="caption">{count}</Typography>
                  <Box
                    sx={{
                      width: '100%',
                      height: `${height}%`,
                      minHeight: count > 0 ? 4 : 0,
                      bgcolor: 'primary.main',
                      borderRadius: 1,
                    }}
                  />
                  <Typography variant="caption" sx={{ mt: 0.5 }}>
                    {cmc === 7 ? '7+' : cmc}
                  </Typography>
                </Box>
              );
            })}
          </Box>
        </Paper>
      </Grid>

      {/* Colors */}
      <Grid size={{ xs: 12, md: 6 }}>
        <Paper sx={{ p: 2 }}>
          <Typography variant="h6" gutterBottom>
            Colors
          </Typography>
          <Box sx={{ display: 'flex', gap: 1, flexWrap: 'wrap' }}>
            {Object.entries(stats.colorDistribution).map(([color, count]) => (
              <Chip
                key={color}
                label={`${COLOR_LABELS[color] || color}: ${count}`}
                sx={{
                  bgcolor: COLOR_MAP[color] || '#9ca3af',
                  color: color === 'B' ? 'white' : color === 'W' ? 'black' : 'white',
                }}
              />
            ))}
          </Box>
        </Paper>
      </Grid>

      {/* Card Types */}
      <Grid size={{ xs: 12, md: 6 }}>
        <Paper sx={{ p: 2 }}>
          <Typography variant="h6" gutterBottom>
            Card Types
          </Typography>
          <Box sx={{ display: 'flex', gap: 1, flexWrap: 'wrap' }}>
            {Object.entries(stats.typeDistribution)
              .sort((a, b) => b[1] - a[1])
              .map(([type, count]) => (
                <Chip key={type} label={`${type}: ${count}`} variant="outlined" />
              ))}
          </Box>
        </Paper>
      </Grid>

      {/* Purpose Breakdown */}
      <Grid size={{ xs: 12, md: 6 }}>
        <Paper sx={{ p: 2 }}>
          <Typography variant="h6" gutterBottom>
            Purpose Breakdown
          </Typography>
          {Object.keys(stats.purposeBreakdown).length === 0 ? (
            <Typography variant="body2" color="text.secondary">
              No purposes assigned to cards in this deck
            </Typography>
          ) : (
            <Box sx={{ display: 'flex', flexDirection: 'column', gap: 1 }}>
              {Object.entries(stats.purposeBreakdown)
                .sort((a, b) => b[1] - a[1])
                .slice(0, 8)
                .map(([purpose, count]) => (
                  <Box key={purpose}>
                    <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 0.5 }}>
                      <Typography variant="body2">{purpose}</Typography>
                      <Typography variant="body2" color="text.secondary">
                        {count}
                      </Typography>
                    </Box>
                    <LinearProgress
                      variant="determinate"
                      value={(count / maxPurpose) * 100}
                      sx={{ height: 6, borderRadius: 1 }}
                    />
                  </Box>
                ))}
            </Box>
          )}
        </Paper>
      </Grid>
    </Grid>
  );
}
