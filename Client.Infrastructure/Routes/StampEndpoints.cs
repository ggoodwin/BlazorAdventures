namespace Client.Infrastructure.Routes
{
    public static class StampEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Export = "api/v1/stamp/export";

        public static string GetAll = "api/v1/stamp";
        public static string Delete = "api/v1/stamp";
        public static string Save = "api/v1/stamp";
        public static string GetCount = "api/v1/stamp/count";
        public static string Import = "api/v1/stamp/import";
    }
}
