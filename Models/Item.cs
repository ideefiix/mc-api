using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace Api.Models;

public class Item
{
    public Item()
    {
        
    }

    private Item(ILazyLoader lazyLoader)
    {
        LazyLoader = lazyLoader;
    }
    
    [Key]
    public int ItemIndex { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public virtual ItemType Type { get; init; }
    private ILazyLoader LazyLoader { get; set; }

    private ItemImage _image;

    public ItemImage Image
    {
        get => LazyLoader.Load(this, ref _image);
        set => _image = value;
    }
    //Equipment
    public int MaxDurability { get; set; }
    //Armor
    public int Protection { get; set; }
}