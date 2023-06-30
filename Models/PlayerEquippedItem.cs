using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Api.Models;


public class PlayerEquippedItem 
{
    /*public Guid PlayerEquippedItemId { get; set; }*/
    public Guid PlayerId { get; set; }
    public virtual Player Player { get; set; }
    public int ItemTypeId { get; set; }
    public virtual ItemType ItemType { get; set; }
    public Guid ItemId { get; set; }
    public virtual PlayerItem Item { get; set; }
}