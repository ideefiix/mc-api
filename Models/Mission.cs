using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class Mission
{
    [Key]
    public int MissionIndex { get; init; }
    public string Name { get; set; }
    public float Duration { get; set; } // In hours
    public virtual ICollection<ItemSpawnProbability> ItemPool { get; init; }
    public string CompletionReward { get; init; }
}

public class ItemReward
{
    public int ItemIndex { get; set; }
    public int Quantity { get; set; }
}

// The Dto has deserialized the CompletionReward
public class MissionDto
{
    public int MissionIndex { get; init; }
    public string Name { get; set; }
    public float Duration { get; set; } // In hours
    public virtual ICollection<ItemSpawnProbability> ItemPool { get; init; }
    public ICollection<ItemReward> CompletionReward { get; init; }
}