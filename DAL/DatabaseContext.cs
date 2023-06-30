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
    /*public DbSet<PlayerEquippedItem> PlayerEquippedItems { get; set; } = null!;*/
    public DbSet<Item> Items { get; set; } = null!;
    public DbSet<ItemImage> ItemImages { get; set; } = null!;
    public DbSet<ItemType> ItemTypes { get; set; } = null!;
    public DbSet<Equipment> Equipments { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Configure relationships here.
        modelBuilder.Entity<Player>().ToTable("Players");
        modelBuilder.Entity<PlayerItem>().ToTable("PlayerItems");
        /*modelBuilder.Entity<PlayerEquippedItem>().ToTable("PlayerEquippedItems");
        modelBuilder.Entity<PlayerEquippedItem>()
            .HasOne(e => e.Player)
            .WithMany()
            .HasForeignKey(e => e.PlayerId);
        modelBuilder.Entity<PlayerEquippedItem>()
            .HasOne(e => e.ItemType)
            .WithMany()
            .HasForeignKey(e => e.ItemTypeId);
        modelBuilder.Entity<PlayerEquippedItem>()
            .HasOne(e => e.Item)
            .WithOne();*/
        /*modelBuilder.Entity<PlayerEquippedItem>().HasKey(e => new { e.PlayerId, e.ItemTypeId });*/


        modelBuilder.Entity<Item>().ToTable("Items");
        modelBuilder.Entity<ItemImage>().ToTable("ItemImages");
        modelBuilder.Entity<ItemType>().ToTable("ItemTypes");
        modelBuilder.Entity<Equipment>().ToTable("Equipments");
    }
}