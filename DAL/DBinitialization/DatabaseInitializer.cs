﻿using System.Drawing;
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
         File.ReadAllText("DAL/DBinitialization/itemRegistry.json"));


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
            MaxDurability = dto.MaxDurability,
            Protection = dto.Protection
         };
         context.Items.AddRange(item);
      }
      
      context.SaveChanges();

      var players = new Player[]
      {
         new Player { MaxHp = 100, Hp = 100, Dmg = 0, Defence = 0 }
      };
      context.Players.AddRange(players);

      /*var itemEntity = context.Items.Find(1);

      var playerItem = new PlayerItem()
      {
         Name = itemEntity.Name,
         Description = itemEntity.Description,
         Type = itemEntity.Type,
         Owner = players[0],
         Quantity = 5
      };

      context.PlayerItems.Add(playerItem);*/

      context.SaveChanges();
      
      /*var itemHelm = context.Items.Find(2);

      var playerHelm = new Equipment
      {
         Name = itemHelm.Name,
         Description = itemHelm.Description,
         Type = itemHelm.Type,
         Owner = players[0],
         Quantity = 1,
         Equipped = true,
         MaxDurability = itemHelm.MaxDurability,
         Durability = itemHelm.MaxDurability,
         Protection = itemHelm.Protection
      };
      context.Equipments.Add(playerHelm);
      context.SaveChanges();

      var equipHelm = new PlayerEquippedItem
      {
         Player = players[0],
         ItemType = playerHelm.Type,
         Equipment = playerHelm
      };
      context.PlayerEquippedItems.Add(equipHelm);
      context.SaveChanges();*/

   }
}