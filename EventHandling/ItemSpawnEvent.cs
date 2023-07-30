namespace Api.Models;

public class ItemSpawnEvent 
{
    public Guid PlayerId { get; set; }
    public Guid TaskId { get; set; }
}