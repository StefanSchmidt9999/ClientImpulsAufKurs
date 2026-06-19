using System.Xml.Linq;

namespace HelloSoapService.Services;

public class ClientIdService
{
    private readonly HttpClient _httpClient;

    public ClientIdService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string?> GetClientIdAsync()
    {
        var xml = await _httpClient.GetStringAsync(
            "http://127.0.0.1:5055/clientid");

        XDocument doc = XDocument.Parse(xml);

        string? status =
            doc.Descendants("Status").FirstOrDefault()?.Value;

        if (status != "OK")
            return null;

        string? clientId =
            doc.Descendants("ClientId").FirstOrDefault()?.Value;

        return clientId;
    }
}