import { Grid, Paper, Typography, Skeleton, Box } from '@mui/material';
import CloudDownloadIcon from '@mui/icons-material/CloudDownload';
import CheckCircleIcon from '@mui/icons-material/CheckCircle';
import AddCircleIcon from '@mui/icons-material/AddCircle';
import UpdateIcon from '@mui/icons-material/Update';
import AccessTimeIcon from '@mui/icons-material/AccessTime';
import SpeedIcon from '@mui/icons-material/Speed';
import type { ImportStatsDto } from '../../types';

interface ImportStatsCardsProps {
  stats?: ImportStatsDto;
  isLoading?: boolean;
}

interface StatCardProps {
  title: string;
  value: string | number;
  icon: React.ReactNode;
  color?: string;
}

function StatCard({ title, value, icon, color = 'primary.main' }: StatCardProps) {
  return (
    <Paper sx={{ p: 2, height: '100%' }}>
      <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, mb: 1 }}>
        <Box sx={{ color }}>{icon}</Box>
        <Typography variant="body2" color="text.secondary">
          {title}
        </Typography>
      </Box>
      <Typography variant="h5" fontWeight={600}>
        {value}
      </Typography>
    </Paper>
  );
}

function formatDuration(seconds?: number): string {
  if (!seconds) return '-';
  const minutes = Math.floor(seconds / 60);
  const secs = Math.floor(seconds % 60);
  return `${minutes}m ${secs}s`;
}

function formatTimeAgo(date?: string): string {
  if (!date) return 'Never';
  const now = new Date();
  const past = new Date(date);
  const diffMs = now.getTime() - past.getTime();
  const diffHours = Math.floor(diffMs / (1000 * 60 * 60));
  const diffDays = Math.floor(diffHours / 24);

  if (diffDays > 0) return `${diffDays}d ago`;
  if (diffHours > 0) return `${diffHours}h ago`;
  const diffMinutes = Math.floor(diffMs / (1000 * 60));
  if (diffMinutes > 0) return `${diffMinutes}m ago`;
  return 'Just now';
}

export function ImportStatsCards({ stats, isLoading }: ImportStatsCardsProps) {
  if (isLoading) {
    return (
      <Grid container spacing={2}>
        {Array.from({ length: 6 }).map((_, i) => (
          <Grid key={i} size={{ xs: 6, sm: 4, md: 2 }}>
            <Skeleton variant="rectangular" height={100} sx={{ borderRadius: 1 }} />
          </Grid>
        ))}
      </Grid>
    );
  }

  const successRate = stats?.totalImports
    ? ((stats.successfulImports / stats.totalImports) * 100).toFixed(1)
    : '0';

  return (
    <Grid container spacing={2}>
      <Grid size={{ xs: 6, sm: 4, md: 2 }}>
        <StatCard
          title="Total Imports"
          value={stats?.totalImports ?? 0}
          icon={<CloudDownloadIcon />}
        />
      </Grid>
      <Grid size={{ xs: 6, sm: 4, md: 2 }}>
        <StatCard
          title="Success Rate"
          value={`${successRate}%`}
          icon={<CheckCircleIcon />}
          color="success.main"
        />
      </Grid>
      <Grid size={{ xs: 6, sm: 4, md: 2 }}>
        <StatCard
          title="Cards Added"
          value={stats?.totalCardsAdded?.toLocaleString() ?? 0}
          icon={<AddCircleIcon />}
          color="info.main"
        />
      </Grid>
      <Grid size={{ xs: 6, sm: 4, md: 2 }}>
        <StatCard
          title="Cards Updated"
          value={stats?.totalCardsUpdated?.toLocaleString() ?? 0}
          icon={<UpdateIcon />}
          color="warning.main"
        />
      </Grid>
      <Grid size={{ xs: 6, sm: 4, md: 2 }}>
        <StatCard
          title="Last Import"
          value={formatTimeAgo(stats?.lastImportAt)}
          icon={<AccessTimeIcon />}
        />
      </Grid>
      <Grid size={{ xs: 6, sm: 4, md: 2 }}>
        <StatCard
          title="Avg Duration"
          value={formatDuration(stats?.averageDurationSeconds)}
          icon={<SpeedIcon />}
        />
      </Grid>
    </Grid>
  );
}
