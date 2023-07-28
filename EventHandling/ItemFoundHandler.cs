using Api.DAL;

namespace Api.Models;

public class ItemFoundHandler : EventHandlerBase
{
    public ItemFoundHandler(DatabaseContext context) : base(context)
    {
    }

    public override Task Handle(Event @event)
    {
        try
        {
            var eventData = JsonConverter.DeserializeObject<ItemFoundEvent>(@event.Data);
            throw new NotImplementedException();

        }
        catch (Exception)
        {
            UnsuccessfulEvent(@event.EventId);
            throw;
        }
        
    }
}