namespace Api.Models;

public class Equipment : PlayerItem
{
    public bool Equipped { get; set; }
    public int MaxDurability { get; set; }
    public int Durability { get; set; }
    public int Protection { get; set; }
    public int Damage { get; set; }
}