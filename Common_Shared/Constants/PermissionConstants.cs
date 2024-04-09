namespace Common_Shared.Constants
{
    public static class PermissionConstants
    {
        public const string Create = "1";
        public const string Read = "2";
        public const string Update = "3";
        public const string Delete = "4";
        public const string ExportExcel = "5";
        public const string GetCustomer = "6";
        public const string UpdateCustomer = "7";
        public const string DeleteCustomer = "8";
        public const string ChangeCustomerPassword = "9";

        public static List<string> GetAllPermissions()
        {
            var permissionList = typeof(PermissionConstants).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                                                            .Where(a => a.FieldType == typeof(string));
            return permissionList.Select(a => a.GetValue(null).ToString()).ToList();

        }
    }
}
