import { useState } from 'react';
import { Container, Typography, Box, Alert } from '@mui/material';
import { ImportStatsCards } from '../components/admin/ImportStatsCards';
import { ImportHistoryTable } from '../components/admin/ImportHistoryTable';
import { ImportDetailDialog } from '../components/admin/ImportDetailDialog';
import { useImportStats, useImportHistory, useImport } from '../hooks';

export function AdminImportsPage() {
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [selectedImportId, setSelectedImportId] = useState<string | null>(null);

  const { data: stats, isLoading: statsLoading, error: statsError } = useImportStats();
  const {
    data: historyData,
    isLoading: historyLoading,
    error: historyError,
  } = useImportHistory(page, pageSize);
  const {
    data: selectedImport,
    isLoading: importLoading,
  } = useImport(selectedImportId ?? '');

  const handleViewDetails = (id: string) => {
    setSelectedImportId(id);
  };

  const handleCloseDialog = () => {
    setSelectedImportId(null);
  };

  const error = statsError || historyError;

  return (
    <Container maxWidth="xl" sx={{ py: 3 }}>
      <Box sx={{ mb: 4 }}>
        <Typography variant="h4" component="h1" gutterBottom fontWeight={600}>
          Card Imports
        </Typography>
        <Typography variant="body1" color="text.secondary">
          Monitor Scryfall card import jobs and view import history.
        </Typography>
      </Box>

      {error && (
        <Alert severity="error" sx={{ mb: 3 }}>
          Failed to load import data. Please try again later.
        </Alert>
      )}

      <Box sx={{ mb: 4 }}>
        <Typography variant="h6" gutterBottom fontWeight={500}>
          Statistics
        </Typography>
        <ImportStatsCards stats={stats} isLoading={statsLoading} />
      </Box>

      <Box>
        <Typography variant="h6" gutterBottom fontWeight={500}>
          Import History
        </Typography>
        <ImportHistoryTable
          imports={historyData?.items}
          totalCount={historyData?.totalCount}
          page={page}
          pageSize={pageSize}
          isLoading={historyLoading}
          onPageChange={setPage}
          onPageSizeChange={(newPageSize) => {
            setPageSize(newPageSize);
            setPage(1);
          }}
          onViewDetails={handleViewDetails}
        />
      </Box>

      <ImportDetailDialog
        open={!!selectedImportId}
        onClose={handleCloseDialog}
        importData={selectedImport}
        isLoading={importLoading}
      />
    </Container>
  );
}
