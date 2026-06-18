using System.Text;
using System.Xml.Linq;


namespace HelloSoapService.Services;

public class HelloSoapService
{
    private readonly HttpClient _httpClient;

    public HelloSoapService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    

    public async Task<string> GetHelloMessageAsync(string messageText)
    {
        if (string.IsNullOrWhiteSpace(messageText))
        {
            messageText = "Keine Nachricht eingegeben.";
        }

        // fertiges SOAP XML aufbauen
        var soapXml = $@"<?xml version=""1.0""?>
                    <soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                        <soap:Body>
                            <Message>{System.Security.SecurityElement.Escape(messageText)}</Message>
                        </soap:Body>
                    </soap:Envelope>";

        var content = new StringContent(
            soapXml,
            Encoding.UTF8,
            "application/xml"
        );

        var response = await _httpClient.PostAsync(
            "https://mysoapapp-eqdtckhxd0enegft.canadacentral-01.azurewebsites.net/api/hello",
            content
        );

        var xml = await response.Content.ReadAsStringAsync();

        Console.WriteLine(xml);

        return xml;
    }

}

