using System.ComponentModel.Design;
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



    public async Task<string> GetHelloMessageAsync(string messageText, string clientId)
    {
        if (string.IsNullOrWhiteSpace(messageText))
            messageText = "Keine Nachricht eingegeben.";

        string requestId = Guid.NewGuid().ToString();

        string commandIdText = "9";
        string storedProcedureIdText = "00000";
        string commandText = "Command_Unknown";

        string parameterNameText = "Message";
        string parameterValueText = messageText;

        var soapXml = $@"<?xml version=""1.0""?>
            <soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                <soap:Body>
                    <Request>
                        <ClientId>{System.Security.SecurityElement.Escape(clientId)}</ClientId>
                        <RequestId>{System.Security.SecurityElement.Escape(requestId)}</RequestId>
                        <Command>
                            <CommandId>{System.Security.SecurityElement.Escape(commandIdText)}</CommandId>
                            <StoredProcedureId>{System.Security.SecurityElement.Escape(storedProcedureIdText)}</StoredProcedureId>
                            <CommandText>{System.Security.SecurityElement.Escape(commandText)}</CommandText>
                        </Command>
                        <Parameters>
                            <Parameter>
                                <ParameterName>{System.Security.SecurityElement.Escape(parameterNameText)}</ParameterName>
                                <ParameterValue>{System.Security.SecurityElement.Escape(parameterValueText)}</ParameterValue>
                            </Parameter>
                        </Parameters>
                    </Request>
                </soap:Body>
            </soap:Envelope>";

        var content = new StringContent(
            soapXml,
            Encoding.UTF8,
            "application/xml"
        );

        var response = await _httpClient.PostAsync(
            "https://mysoapapp-eqdtckhxd0enegft.canadacentral-01.azurewebsites.net/api/hello/" +
            "",
            content
        );

        string xml = await response.Content.ReadAsStringAsync();

        return xml;
    }
    private string GetClientId()
    {
        return "CLIENT-001";
    }

    public async Task<string> SendCommandAsync(
    string clientId,
    string commandId,
    string storedProcedureId,
    string commandText,
    List<(string Name, string Value)> parameters)
    {
        string requestId = Guid.NewGuid().ToString();

        var parameterXml = new StringBuilder();

        foreach (var p in parameters)
        {
            if (!string.IsNullOrWhiteSpace(p.Name) || !string.IsNullOrWhiteSpace(p.Value))
            {
                parameterXml.Append($@"
                <Parameter>
                    <ParameterName>{System.Security.SecurityElement.Escape(p.Name)}</ParameterName>
                    <ParameterValue>{System.Security.SecurityElement.Escape(p.Value)}</ParameterValue>
                </Parameter>");
            }
        }

        var soapXml = $@"<?xml version=""1.0""?>
            <soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                <soap:Body>
                    <Request>
                        <ClientId>{System.Security.SecurityElement.Escape(clientId)}</ClientId>
                        <RequestId>{System.Security.SecurityElement.Escape(requestId)}</RequestId>
                        <Command>
                            <CommandId>{System.Security.SecurityElement.Escape(commandId)}</CommandId>
                            <StoredProcedureId>{System.Security.SecurityElement.Escape(storedProcedureId)}</StoredProcedureId>
                            <CommandText>{System.Security.SecurityElement.Escape(commandText)}</CommandText>
                        </Command>
                        <Parameters>
                            {parameterXml}
                        </Parameters>
                    </Request>
                </soap:Body>
            </soap:Envelope>";

        var content = new StringContent(
            soapXml,
            Encoding.UTF8,
            "application/xml"
        );

        var response = await _httpClient.PostAsync(
            "https://mysoapapp-eqdtckhxd0enegft.canadacentral-01.azurewebsites.net/api/hello/test",
            content
        );

        return await response.Content.ReadAsStringAsync();
    }
}

