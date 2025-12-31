import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  Typography,
  Box,
  Grid,
  Divider,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  CircularProgress,
  Alert,
} from '@mui/material';
import { ImportStatusChip } from './ImportStatusChip';
import type { CardImportDto } from '../../types';

interface ImportDetailDialogProps {
  open: boolean;
  onClose: () => void;
  importData?: CardImportDto | null;
  isLoading?: boolean;
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

function formatBytes(bytes: number): string {
  const mb = bytes / (1024 * 1024);
  return `${mb.toFixed(1)} MB`;
}

export function ImportDetailDialog({
  open,
  onClose,
  importData,
  isLoading,
}: ImportDetailDialogProps) {
  return (
    <Dialog open={open} onClose={onClose} maxWidth="md" fullWidth>
      <DialogTitle>Import Details</DialogTitle>
      <DialogContent dividers>
        {isLoading ? (
          <Box sx={{ display: 'flex', justifyContent: 'center', py: 4 }}>
            <CircularProgress />
          </Box>
        ) : importData ? (
          <Box>
            {/* Status and Error */}
            <Box sx={{ mb: 3 }}>
              <ImportStatusChip status={importData.status} size="medium" />
              {importData.errorMessage && (
                <Alert severity="error" sx={{ mt: 2 }}>
                  {importData.errorMessage}
                </Alert>
              )}
            </Box>

            {/* Stats Grid */}
            <Grid container spacing={2} sx={{ mb: 3 }}>
              <Grid size={{ xs: 6, sm: 4 }}>
                <Typography variant="body2" color="text.secondary">
                  Started
                </Typography>
                <Typography variant="body1" fontWeight={500}>
                  {formatDateTime(importData.startedAt)}
                </Typography>
              </Grid>
              <Grid size={{ xs: 6, sm: 4 }}>
                <Typography variant="body2" color="text.secondary">
                  Completed
                </Typography>
                <Typography variant="body1" fontWeight={500}>
                  {formatDateTime(importData.completedAt)}
                </Typography>
              </Grid>
              <Grid size={{ xs: 6, sm: 4 }}>
                <Typography variant="body2" color="text.secondary">
                  Duration
                </Typography>
                <Typography variant="body1" fontWeight={500}>
                  {formatDuration(importData.durationSeconds)}
                </Typography>
              </Grid>
              <Grid size={{ xs: 6, sm: 4 }}>
                <Typography variant="body2" color="text.secondary">
                  Total in File
                </Typography>
                <Typography variant="body1" fontWeight={500}>
                  {importData.totalCardsInFile.toLocaleString()}
                </Typography>
              </Grid>
              <Grid size={{ xs: 6, sm: 4 }}>
                <Typography variant="body2" color="text.secondary">
                  Processed
                </Typography>
                <Typography variant="body1" fontWeight={500}>
                  {importData.cardsProcessed.toLocaleString()}
                </Typography>
              </Grid>
              <Grid size={{ xs: 6, sm: 4 }}>
                <Typography variant="body2" color="text.secondary">
                  File Size
                </Typography>
                <Typography variant="body1" fontWeight={500}>
                  {formatBytes(importData.fileSizeBytes)}
                </Typography>
              </Grid>
            </Grid>

            <Divider sx={{ my: 2 }} />

            {/* Results */}
            <Typography variant="subtitle1" gutterBottom fontWeight={600}>
              Results
            </Typography>
            <Grid container spacing={2} sx={{ mb: 3 }}>
              <Grid size={{ xs: 6, sm: 3 }}>
                <Paper sx={{ p: 2, textAlign: 'center', bgcolor: 'success.light' }}>
                  <Typography variant="h6" color="success.dark">
                    +{importData.cardsAdded.toLocaleString()}
                  </Typography>
                  <Typography variant="body2">Added</Typography>
                </Paper>
              </Grid>
              <Grid size={{ xs: 6, sm: 3 }}>
                <Paper sx={{ p: 2, textAlign: 'center', bgcolor: 'warning.light' }}>
                  <Typography variant="h6" color="warning.dark">
                    ~{importData.cardsUpdated.toLocaleString()}
                  </Typography>
                  <Typography variant="body2">Updated</Typography>
                </Paper>
              </Grid>
              <Grid size={{ xs: 6, sm: 3 }}>
                <Paper sx={{ p: 2, textAlign: 'center', bgcolor: 'grey.100' }}>
                  <Typography variant="h6" color="text.secondary">
                    {importData.cardsSkipped.toLocaleString()}
                  </Typography>
                  <Typography variant="body2">Skipped</Typography>
                </Paper>
              </Grid>
              <Grid size={{ xs: 6, sm: 3 }}>
                <Paper sx={{ p: 2, textAlign: 'center', bgcolor: importData.cardsFailed > 0 ? 'error.light' : 'grey.100' }}>
                  <Typography variant="h6" color={importData.cardsFailed > 0 ? 'error.dark' : 'text.secondary'}>
                    {importData.cardsFailed.toLocaleString()}
                  </Typography>
                  <Typography variant="body2">Failed</Typography>
                </Paper>
              </Grid>
            </Grid>

            {/* Errors Table */}
            {importData.errors && importData.errors.length > 0 && (
              <>
                <Divider sx={{ my: 2 }} />
                <Typography variant="subtitle1" gutterBottom fontWeight={600}>
                  Errors ({importData.errors.length})
                </Typography>
                <TableContainer component={Paper} variant="outlined" sx={{ maxHeight: 300 }}>
                  <Table size="small" stickyHeader>
                    <TableHead>
                      <TableRow>
                        <TableCell>Card Name</TableCell>
                        <TableCell>Error</TableCell>
                      </TableRow>
                    </TableHead>
                    <TableBody>
                      {importData.errors.map((error) => (
                        <TableRow key={error.id}>
                          <TableCell>{error.cardName || 'Unknown'}</TableCell>
                          <TableCell sx={{ color: 'error.main' }}>{error.errorMessage}</TableCell>
                        </TableRow>
                      ))}
                    </TableBody>
                  </Table>
                </TableContainer>
              </>
            )}

            {/* Scryfall Metadata */}
            <Divider sx={{ my: 2 }} />
            <Typography variant="subtitle1" gutterBottom fontWeight={600}>
              Scryfall Data
            </Typography>
            <Typography variant="body2" color="text.secondary">
              Bulk Data ID: {importData.bulkDataId}
            </Typography>
            <Typography variant="body2" color="text.secondary">
              Scryfall Updated: {formatDateTime(importData.scryfallUpdatedAt)}
            </Typography>
          </Box>
        ) : (
          <Typography color="text.secondary">Import not found</Typography>
        )}
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Close</Button>
      </DialogActions>
    </Dialog>
  );
}
