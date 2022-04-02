namespace Application.Configurations
{
    public class AzureConfiguration
    {
        public string? ConnectionString { get; set; }
        public string? BaseUrl { get; set; }
        public string? SecondaryUrl { get; set; }
        public string? ProfileToken { get; set; }
        public string? DocToken { get; set; }   
        public string? ProfileContainer { get; set; }
        public string? DocContainer { get; set; }
    }
}
