namespace PAR.Shared.Constants;

public static class AppConstants
{
    public static class Auth
    {
        public const int MaxPasswordHistory = 3;
        public const int MaxLoginAttemptsPerIp = 10;
        public const int MaxLoginAttemptsPerUser = 5;
        public const int BruteForceWindowMinutes = 15;
    }

    public static class Roles
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }

    public static class Permissions
    {
        public const string UsersRead = "users.read";
        public const string UsersCreate = "users.create";
        public const string UsersUpdate = "users.update";
        public const string UsersDelete = "users.delete";
        public const string UsersLock = "users.lock";
    }
}
