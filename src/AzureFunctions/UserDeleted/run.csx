#r "Microsoft.WindowsAzure.Storage"
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq;

public async static Task Run(NewUserQueueItem myQueueItem, CloudTable outputTable, TraceWriter log)
{
    log.Info($"User deleted: {myQueueItem.PartitionKey}, {myQueueItem.RowKey}");

    // Get user
    var retrieveOperation = TableOperation.Retrieve<User>(myQueueItem.PartitionKey, myQueueItem.RowKey);
    var result = await outputTable.ExecuteAsync(retrieveOperation);
    var user = result.Result as User;

    // Delete user
    var deleteOperation = TableOperation.Delete(user);
    var deleteResult = await outputTable.ExecuteAsync(deleteOperation);
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