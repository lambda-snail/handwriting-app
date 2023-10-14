using Azure.Storage.Blobs;
using OneOf;

namespace HandwritingApp.Services;

public record MissingConnectionString();

public class BlobClientFactory
{
    public async Task<OneOf<BlobServiceClient, MissingConnectionString>> GetBlobClientAsync()
    {
        string connectionString = await SecureStorage.Default.GetAsync("blob_connection_string");
        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            return new BlobServiceClient(connectionString);
        }
        else
        {
            return new MissingConnectionString();
        }
    }
}