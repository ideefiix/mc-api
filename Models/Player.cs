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
    public Guid? HelmId { get; set; }
    public virtual Equipment? Chest { get; set; }
    public Guid? ChestId { get; set; }
    public virtual Equipment? Legs { get; set; }
    public Guid? LegsId { get; set; }
    public virtual Equipment? Boots { get; set; }
    public Guid? BootsId { get; set; }
    public virtual Equipment? Weapon { get; set; }
    public Guid? WeaponId { get; set; }
    public virtual Equipment? Pet { get; set; }
    public Guid? PetId { get; set; }
    
    public void EquipItem(Equipment item)
    {
        if (item.Item.Type.TypeId == 2)
        {
            int damageDiff = item.Damage - (Helm?.Damage ?? 0);
            int protecDiff = item.Protection - (Helm?.Protection ?? 0);
            Helm = item;
            Dmg += damageDiff;
            Defence += protecDiff;
        }else if (item.Item.Type.TypeId == 3)
        {
            int damageDiff = item.Damage - (Chest?.Damage ?? 0);
            int protecDiff = item.Protection - (Chest?.Protection ?? 0);
            Chest = item;
            Dmg += damageDiff;
            Defence += protecDiff;
        }else if (item.Item.Type.TypeId == 4)
        {
            int damageDiff = item.Damage - (Legs?.Damage ?? 0);
            int protecDiff = item.Protection - (Legs?.Protection ?? 0);
            Legs = item;
            Dmg += damageDiff;
            Defence += protecDiff;
        }else if (item.Item.Type.TypeId == 5)
        {
            int damageDiff = item.Damage - (Boots?.Damage ?? 0);
            int protecDiff = item.Protection - (Boots?.Protection ?? 0);
            Boots = item;
            Dmg += damageDiff;
            Defence += protecDiff;
        }else if (item.Item.Type.TypeId == 6)
        {
            int damageDiff = item.Damage - (Weapon?.Damage ?? 0);
            int protecDiff = item.Protection - (Weapon?.Protection ?? 0);
            Weapon = item;
            Dmg += damageDiff;
            Defence += protecDiff;
        }else if (item.Item.Type.TypeId == 7)
        {
            int damageDiff = item.Damage - (Pet?.Damage ?? 0);
            int protecDiff = item.Protection - (Pet?.Protection ?? 0);
            Pet = item;
            Dmg += damageDiff;
            Defence += protecDiff;
        }
        else
        {
            throw new ArgumentException("Item to be equipped must be of a Equipment type");
        }
    }
}