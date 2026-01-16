import { ProtectedRoute } from './ProtectedRoute';
import { Roles } from '../../types';

interface AdminRouteProps {
  children: React.ReactNode;
}

export function AdminRoute({ children }: AdminRouteProps) {
  return (
    <ProtectedRoute requiredRoles={[Roles.Admin]}>
      {children}
    </ProtectedRoute>
  );
}
