using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class Item
{
    public Item()
    {
        
    }

    [Key]
    public int ItemIndex { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public virtual ItemType Type { get; init; }
    
    public virtual ItemImage Image { get; set; }
    //Equipment
    public int MaxDurability { get; set; }
    //Armor
    public int Protection { get; set; }
}