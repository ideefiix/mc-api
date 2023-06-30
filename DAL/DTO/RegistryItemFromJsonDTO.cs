namespace Api.DAL.DTO;

public class RegistryItemFromJsonDTO
{
    public int ItemIndex { get; set; }
    public string Name { get; init; }
    public string Description { get; init; }
    public int TypeId { get; init; }
    public string ImagePath { get; set; }
    //Equipment
    public int MaxDurability { get; set; }
    //Armor
    public int Protection { get; set; }
}