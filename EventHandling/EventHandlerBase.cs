using Api.DAL;
using Newtonsoft.Json;

namespace Api.Models;

public abstract class EventHandlerBase
{
    private readonly DatabaseContext _context;

    public EventHandlerBase(DatabaseContext context)
    {
        _context = context;
    }
    
    public abstract Task Handle(Event @event);
    
    protected void SuccessfulEvent(Guid eventId)
    {
        var @event = _context.Events.Find(eventId);
        if (@event == null) throw new Exception("Event being handled is gone...");

        @event.EventStatus = EventStatus.SUCCESS;
        @event.HandledAt = DateTime.UtcNow;

        _context.Events.Update(@event);
        _context.SaveChanges();
    }

    protected void UnsuccessfulEvent(Guid eventId)
    {
        var @event = _context.Events.Find(eventId);
        if (@event == null) throw new Exception("Event being handled is gone...");

        @event.EventStatus = EventStatus.FAILED;
        @event.HandledAt = DateTime.UtcNow;

        _context.Events.Update(@event);
        _context.SaveChanges();
    }
    
    protected static T ExtractEventData<T>(Event e) where T : class
    {
        try
        {
            return JsonConvert.DeserializeObject<T>(e.Data);
        }
        catch (Exception ex)
        {
            throw new Exception("Unable to deserialize(generic) message data to object.", ex);
        }
    }
}