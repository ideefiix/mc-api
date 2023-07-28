using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class Mission
{
    [Key]
    public int MissionIndex { get; init; }
    public string Name { get; set; }
    public float Duration { get; set; } // In hours
    public string ItemPool { get; init; }
    public string CompletionReward { get; init; }
}

public class ItemReward
{
    public int ItemIndex { get; set; }
    public int Quantity { get; set; }
}

public class ItemProbability
{
    public int ItemIndex { get; set; }
    public float Probability { get; set; }
}

public class MissionDto
{
    public int MissionIndex { get; init; }
    public string Name { get; set; }
    public float Duration { get; set; } // In hours
    public ICollection<ItemProbability> ItemPool { get; init; }
    public ICollection<ItemReward> CompletionReward { get; init; }
}