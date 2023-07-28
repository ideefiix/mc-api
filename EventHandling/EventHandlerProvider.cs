namespace Api.Models;

public class EventHandlerProvider
{
    private readonly Dictionary<Type, EventHandlerBase> _handlerBindings = new Dictionary<Type, EventHandlerBase>();
    
    public EventHandlerProvider(
        ItemFoundHandler itemFoundHandler)
    {
        _handlerBindings.Add(typeof(ItemFoundEvent), itemFoundHandler);
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