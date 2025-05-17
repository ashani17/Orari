namespace Orari.Constants
{
    public static class UserRoles
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string Admin = "Admin";
        public const string Professor = "Professor";
        public const string Student = "Student";

        public static readonly IReadOnlyList<string> AllRoles = new[] 
        { 
            SuperAdmin,
            Admin, 
            Professor, 
            Student 
        };

        public static readonly IReadOnlyList<string> VisibleRoles = new[] 
        { 
            Admin, 
            Professor, 
            Student 
        };
    }
} 