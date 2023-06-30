namespace Api.Models;

public class Player
{
    public Guid PlayerId { get; set; }
    public int MaxHp { get; set; }
    public int Hp { get; set; }
    /*public virtual ICollection<PlayerEquippedItem> EquippedItems { get; set; }*/
    public virtual ICollection<PlayerItem> Items { get; set; }
    public int Dmg { get; set; }
    public int Defence { get; set; }
    
    public virtual Equipment? Helm { get; set; }
    public virtual Equipment? Chest { get; set; }
    public virtual Equipment? Legs { get; set; }
    public virtual Equipment? Boots { get; set; }
    public virtual Equipment? Weapon { get; set; }
    public virtual Equipment? Pet { get; set; }

    public void EquipItem(Equipment item)
    {
        if (item.Item.Type.TypeId == 2)
        {
            int damageDiff = item.Damage - (Helm?.Damage ?? 0);
            int ProtecDiff
        }
    }
}