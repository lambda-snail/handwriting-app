using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Azure.Identity;

namespace HandwritingApp.Services;

public interface IDocumentAnalysisService
{
    public Task<AnalyzeResult> AnalyzeHandwriting(Uri uri);
    public Task<AnalyzeResult> AnalyzeHandwriting(Stream stream);
}

public class DocumentAnalysisService : IDocumentAnalysisService
{
    private readonly DocumentAnalysisClient _client;
    
    public DocumentAnalysisService(DefaultAzureCredential credential2)
    {
        string endpoint = "https://hedgehog-documents.cognitiveservices.azure.com/";
        //string endpoint = "https://westeurope.api.cognitive.microsoft.com/";
        
        AzureKeyCredential credential = new AzureKeyCredential(key);
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