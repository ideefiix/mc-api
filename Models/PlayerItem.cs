namespace Api.Models;

public class PlayerItem
{
    public Guid PlayerItemId { get; set; }

    public virtual Item Item { get; set; }
    public virtual Player Owner { get; set; }
    public int Quantity { get; set; } 
}