import {
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Skeleton,
  Typography,
  Box,
} from '@mui/material';
import { ExtractionStatusChip } from './ExtractionStatusChip';
import type { PurposeExtractionJobSummaryDto } from '../../types';

interface ExtractionHistoryTableProps {
  jobs?: PurposeExtractionJobSummaryDto[];
  isLoading?: boolean;
  onJobClick?: (job: PurposeExtractionJobSummaryDto) => void;
}

function formatDate(date: string): string {
  return new Date(date).toLocaleString();
}

function formatDuration(start: string, end?: string): string {
  if (!end) return '-';
  const startDate = new Date(start);
  const endDate = new Date(end);
  const seconds = Math.floor((endDate.getTime() - startDate.getTime()) / 1000);
  const minutes = Math.floor(seconds / 60);
  const secs = seconds % 60;
  return `${minutes}m ${secs}s`;
}

export function ExtractionHistoryTable({ jobs, isLoading, onJobClick }: ExtractionHistoryTableProps) {
  if (isLoading) {
    return (
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Started</TableCell>
              <TableCell>Status</TableCell>
              <TableCell align="right">Processed</TableCell>
              <TableCell align="right">Assigned</TableCell>
              <TableCell align="right">Duration</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {Array.from({ length: 5 }).map((_, i) => (
              <TableRow key={i}>
                <TableCell><Skeleton /></TableCell>
                <TableCell><Skeleton width={80} /></TableCell>
                <TableCell><Skeleton width={60} /></TableCell>
                <TableCell><Skeleton width={60} /></TableCell>
                <TableCell><Skeleton width={60} /></TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    );
  }

  if (!jobs?.length) {
    return (
      <Paper sx={{ p: 4, textAlign: 'center' }}>
        <Typography color="text.secondary">
          No extraction jobs yet. Trigger an extraction to categorize your cards.
        </Typography>
      </Paper>
    );
  }

  return (
    <TableContainer component={Paper}>
      <Table>
        <TableHead>
          <TableRow>
            <TableCell>Started</TableCell>
            <TableCell>Status</TableCell>
            <TableCell align="right">Processed</TableCell>
            <TableCell align="right">Assigned</TableCell>
            <TableCell align="right">Duration</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {jobs.map((job) => (
            <TableRow
              key={job.id}
              hover
              onClick={() => onJobClick?.(job)}
              sx={{ cursor: onJobClick ? 'pointer' : 'default' }}
            >
              <TableCell>
                <Box>
                  <Typography variant="body2">{formatDate(job.startedAt)}</Typography>
                </Box>
              </TableCell>
              <TableCell>
                <ExtractionStatusChip status={job.status} />
              </TableCell>
              <TableCell align="right">
                {job.processedCards.toLocaleString()}
              </TableCell>
              <TableCell align="right">
                {job.purposesAssigned.toLocaleString()}
              </TableCell>
              <TableCell align="right">
                {formatDuration(job.startedAt, job.completedAt)}
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
}
