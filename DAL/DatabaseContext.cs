using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.DAL;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    public DbSet<Player> Players { get; set; } = null!;
    public DbSet<PlayerItem> PlayerItems { get; set; } = null!;
    public DbSet<Item> Items { get; set; } = null!;
    public DbSet<ItemImage> ItemImages { get; set; } = null!;
    public DbSet<ItemSpawnProbability> ItemSpawnProbabilities { get; set; } = null!;
    public DbSet<Mission> Missions { get; set; } = null!;
    public DbSet<PlayerMission> PlayerMissions { get; set; } = null!;
    public DbSet<ItemType> ItemTypes { get; set; } = null!;
    public DbSet<Equipment> Equipments { get; set; } = null!;
    public DbSet<Event> Events { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Configure relationships here.
        modelBuilder.Entity<Player>().ToTable("Players");
        modelBuilder.Entity<PlayerItem>().ToTable("PlayerItems");
        modelBuilder.Entity<Item>().ToTable("Items");
        modelBuilder.Entity<ItemImage>().ToTable("ItemImages");
        modelBuilder.Entity<ItemType>().ToTable("ItemTypes");
        modelBuilder.Entity<ItemSpawnProbability>().ToTable("ItemSpawnProbabilities");
        modelBuilder.Entity<Equipment>().ToTable("Equipments");
        modelBuilder.Entity<Event>().ToTable("Events");
        modelBuilder.Entity<Mission>().ToTable("Missions");
        modelBuilder.Entity<PlayerMission>().ToTable("PlayerMissions");
    }
}