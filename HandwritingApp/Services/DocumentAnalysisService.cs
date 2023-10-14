using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Azure.Identity;

namespace HandwritingApp.Services;

public interface IDocumentAnalysisService
{
    public bool HasAzureConnection { get; }
    public Task<AnalyzeResult> AnalyzeHandwriting_WaitUntilDone(Uri uri);
    public Task<AnalyzeResult> AnalyzeHandwriting_WaitUntilDone(Stream stream);
}

public class DocumentAnalysisService : IDocumentAnalysisService
{
    private DocumentAnalysisClient _client;

    public bool HasAzureConnection { get; private set; }

    public DocumentAnalysisService() { }

    public async Task<AnalyzeResult> AnalyzeHandwriting_WaitUntilDone(Uri uri)
    {
        await EnsureClientSetUp();
        AnalyzeDocumentOperation operation = await _client.AnalyzeDocumentFromUriAsync(WaitUntil.Completed, "prebuilt-layout", uri);
        return operation.Value;
    }
    
    public async Task<AnalyzeResult> AnalyzeHandwriting_WaitUntilDone(Stream stream)
    {
        await EnsureClientSetUp();
        AnalyzeDocumentOperation operation = await _client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-layout", stream);
        return operation.Value;
    }

    private async Task EnsureClientSetUp()
    {
        if (HasAzureConnection)
        {
            return;
        }
        
        string endpoint = await SecureStorage.Default.GetAsync("di_endpoint");
        string key = await SecureStorage.Default.GetAsync("di_key");
        HasAzureConnection = !string.IsNullOrWhiteSpace(endpoint) && !string.IsNullOrWhiteSpace(key);
        if (HasAzureConnection)
        {
            AzureKeyCredential credential = new AzureKeyCredential(key);
            _client = new DocumentAnalysisClient(new Uri(endpoint), credential);
        }
    }
}