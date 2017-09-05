#r "Microsoft.WindowsAzure.Storage"
using Microsoft.WindowsAzure.Storage.Table;
using System;

public static async Task Run(DeleteEntityMessage deleteBoardMessage, CloudTable boardsTable, TraceWriter log)
{
    log.Info($"Delete board: {deleteBoardMessage.PartitionKey} + {deleteBoardMessage.RowKey}");

    // Get the board to delete
    var retrieveOperation = TableOperation.Retrieve<DynamicTableEntity>(deleteBoardMessage.PartitionKey, deleteBoardMessage.RowKey);
    var getResult = await boardsTable.ExecuteAsync(retrieveOperation);
    if (getResult == null || (getResult.HttpStatusCode < 200 && getResult.HttpStatusCode >= 300) || getResult.Result == null)
    {
        log.Info($"Board does not exists: {deleteBoardMessage.PartitionKey} + {deleteBoardMessage.RowKey}");
        return;
    }

    // Delete the board
    var deleteOperation = TableOperation.Delete(getResult.Result as DynamicTableEntity);
    var deleteResult = await boardsTable.ExecuteAsync(deleteOperation);
    if (deleteResult == null || (deleteResult.HttpStatusCode < 200 && deleteResult.HttpStatusCode >= 300) || deleteResult.Result == null)
    {
        log.Info($"Deleting board failed: {deleteBoardMessage.PartitionKey} + {deleteBoardMessage.RowKey}");
    }
    else 
    {
        log.Info($"Board deleted: {deleteBoardMessage.PartitionKey} + {deleteBoardMessage.RowKey}");
    }
}

public class DeleteEntityMessage
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
}