export interface Company { id: string; name: string; document?: string; phone?: string; email?: string; isActive: boolean; createdAt: string; }
export interface Branch { id: string; name: string; address?: string; phone?: string; isActive: boolean; createdAt: string; companyId: string; }
export interface App { id: string; name: string; description?: string; url?: string; icon?: string; isActive: boolean; createdAt: string; companyId: string; }
export interface Functionality { id: string; name: string; description?: string; code?: string; isActive: boolean; createdAt: string; applicationId: string; }
export interface Role { id: string; name: string; description?: string; isActive: boolean; createdAt: string; applicationId: string; }
export interface RoleFunctionality { id: string; canCreate: boolean; canRead: boolean; canUpdate: boolean; canDelete: boolean; createdAt: string; roleId: string; functionalityId: string; }
export interface User { id: string; name: string; email: string; status: string; isSuperAdmin: boolean; ssoProvider?: string; createdAt: string; lastLoginAt?: string; companyId?: string; }
export interface UserAppRole { id: string; createdAt: string; userId: string; applicationId: string; roleId: string; }
export interface Permission { id: string; resource: string; action: string; isAllowed: boolean; createdAt: string; userId: string; }
export interface AuthResponse { token: string; refreshToken: string; user: User; claims: UserAppClaim[]; }
export interface UserAppClaim { applicationId: string; applicationName: string; roleId: string; roleName: string; permissions: string[]; }
export interface DashboardSummary { totalCompanies: number; totalApplications: number; totalUsers: number; totalRoles: number; activeUsers: number; inactiveUsers: number; }
export interface RecentActivity { description: string; type: string; date: string; }

export type EntityType = 'companies' | 'branches' | 'applications' | 'functionalities' | 'roles' | 'rolefunctionalities' | 'users' | 'userapplicationroles' | 'permissions';
