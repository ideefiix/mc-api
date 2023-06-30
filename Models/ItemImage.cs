using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Api.Models;

public class ItemImage
{

    [Key]
    public Guid ImageId { get; set; } 
    public byte[] Image { get; set; }
    
}