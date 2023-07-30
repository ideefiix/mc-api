using Api.DAL;

namespace Api.Models;

public class ItemSpawnHandler : EventHandlerBase
{
    private DatabaseContext _databaseContext;
    public ItemSpawnHandler(DatabaseContext context) : base(context)
    {
        _databaseContext = context;
    }

    public override Task Handle(Event @event)
    {
        try
        {
            var eventData = JsonConverter.DeserializeObject<ItemSpawnEventData>(@event.Data);
            var mission = _databaseContext.Missions.Find(eventData.MissionId);
            if (mission == null)
            {
                throw new KeyNotFoundException("Could not find the missions to spawn item from");
            }

            throw new NotImplementedException();
            //spawnItem()


        }
        catch (Exception)
        {
            UnsuccessfulEvent(@event.EventId);
            throw;
        }
        
    }
}