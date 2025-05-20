using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon;

public class DynamoUserRepository : IUserRepository
{
    private readonly AmazonDynamoDBClient _client;
    private readonly string _tableName = "Users";

    public DynamoUserRepository()
    {
        var config = new AmazonDynamoDBConfig { RegionEndpoint = RegionEndpoint.USEast1 };
        _client = new AmazonDynamoDBClient(config);
    }

    public async Task SaveUserAsync(UserData user)
    {
        var item = new Dictionary<string, AttributeValue>
        {
            { "userID", new AttributeValue { S = user.userID } },
            { "email", new AttributeValue { S = user.email } },
            { "RegisteredAt", new AttributeValue { S = DateTime.UtcNow.ToString("o") } }
        };

        var request = new PutItemRequest
        {
            TableName = _tableName,
            Item = item
        };

        await _client.PutItemAsync(request);
    }
}