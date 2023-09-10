namespace Api.Models;

public class Event
{
    public Guid EventId { get; set; }
    public string EventType { get; set; }
    public EventStatus EventStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? HandleAt { get; set; }
    public int Ntries { get; set; }
    public string Data { get; set; }
}