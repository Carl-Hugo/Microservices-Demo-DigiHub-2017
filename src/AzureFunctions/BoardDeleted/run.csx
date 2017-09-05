#r "Microsoft.WindowsAzure.Storage"
#r "Newtonsoft.Json"
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using Newtonsoft.Json;

public static async Task Run(DeletedBoardQueueItem deletedBoard, CloudTable boardsTable, CloudTable cardsTable, CloudQueue deleteBoardQueue, CloudTable usersTable, TraceWriter log)
{
    // TODO: The deletion was done by the board owner, delete this board and cards of other users as well. 
    log.Info($"Deleting board: {deletedBoard.PartitionKey} + {deletedBoard.RowKey}");
    log.Info($"Deleting cards of board: {deletedBoard.PartitionKey} + {deletedBoard.RowKey}");
    
    // Gathering Info
    var boardId = deletedBoard.RowKey;
    
    // Queue the board deletion for all users
    TableContinuationToken usersContinuationToken = null;
    do
    {
        var users = await DynamicQueryTable(usersTable, new TableQuery(), usersContinuationToken);
        var queueDeleteBoardMessages = users
            .Select(user => deleteBoardQueue.AddMessageAsync(new CloudQueueMessage(JsonConvert.SerializeObject(new DeleteEntityMessage
            {
                PartitionKey = user.RowKey,
                RowKey = boardId
            }))));
        await Task.WhenAll(queueDeleteBoardMessages);
    } while (usersContinuationToken != null);
    
    // Delete all cards of the deleted board
    var queryCards = new TableQuery()
        .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, boardId));
    TableContinuationToken cardsContinuationToken = null;
    do
    {
        var cards = await DynamicQueryTable(cardsTable, queryCards, cardsContinuationToken);
        var cardsToDelete = cards
            .Select(card => cardsTable.ExecuteAsync(TableOperation.Delete(card)));
        await Task.WhenAll(cardsToDelete);
    } while (cardsContinuationToken != null);
}

public static async Task<IEnumerable<DynamicTableEntity>> DynamicQueryTable(CloudTable table, TableQuery query, TableContinuationToken continuationToken)
{
    var segment = await table.ExecuteQuerySegmentedAsync(query, continuationToken);
    continuationToken = segment.ContinuationToken;
    return segment;
}

public class DeletedBoardQueueItem
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
}

public class DeleteEntityMessage
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
}