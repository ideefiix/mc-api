namespace Api.Models;

public class ItemFoundEvent 
{
    public Guid PlayerId { get; set; }
    public Guid TaskId { get; set; }
}