import {
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  TablePagination,
  Skeleton,
  IconButton,
  Tooltip,
} from '@mui/material';
import InfoIcon from '@mui/icons-material/Info';
import { ImportStatusChip } from './ImportStatusChip';
import type { CardImportSummaryDto } from '../../types';

interface ImportHistoryTableProps {
  imports?: CardImportSummaryDto[];
  totalCount?: number;
  page: number;
  pageSize: number;
  isLoading?: boolean;
  onPageChange: (page: number) => void;
  onPageSizeChange: (pageSize: number) => void;
  onViewDetails: (id: string) => void;
}

function formatDateTime(date?: string): string {
  if (!date) return '-';
  return new Date(date).toLocaleString();
}

function formatDuration(seconds?: number): string {
  if (!seconds) return '-';
  const minutes = Math.floor(seconds / 60);
  const secs = Math.floor(seconds % 60);
  return `${minutes}m ${secs}s`;
}

export function ImportHistoryTable({
  imports,
  totalCount = 0,
  page,
  pageSize,
  isLoading,
  onPageChange,
  onPageSizeChange,
  onViewDetails,
}: ImportHistoryTableProps) {
  if (isLoading) {
    return (
      <Paper>
        <TableContainer>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>Started</TableCell>
                <TableCell>Duration</TableCell>
                <TableCell>Status</TableCell>
                <TableCell align="right">Added</TableCell>
                <TableCell align="right">Updated</TableCell>
                <TableCell align="right">Failed</TableCell>
                <TableCell align="center">Actions</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {Array.from({ length: 5 }).map((_, i) => (
                <TableRow key={i}>
                  {Array.from({ length: 7 }).map((_, j) => (
                    <TableCell key={j}>
                      <Skeleton />
                    </TableCell>
                  ))}
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      </Paper>
    );
  }

  return (
    <Paper>
      <TableContainer>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Started</TableCell>
              <TableCell>Duration</TableCell>
              <TableCell>Status</TableCell>
              <TableCell align="right">Added</TableCell>
              <TableCell align="right">Updated</TableCell>
              <TableCell align="right">Failed</TableCell>
              <TableCell align="center">Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {imports?.map((imp) => (
              <TableRow key={imp.id} hover>
                <TableCell>{formatDateTime(imp.startedAt)}</TableCell>
                <TableCell>{formatDuration(imp.durationSeconds)}</TableCell>
                <TableCell>
                  <ImportStatusChip status={imp.status} />
                </TableCell>
                <TableCell align="right" sx={{ color: 'success.main', fontWeight: 500 }}>
                  +{imp.cardsAdded.toLocaleString()}
                </TableCell>
                <TableCell align="right" sx={{ color: 'warning.main', fontWeight: 500 }}>
                  ~{imp.cardsUpdated.toLocaleString()}
                </TableCell>
                <TableCell align="right" sx={{ color: imp.cardsFailed > 0 ? 'error.main' : 'text.secondary', fontWeight: 500 }}>
                  {imp.cardsFailed.toLocaleString()}
                </TableCell>
                <TableCell align="center">
                  <Tooltip title="View Details">
                    <IconButton size="small" onClick={() => onViewDetails(imp.id)}>
                      <InfoIcon />
                    </IconButton>
                  </Tooltip>
                </TableCell>
              </TableRow>
            ))}
            {imports?.length === 0 && (
              <TableRow>
                <TableCell colSpan={7} align="center" sx={{ py: 4, color: 'text.secondary' }}>
                  No imports found
                </TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </TableContainer>
      <TablePagination
        component="div"
        count={totalCount}
        page={page - 1}
        onPageChange={(_, newPage) => onPageChange(newPage + 1)}
        rowsPerPage={pageSize}
        onRowsPerPageChange={(e) => onPageSizeChange(parseInt(e.target.value, 10))}
        rowsPerPageOptions={[10, 20, 50]}
      />
    </Paper>
  );
}
