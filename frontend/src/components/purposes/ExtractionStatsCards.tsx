import { Grid, Paper, Typography, Skeleton, Box } from '@mui/material';
import CategoryIcon from '@mui/icons-material/Category';
import CheckCircleIcon from '@mui/icons-material/CheckCircle';
import LabelIcon from '@mui/icons-material/Label';
import PendingIcon from '@mui/icons-material/Pending';
import AccessTimeIcon from '@mui/icons-material/AccessTime';
import PercentIcon from '@mui/icons-material/Percent';
import type { ExtractionStatsDto } from '../../types';

interface ExtractionStatsCardsProps {
  stats?: ExtractionStatsDto;
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

export function ExtractionStatsCards({ stats, isLoading }: ExtractionStatsCardsProps) {
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

  const totalCards = (stats?.cardsWithPurposes ?? 0) + (stats?.cardsWithoutPurposes ?? 0);
  const coveragePercent = totalCards > 0
    ? ((stats?.cardsWithPurposes ?? 0) / totalCards * 100).toFixed(1)
    : '0';

  const successRate = stats?.totalJobs
    ? ((stats.successfulJobs / stats.totalJobs) * 100).toFixed(1)
    : '0';

  return (
    <Grid container spacing={2}>
      <Grid size={{ xs: 6, sm: 4, md: 2 }}>
        <StatCard
          title="Total Jobs"
          value={stats?.totalJobs ?? 0}
          icon={<CategoryIcon />}
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
          title="Purposes Assigned"
          value={stats?.totalPurposesAssigned?.toLocaleString() ?? 0}
          icon={<LabelIcon />}
          color="info.main"
        />
      </Grid>
      <Grid size={{ xs: 6, sm: 4, md: 2 }}>
        <StatCard
          title="Cards Pending"
          value={stats?.cardsWithoutPurposes?.toLocaleString() ?? 0}
          icon={<PendingIcon />}
          color="warning.main"
        />
      </Grid>
      <Grid size={{ xs: 6, sm: 4, md: 2 }}>
        <StatCard
          title="Coverage"
          value={`${coveragePercent}%`}
          icon={<PercentIcon />}
          color="success.main"
        />
      </Grid>
      <Grid size={{ xs: 6, sm: 4, md: 2 }}>
        <StatCard
          title="Last Extraction"
          value={formatTimeAgo(stats?.lastExtraction)}
          icon={<AccessTimeIcon />}
        />
      </Grid>
    </Grid>
  );
}
