using Api.DAL;
using Microsoft.EntityFrameworkCore;

namespace Api.Models;

public class EventHandlerProvider
{
    
    private readonly IDbContextFactory<DatabaseContext> _contextFactory;
    
    public EventHandlerProvider(IDbContextFactory<DatabaseContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }
    public EventHandlerBase GetHandler(Type eventData)
    {
        if (eventData == null) throw new ArgumentException("EventData cannot be null when getting a handler");

        if (eventData == typeof(ItemSpawnEventData)) return new ItemSpawnHandler(_contextFactory.CreateDbContext());
        if (eventData == typeof(MissionEndedData)) return new MissionEndedHandler(_contextFactory.CreateDbContext());



        throw new KeyNotFoundException("The event type " + eventData +
                                           " has no associated handler. Unable to process event.");

    }

}