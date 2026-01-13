import { useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import {
  Box,
  Typography,
  Button,
  Tabs,
  Tab,
  Paper,
  List,
  ListItem,
  ListItemAvatar,
  ListItemText,
  Avatar,
  IconButton,
  Chip,
  CircularProgress,
  Alert,
  TextField,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
} from '@mui/material';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import AddIcon from '@mui/icons-material/Add';
import RemoveIcon from '@mui/icons-material/Remove';
import DeleteIcon from '@mui/icons-material/Delete';
import { DeckStats } from '../components';
import {
  useDeck,
  useDeckStats,
  useAddCardToDeck,
  useUpdateDeckCard,
  useRemoveCardFromDeck,
  useCardSearch,
} from '../hooks';
import type { DeckCardDto } from '../types';

export function DeckDetailPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [activeTab, setActiveTab] = useState(0);
  const [searchQuery, setSearchQuery] = useState('');
  const [addCardDialogOpen, setAddCardDialogOpen] = useState(false);

  const { data: deck, isLoading, error } = useDeck(id || '', !!id);
  const { data: stats, isLoading: statsLoading, error: statsError } = useDeckStats(id || '', !!id);
  const addCard = useAddCardToDeck(id || '');
  const updateCard = useUpdateDeckCard(id || '');
  const removeCard = useRemoveCardFromDeck(id || '');

  // Card search for adding cards
  const { data: searchResults } = useCardSearch(
    { search: searchQuery, pageSize: 10 },
    searchQuery.length >= 2
  );

  if (isLoading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', py: 8 }}>
        <CircularProgress />
      </Box>
    );
  }

  if (error || !deck) {
    return (
      <Box>
        <Button startIcon={<ArrowBackIcon />} onClick={() => navigate('/decks')} sx={{ mb: 2 }}>
          Back to Decks
        </Button>
        <Alert severity="error">
          {error?.message || 'Deck not found'}
        </Alert>
      </Box>
    );
  }

  const mainboardCards = deck.cards.filter((c) => !c.isSideboard);
  const sideboardCards = deck.cards.filter((c) => c.isSideboard);

  const handleQuantityChange = async (card: DeckCardDto, delta: number) => {
    const newQuantity = card.quantity + delta;
    if (newQuantity <= 0) {
      await removeCard.mutateAsync(card.cardId);
    } else {
      await updateCard.mutateAsync({ cardId: card.cardId, request: { quantity: newQuantity } });
    }
  };

  const handleToggleSideboard = async (card: DeckCardDto) => {
    await updateCard.mutateAsync({
      cardId: card.cardId,
      request: { isSideboard: !card.isSideboard },
    });
  };

  const handleAddCard = async (cardId: string, isSideboard: boolean) => {
    await addCard.mutateAsync({ cardId, quantity: 1, isSideboard });
    setAddCardDialogOpen(false);
    setSearchQuery('');
  };

  const renderCardList = (cards: DeckCardDto[], title: string) => (
    <Box>
      <Typography variant="h6" sx={{ mb: 1 }}>
        {title} ({cards.reduce((sum, c) => sum + c.quantity, 0)} cards)
      </Typography>
      {cards.length === 0 ? (
        <Typography color="text.secondary">No cards</Typography>
      ) : (
        <List dense>
          {cards.map((card) => (
            <ListItem
              key={card.cardId}
              secondaryAction={
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                  <IconButton
                    size="small"
                    onClick={() => handleQuantityChange(card, -1)}
                    disabled={updateCard.isPending || removeCard.isPending}
                  >
                    <RemoveIcon fontSize="small" />
                  </IconButton>
                  <Typography sx={{ minWidth: 20, textAlign: 'center' }}>{card.quantity}</Typography>
                  <IconButton
                    size="small"
                    onClick={() => handleQuantityChange(card, 1)}
                    disabled={updateCard.isPending}
                  >
                    <AddIcon fontSize="small" />
                  </IconButton>
                  <IconButton
                    size="small"
                    onClick={() => handleToggleSideboard(card)}
                    disabled={updateCard.isPending}
                    title={card.isSideboard ? 'Move to mainboard' : 'Move to sideboard'}
                  >
                    <Typography variant="caption" sx={{ fontSize: 10 }}>
                      {card.isSideboard ? 'MB' : 'SB'}
                    </Typography>
                  </IconButton>
                  <IconButton
                    size="small"
                    color="error"
                    onClick={() => removeCard.mutateAsync(card.cardId)}
                    disabled={removeCard.isPending}
                  >
                    <DeleteIcon fontSize="small" />
                  </IconButton>
                </Box>
              }
            >
              <ListItemAvatar>
                <Avatar
                  variant="rounded"
                  src={card.imageUrl}
                  sx={{ width: 40, height: 56 }}
                />
              </ListItemAvatar>
              <ListItemText
                primary={card.name}
                secondary={
                  <Box sx={{ display: 'flex', gap: 0.5, flexWrap: 'wrap', mt: 0.5 }}>
                    {card.manaCost && <Chip label={card.manaCost} size="small" />}
                    <Chip label={card.typeLine} size="small" variant="outlined" />
                    {card.purposes.slice(0, 2).map((p) => (
                      <Chip key={p} label={p} size="small" color="primary" variant="outlined" />
                    ))}
                  </Box>
                }
              />
            </ListItem>
          ))}
        </List>
      )}
    </Box>
  );

  return (
    <Box>
      <Button startIcon={<ArrowBackIcon />} onClick={() => navigate('/decks')} sx={{ mb: 2 }}>
        Back to Decks
      </Button>

      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', mb: 3 }}>
        <Box>
          <Typography variant="h4" fontWeight={700}>
            {deck.name}
          </Typography>
          {deck.description && (
            <Typography color="text.secondary" sx={{ mt: 0.5 }}>
              {deck.description}
            </Typography>
          )}
          <Box sx={{ display: 'flex', gap: 1, mt: 1 }}>
            {deck.format && <Chip label={deck.format} color="primary" />}
            <Chip label={`${deck.mainboardCount} mainboard`} variant="outlined" />
            <Chip label={`${deck.sideboardCount} sideboard`} variant="outlined" />
          </Box>
        </Box>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={() => setAddCardDialogOpen(true)}
        >
          Add Card
        </Button>
      </Box>

      <Paper sx={{ mb: 3 }}>
        <Tabs value={activeTab} onChange={(_, v) => setActiveTab(v)}>
          <Tab label="Cards" />
          <Tab label="Statistics" />
        </Tabs>
      </Paper>

      {activeTab === 0 && (
        <Box sx={{ display: 'grid', gridTemplateColumns: { xs: '1fr', md: '2fr 1fr' }, gap: 3 }}>
          <Paper sx={{ p: 2 }}>{renderCardList(mainboardCards, 'Mainboard')}</Paper>
          <Paper sx={{ p: 2 }}>{renderCardList(sideboardCards, 'Sideboard')}</Paper>
        </Box>
      )}

      {activeTab === 1 && (
        <DeckStats stats={stats} isLoading={statsLoading} error={statsError} />
      )}

      {/* Add Card Dialog */}
      <Dialog open={addCardDialogOpen} onClose={() => setAddCardDialogOpen(false)} maxWidth="sm" fullWidth>
        <DialogTitle>Add Card to Deck</DialogTitle>
        <DialogContent>
          <TextField
            autoFocus
            fullWidth
            label="Search cards"
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            sx={{ mt: 1 }}
          />
          {searchResults && searchResults.items.length > 0 && (
            <List>
              {searchResults.items.map((card) => (
                <ListItem
                  key={card.id}
                  secondaryAction={
                    <Box>
                      <Button
                        size="small"
                        onClick={() => handleAddCard(card.id, false)}
                        disabled={addCard.isPending}
                      >
                        Main
                      </Button>
                      <Button
                        size="small"
                        onClick={() => handleAddCard(card.id, true)}
                        disabled={addCard.isPending}
                      >
                        Side
                      </Button>
                    </Box>
                  }
                >
                  <ListItemAvatar>
                    <Avatar variant="rounded" src={card.imageUri} sx={{ width: 30, height: 42 }} />
                  </ListItemAvatar>
                  <ListItemText primary={card.name} secondary={card.typeLine} />
                </ListItem>
              ))}
            </List>
          )}
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setAddCardDialogOpen(false)}>Close</Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
}
