using Api.DAL;
using Quartz;

namespace Api.Models;

public class PollReadyEventJob : IJob
{
    private readonly DatabaseContext _context;
    private readonly EventHandlerProvider _eventHandlerProvider;

    public PollReadyEventJob(DatabaseContext context, EventHandlerProvider eventHandlerProvider)
    {
        _context = context;
        _eventHandlerProvider = eventHandlerProvider;
    }
    public Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("EXECUTING POLLING TASK");
        var events = _context.Events
            .Where(e => e.EventStatus == EventStatus.READY)
            .Where(e => e.HandleAt <= DateTime.UtcNow);


        // CONCURRENT handling
        if (events.Any())
        {
            var runningHandlers = new List<Thread>();
            
            foreach (var e in events)
            {
                var etype = Type.GetType(e.EventType);
                if (etype == null) throw new Exception("Could not find the events type!");

                EventHandlerBase handler = _eventHandlerProvider.GetHandler(etype);
                var thread = new Thread(() => { handler.Handle(e).Wait(); });
                runningHandlers.Add(thread);
                thread.Start();
            }
            
            foreach (var handler in runningHandlers)
            {
                //Wait for all handlers to execute
                handler.Join();
            }
        }
        else
        {
            //No events to handle
        }


        return Task.CompletedTask;
    }
}