using Api.DAL;

namespace Api.Models;

public class MissionEndedHandler : EventHandlerBase
{
    private DatabaseContext _databaseContext;
    public MissionEndedHandler(DatabaseContext context) : base(context)
    {
        _databaseContext = context;
    }

    public override Task Handle(Event @event)
    {
        try
        {
            var eventData = JsonConverter.DeserializeObject<MissionEndedData>(@event.Data);
            var playerMission = _databaseContext.PlayerMissions.Find(eventData.PlayerMissionId);
            if (playerMission == null) throw new KeyNotFoundException("The mission to end does not exist");

            var items =
                JsonConverter.DeserializeObject<ICollection<ItemReward>>(playerMission.Mission.CompletionReward);

            foreach (var itemReward in items)
            {
                GivePlayerItems(playerMission.Player, itemReward.ItemIndex, itemReward.Quantity);
            }

            playerMission.ItemSpawnEvent.EventStatus = EventStatus.SUCCESS;
            _databaseContext.PlayerMissions.Remove(playerMission);

            _databaseContext.SaveChanges();
            //TODO create mission report
            
            SuccessfulEvent(@event.EventId);
        }
        catch (Exception)
        {
            UnsuccessfulEvent(@event.EventId);
            throw;
        }
        
        return Task.CompletedTask;
    }
    
    private void GivePlayerItems(Player p, int itemId, int quantity) // Without saving changes to context
    {
        var playerItem = p.Items.FirstOrDefault(item => item.ItemId == itemId);
                    
        //Check if player already has that Item
        if (playerItem != null)
        {
            playerItem.Quantity += quantity;
            _databaseContext.PlayerItems.Update(playerItem);
        }
        else
        {
            var newPlayerItem = new PlayerItem
            {
                ItemId = itemId,
                OwnerId = p.PlayerId,
                Quantity = quantity
            };
            _databaseContext.PlayerItems.Add(newPlayerItem);
        }
    }
}

