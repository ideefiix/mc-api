namespace Api.Models;

public class EventHandlerProvider
{
    private readonly Dictionary<Type, EventHandlerBase> _handlerBindings = new Dictionary<Type, EventHandlerBase>();
    
    public EventHandlerProvider(
        ItemSpawnHandler itemSpawnHandler)
    {
        _handlerBindings.Add(typeof(ItemSpawnEventData), itemSpawnHandler);
    }
    public EventHandlerBase GetHandler(Type eventType)
    {
        var handler = _handlerBindings.GetValueOrDefault(eventType);
        if (handler == null)
            throw new KeyNotFoundException("The event type " + eventType +
                                           " has no associated handler. Unable to process event.");
        return handler;
    }

}