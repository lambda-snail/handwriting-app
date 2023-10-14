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
    private readonly DocumentAnalysisClient _client;

    public bool HasAzureConnection { get; init; }

    public DocumentAnalysisService()
    {
        string endpoint = SecureStorage.Default.GetAsync("di_endpoint").Result;
        string key = SecureStorage.Default.GetAsync("di_key").Result;
        HasAzureConnection = !string.IsNullOrWhiteSpace(endpoint) && !string.IsNullOrWhiteSpace(key);
        if (HasAzureConnection)
        {
            AzureKeyCredential credential = new AzureKeyCredential(key);
            _client = new DocumentAnalysisClient(new Uri(endpoint), credential);
        }
    }

    public async Task<AnalyzeResult> AnalyzeHandwriting_WaitUntilDone(Uri uri)
    {
        AnalyzeDocumentOperation operation = await _client.AnalyzeDocumentFromUriAsync(WaitUntil.Completed, "prebuilt-layout", uri);
        return operation.Value;
    }
    
    public async Task<AnalyzeResult> AnalyzeHandwriting_WaitUntilDone(Stream stream)
    {
        AnalyzeDocumentOperation operation = await _client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-layout", stream);
        return operation.Value;
    }
}