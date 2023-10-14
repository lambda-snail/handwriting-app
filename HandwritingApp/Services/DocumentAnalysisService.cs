using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Azure.Identity;

namespace HandwritingApp.Services;

public class DocumentAnalysisService
{
    private readonly DocumentAnalysisClient _client;
    
    public DocumentAnalysisService(DefaultAzureCredential credential)
    {
        string endpoint = "https://westeurope.api.cognitive.microsoft.com/";
        _client = new DocumentAnalysisClient(new Uri(endpoint), credential);
    }

    public async Task<AnalyzeResult> AnalyzeHandwriting(Uri uri)
    {
        AnalyzeDocumentOperation operation = await _client.AnalyzeDocumentFromUriAsync(WaitUntil.Completed, "prebuilt-layout", uri);
        return operation.Value;
    }
    
    public async Task<AnalyzeResult> AnalyzeHandwriting(Stream stream)
    {
        AnalyzeDocumentOperation operation = await _client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-layout", stream);
        return operation.Value;
    }
}