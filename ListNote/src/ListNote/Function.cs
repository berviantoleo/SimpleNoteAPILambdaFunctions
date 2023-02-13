using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using ListNote.Models;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ListNote;

public class Function
{
    AmazonDynamoDBClient amazonDynamoDBClient = new AmazonDynamoDBClient();

    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task<List<Note>> ListNoteAsync(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
    {
        var dataRequest = request.QueryStringParameters ?? new Dictionary<string, string>();
        var specificId = dataRequest.ContainsKey("id");
        var queryRequest = new ScanRequest()
        {
            TableName = "note",
        };
        if (specificId)
        {
            queryRequest.FilterExpression = "id = :v_id";
            queryRequest.ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
            {
                {":v_id", new AttributeValue() { S = dataRequest["id"] }}
            };
        }

        var response = await amazonDynamoDBClient.ScanAsync(queryRequest);
        return response.Items.Select(item => new Note()
        {
            Id = Guid.Parse(item["id"].S),
            Message = item["message"].S
        }).ToList();
    }
}
