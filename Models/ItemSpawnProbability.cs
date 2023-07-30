namespace Api.Models;

public class ItemSpawnProbability
{
    public Guid ItemSpawnProbabilityId { get; set; }
    public virtual Mission BelongsToMission { get; set; }
    public virtual Item ItemToSpawn { get; set; }
    public float Probability { get; set; }
}