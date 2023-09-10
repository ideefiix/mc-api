using Api.DAL;

namespace Api.Models;

public class ItemSpawnHandler : EventHandlerBase
{
    private readonly DatabaseContext _databaseContext;
    public ItemSpawnHandler(DatabaseContext context) : base(context)
    {
        _databaseContext = context;
    }

    public override Task Handle(Event @event)
    {
        try
        {
            var eventData = JsonConverter.DeserializeObject<ItemSpawnEventData>(@event.Data);
            var player = _databaseContext.Players.Find(eventData.PlayerId);
            if (player == null) throw new KeyNotFoundException("Could not find the player to spawn item for");
            var playerMission = _databaseContext.PlayerMissions.Find(eventData.PlayerMissionId);
            if (playerMission == null) throw new KeyNotFoundException("Could not find the playerMission to spawn item from");

            if (playerMission.Mission.ItemPool.Count == 1)
            {
                int itemId = playerMission.Mission.ItemPool.First(i => true).ItemToSpawn.ItemIndex;
                GivePlayerItem(player, itemId);

            }
            else //Select an item in the Pool
            {
                Random random = new Random();
                var itemPool = playerMission.Mission.ItemPool.ToArray();
                bool itemSpawned = false;
                
                for (var i = 0; i < itemPool.Length - 1; i++)
                {
                    if (random.NextDouble() < itemPool[i].Probability)
                    {
                        GivePlayerItem(player, itemPool[i].ItemToSpawn.ItemIndex);
                        itemSpawned = true;
                        break;
                    }
                }
                
                if(itemSpawned == false) GivePlayerItem(player, itemPool[itemPool.Length - 1].ItemToSpawn.ItemIndex);
            }
            
            //Reschedule event in 10min
            @event.Ntries += 1;
            @event.HandleAt.Value.AddMinutes(10);
            _databaseContext.Events.Update(@event);
            
            _databaseContext.SaveChanges();
           // Event is success 
        }
        catch (Exception)
        {
            UnsuccessfulEvent(@event.EventId);
            throw;
        }
        return Task.CompletedTask;
    }

    
    private void GivePlayerItem(Player p, int itemId) // Without saving changes to context
    {
        var playerItem = p.Items.FirstOrDefault(item => item.ItemId == itemId);
                
        //Check if player already has that Item
        if (playerItem != null)
        {
            playerItem.Quantity += 1;
            _databaseContext.PlayerItems.Update(playerItem);
        }
        else
        {
            var newPlayerItem = new PlayerItem
            {
                ItemId = itemId,
                OwnerId = p.PlayerId,
                Quantity = 1
            };
            _databaseContext.PlayerItems.Add(newPlayerItem);
        }
    }
}