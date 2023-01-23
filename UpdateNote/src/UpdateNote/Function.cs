using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using UpdateNote.Models;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace UpdateNote;

public class Function
{
    AmazonDynamoDBClient amazonDynamoDBClient = new AmazonDynamoDBClient();

    /// <summary>
    /// A simple function to update note data in DynamoDB
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task<bool> UpdateNoteAsync(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        var noteRequest = JsonSerializer.Deserialize<Note>(request.Body);
        var noteItem = new Dictionary<string, AttributeValue>()
        {
            ["message"] = new AttributeValue { S = noteRequest!.Message },
            ["id"] = new AttributeValue { S = noteRequest.Id.ToString() }
        };
        var updateRequest = new PutItemRequest
        {
            TableName = "note",
            Item = noteItem
        };
        var response = await amazonDynamoDBClient.PutItemAsync(updateRequest);
        return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
    }
}
