namespace Common_Shared.Constants
{
    public static class PermissionConstants
    {
        public const string Create = "1";
        public const string Read = "2";
        public const string Update = "3";
        public const string Delete = "4";

        public static List<string> GetAllPermissions()
        {
            var permissionList = typeof(PermissionConstants).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                                                            .Where(a => a.FieldType == typeof(string));
            return permissionList.Select(a => a.GetValue(null).ToString()).ToList();

        }
    }
}
