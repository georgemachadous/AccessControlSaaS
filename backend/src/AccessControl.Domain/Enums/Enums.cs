namespace AccessControl.Domain.Entities;

public enum UserStatus
{
    Active,
    Inactive,
    Suspended,
    Pending
}

public enum LogAction
{
    Create,
    Update,
    Delete,
    Login,
    Logout,
    FailedLogin
}
