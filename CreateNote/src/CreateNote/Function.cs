using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using CreateNote.Models;
using System.Text.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace CreateNote;

public class Function
{
    AmazonDynamoDBClient amazonDynamoDBClient = new AmazonDynamoDBClient();
    /// <summary>
    /// A simple function to store note data into DynamoDB
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task<string> CreateNoteAsync(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        var noteRequest = JsonSerializer.Deserialize<Note>(request.Body);
        var noteId = Guid.NewGuid().ToString();
        var noteItem = new Dictionary<string, AttributeValue>()
        {
            ["message"] = new AttributeValue { S = noteRequest!.Message },
            ["id"] = new AttributeValue { S = noteId }
        };
        var createRequest = new PutItemRequest
        {
            TableName = "note",
            Item = noteItem
        };
        var response = await amazonDynamoDBClient.PutItemAsync(createRequest);
        return response.HttpStatusCode == System.Net.HttpStatusCode.OK ? noteId : string.Empty;
    }
}
