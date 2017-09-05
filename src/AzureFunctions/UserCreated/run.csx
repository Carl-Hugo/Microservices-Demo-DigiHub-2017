#r "Microsoft.WindowsAzure.Storage"
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq;

public async static Task Run(NewUserQueueItem myQueueItem, CloudTable inputTable, CloudTable outputTable, TraceWriter log)
{
    log.Info($"User created: {myQueueItem.PartitionKey}, {myQueueItem.RowKey}");

    // Get data
    var retrieveOperation = TableOperation.Retrieve<User>(myQueueItem.PartitionKey, myQueueItem.RowKey);
    var result = await inputTable.ExecuteAsync(retrieveOperation);
    var user = result.Result as User;

    // Save data
    var insertOperation = TableOperation.InsertOrReplace(user);
    var insertResult = await outputTable.ExecuteAsync(insertOperation);
}

public class NewUserQueueItem
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
}

public class User : TableEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}