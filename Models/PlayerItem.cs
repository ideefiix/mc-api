namespace Api.Models;

public class PlayerItem
{
    public Guid PlayerItemId { get; set; }

    public virtual Item Item { get; set; }
    public int ItemId { get; set; }
    public virtual Player Owner { get; set; }
    public Guid OwnerId { get; set; }
    public int Quantity { get; set; } 
}