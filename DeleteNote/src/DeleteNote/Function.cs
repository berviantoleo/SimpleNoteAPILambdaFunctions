using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace DeleteNote;

public class Function
{
    AmazonDynamoDBClient amazonDynamoDBClient = new AmazonDynamoDBClient();
  
    /// <summary>
    /// A simple function that delete a note from DynamoDB
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task<bool> DeleteNoteAsync(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        var dataRequest = JsonSerializer.Deserialize<Dictionary<string, string>>(request.Body);
        var item = new Dictionary<string, AttributeValue>()
        {
            ["id"] = new AttributeValue(dataRequest!["Id"])
        };
        var response = await amazonDynamoDBClient.DeleteItemAsync("note", item);
        return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
    }
}
