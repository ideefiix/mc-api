using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Api.Models;

public class ItemType
{
    [Key]
    public int TypeId { get; set; }
    public string TypeName { get; set; }
}