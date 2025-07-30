namespace CatGallery2.Infrastructure.MinioStorage;

internal sealed class MinioRepositoryOptions
{
    public string Endpoint { get; set; }
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    public string BucketName { get; set; }
    public bool UseSsl { get; set; }
    public static readonly string SectionName = nameof(MinioRepositoryOptions);
}