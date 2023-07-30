using System.Drawing;
using System.Reflection;
using System.Text.Json;
using Api.DAL.DTO;
using Api.Models;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Webp;
using Image = SixLabors.ImageSharp.Image;

namespace Api.DAL.DBinitialization;

public static class DatabaseInitializer
{
   public static void Initialize(DatabaseContext context)
   {
      if (context.Players.Any())
      {
         return; //Database is already seeded
      }
      
      //Load the itemTypes

      var itemTypes = JsonSerializer.Deserialize<ItemType[]>(File.ReadAllText("DAL/DBinitialization/itemTypes.json"));
      context.ItemTypes.AddRange(itemTypes);
      context.SaveChanges();
      
      //Load the Items -> Connect with Image and type

      var dtoList = JsonSerializer.Deserialize<RegistryItemFromJsonDTO[]>(
         File.ReadAllText("DAL/DBinitialization/items.json"));


      foreach (var dto in dtoList)
      {
         if (File.Exists(dto.ImagePath) == false) throw new ArgumentException("FilePath has no file");
         byte[] imageBytes;
         using (var image = Image.Load(dto.ImagePath))
         {
            using (MemoryStream m = new MemoryStream())
            {
               image.Save(m, new WebpEncoder()); // Images must be webp
               imageBytes = m.ToArray();
            }
         }

         ItemImage imageEntity = new ItemImage();
         imageEntity.Image = imageBytes;

         var item = new Item()
         {
            ItemIndex = dto.ItemIndex,
            Name = dto.Name,
            Description = dto.Description,
            Type = itemTypes.First(type => type.TypeId == dto.TypeId),
            Image = imageEntity,
         };
         context.Items.AddRange(item);
      }
      
      context.SaveChanges();
      
      // Load tasks
      var missionList = JsonSerializer.Deserialize<MissionDto[]>(
         File.ReadAllText("DAL/DBinitialization/missions.json"));

      var missionsToAdd = new List<Mission>();
      foreach (var dto in missionList)
      {
         missionsToAdd.Add(new Mission
         {
            MissionIndex = dto.MissionIndex,
            Name = dto.Name,
            Duration = dto.Duration,
            ItemPool = JsonConverter.SerializeObject(dto.ItemPool),
            CompletionReward = JsonConverter.SerializeObject(dto.CompletionReward)
         });
      }
      
      context.Missions.AddRange(missionsToAdd);
      context.SaveChanges();
      
      // SEED DATABASE
      
      string hashedPassword = BCrypt.Net.BCrypt.HashPassword("admin");

      var players = new Player[]
      {
         new Player { PlayerId = Guid.Parse("43321d77-3d6e-40e6-8d1e-114688272001"), PlayerName = "admin", PasswordHash = hashedPassword, MaxHp = 100, Hp = 100, Dmg = 0, Defence = 0 }
      };
      context.Players.AddRange(players);

      var itemEntity = context.Items.Find(1);
      var itemEntity2 = context.Items.Find(2);
      
      var playerItem = new PlayerItem()
      {
         Item = itemEntity,
         Owner = players[0],
         Quantity = 5
      };
      var playerItem2 = new PlayerItem()
      {
         Item = itemEntity2,
         Owner = players[0],
         Quantity = 1
      };

      context.PlayerItems.Add(playerItem);
      context.PlayerItems.Add(playerItem2);

      context.SaveChanges();

      var eventData = new ItemSpawnEvent()
      {
         PlayerId = Guid.Parse("43321d77-3d6e-40e6-8d1e-114688272001"),
         TaskId = Guid.Parse("43321d77-3d6e-40e6-8d1e-114688272001")
      };
      
      var @event = new Event
      {
         EventType = typeof(ItemSpawnEvent).AssemblyQualifiedName,
         EventStatus = EventStatus.READY,
         CreatedAt = DateTime.UtcNow,
         HandledAt = null,
         Ntries = 0,
         Data = JsonConverter.SerializeObject(eventData),
      };

      context.Events.Add(@event);
      context.SaveChanges();

   }
}