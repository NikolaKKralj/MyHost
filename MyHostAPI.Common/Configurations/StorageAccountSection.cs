namespace MyHostAPI.Common.Configurations
{
    public class StorageAccountSection
    {
        public const string Name = "StorageAccount";
        public string StorageConnectionString { get; set; } = null!;
        public string FullImagesContainerNameOption { get; set; } = null!;
    }
}
