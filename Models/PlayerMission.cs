using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class PlayerMission
{
    [Key]
    public Guid PlayerMissionId { get; set; }
    public virtual Task Task { get; set; }
    public virtual Player Player { get; set; }
    public virtual ICollection<Item> FoundItems { get; set; }
    public DateTime StartedTime { get; set; }
    public DateTime FinishedTime { get; set; }
    public int HpThreshold { get; set; } // TODO
    public virtual Event CompletionEvent { get; set; }
    public virtual Event ItemSpawnEvent { get; set; }

}