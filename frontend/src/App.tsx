import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ThemeProvider, CssBaseline } from '@mui/material';
import theme from './theme';
import { MainLayout } from './layouts';
import {
  HomePage,
  CardSearchPage,
  CardDetailPage,
  SetBrowserPage,
  LoginPage,
  RegisterPage,
  NotFoundPage,
  AdminImportsPage,
  DecksPage,
  DeckDetailPage,
} from './pages';
import { ProtectedRoute } from './components';
import { useEffect } from 'react';
import { useAuthStore } from './stores';

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      retry: 1,
      refetchOnWindowFocus: false,
    },
  },
});

function AuthInitializer({ children }: { children: React.ReactNode }) {
  const { checkAuth } = useAuthStore();

  useEffect(() => {
    checkAuth();
  }, [checkAuth]);

  return <>{children}</>;
}

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <ThemeProvider theme={theme}>
        <CssBaseline />
        <BrowserRouter>
          <AuthInitializer>
            <Routes>
              <Route path="/" element={<MainLayout />}>
                <Route index element={<HomePage />} />
                <Route path="search" element={<CardSearchPage />} />
                <Route path="cards/:id" element={<CardDetailPage />} />
                <Route path="sets" element={<SetBrowserPage />} />
                <Route path="login" element={<LoginPage />} />
                <Route path="register" element={<RegisterPage />} />
                <Route path="decks" element={<ProtectedRoute><DecksPage /></ProtectedRoute>} />
                <Route path="decks/:id" element={<ProtectedRoute><DeckDetailPage /></ProtectedRoute>} />
                <Route path="admin/imports" element={<AdminImportsPage />} />
                <Route path="*" element={<NotFoundPage />} />
              </Route>
            </Routes>
          </AuthInitializer>
        </BrowserRouter>
      </ThemeProvider>
    </QueryClientProvider>
  );
}

export default App;
