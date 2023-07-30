using Api.DAL;

namespace Api.Models;

public class ItemSpawnHandler : EventHandlerBase
{
    public ItemSpawnHandler(DatabaseContext context) : base(context)
    {
    }

    public override Task Handle(Event @event)
    {
        try
        {
            var eventData = JsonConverter.DeserializeObject<ItemSpawnEvent>(@event.Data);
            throw new NotImplementedException();

        }
        catch (Exception)
        {
            UnsuccessfulEvent(@event.EventId);
            throw;
        }
        
    }
}