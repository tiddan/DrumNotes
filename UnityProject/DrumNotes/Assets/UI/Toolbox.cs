// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Globalization;
// using System.IO;
// using System.IO.Compression;
// using System.Linq;
// using System.Text;
// using AgeOfSpaceData;
// using AgeOfSpaceData.GameData.Models;
// using AgeOfSpaceData.GameData.Models.Items;
// using AgeOfSpaceData.GameData.Models.Skills;
// using AgeOfSpaceData.Models.Items;
// using AgeOfSpaceData.SaveGame.Items;
// using AgeOfSpaceData.SaveGame.Ships;
// using Assets.Scripts.UI.FleetInfo;
// using PodPalGames.AgeOfSpace;
// using PodPalGames.AgeOfSpace.Network.Tokens;
// using UnityEngine;
//
// namespace PodPalGames.Common
// {
//     public static class Toolbox
//     {
//         public static void SetLayerRecursively(GameObject gameObject, int layer)
//         {
//             gameObject.layer = layer;
//         
//             foreach (Transform child in gameObject.transform)
//             {
//                 SetLayerRecursively(child.gameObject, layer);
//             }
//         }
//
//         public static void SetTagRecursively(GameObject gameObject, string tag)
//         {
//             gameObject.tag = tag;
//             foreach (Transform child in gameObject.transform)
//             {
//                 SetTagRecursively(child.gameObject, tag);
//             }
//         }
//
//         public static Vector3 Vector3FromStruct(Vector3Struct vectorStruct)
//         {
//             return new Vector3(
//                 vectorStruct.x,
//                 vectorStruct.y,
//                 vectorStruct.z
//             );
//         }
//
//         public static Vector3Struct Vector3ToStruct(Vector3 v)
//         {
//             return new Vector3Struct()
//             {
//                 x = v.x,
//                 y = v.y,
//                 z = v.z
//             };
//         }
//
//         public static CharacterInstance CharacterInstanceFromSlim(CharacterInstanceSlim slim)
//         {
//             var c = new CharacterInstance();
//             c.Character = GameConstants.Instance.AOSData.Characters.First(x => x.Id == slim.CId);
//             c.CharacterId = slim.CId;
//             foreach (var skillStr in slim.S)
//             {
//                 var parts = skillStr.Split("$"[0]);
//                 var shortSkillId = int.Parse(parts[0]);
//                 var level = int.Parse(parts[1]);
//                 c.Skills.Add(new SkillLink() {SkillId = GameConstants.Instance.GetLongSkillId(shortSkillId), Level = level});
//             }
//
//             foreach (var invStr in slim.I)
//             {
//                 var bookInfo = GameConstants.Instance.AOSData.Books.First(x => x.Id == invStr);
//                 var bookInstance = new ItemInstance<Book>();
//                 bookInstance.Configure(bookInfo);
//                 c.Inventory.Add(bookInstance);
//             }
//             return c;
//         }
//         
//         public static ShipInstance CreateFromSlipShip(ShipInstanceSlim slimShip)
//         {
//             try
//             {
//                 var shipInstance = new ShipInstance();
//                 shipInstance.Id = slimShip.InvId;
//                 shipInstance.ItemId = slimShip.Id;
//                 shipInstance.ItemData = GameConstants.Instance.AOSData.Ships.FirstOrDefault(x => x.Id == slimShip.Id);
//
//                 foreach (var ts in slimShip.T)
//                 {
//                     if (ts == null) continue;
//                     var index = int.Parse(ts[0].ToString());
//                     var tId = ts.Substring(2);
//                     var tacticalInstance = new ItemInstance<Item>();
//                     var tacticalInfo = GameConstants.Instance.AOSData.TacticalModules.First(x => x.Id == tId);
//                     tacticalInstance.GridX = index;
//                     tacticalInstance.Configure(tacticalInfo, 1);
//                     shipInstance.TacticalModules.Add(tacticalInstance);
//                 }
//
//                 foreach (var am in slimShip.A)
//                 {
//                     if (am == null) continue;
//                     var index = int.Parse(am[0].ToString());
//                     var aId = am.Substring(2);
//                     var activeInstance = new ItemInstance<Item>();
//                     var activeInfo = GameConstants.Instance.AOSData.ActiveModules.First(x => x.Id == aId);
//                     activeInstance.GridX = index;
//                     activeInstance.Configure(activeInfo, 1);
//                     shipInstance.ActiveModules.Add(activeInstance);
//                 }
//
//                 foreach (var pm in slimShip.P)
//                 {
//                     if (pm == null) continue;
//                     var index = int.Parse(pm[0].ToString());
//                     var pId = pm.Substring(2);
//                     var passiveInstance = new ItemInstance<Item>();
//                     var passiveInfo = GameConstants.Instance.AOSData.PassiveModules.First(x => x.Id == pId);
//                     passiveInstance.GridX = index;
//                     passiveInstance.Configure(passiveInfo, 1);
//                     shipInstance.PassiveModules.Add(passiveInstance);
//                 }
//
//                 foreach (var b in slimShip.B)
//                 {
//                     var parts = b.Split("$"[0]);
//                     var attribute = (Attributes) (int.Parse(parts[0]));
//                     var value = float.Parse(parts[1], CultureInfo.InvariantCulture);
//
//                     shipInstance.Boosts.Add(new AttributeBoostLink() {Attribute = attribute, Value = value});
//                 }
//
//                 return shipInstance;
//             }
//             catch(Exception e)
//             {
//                 Console.WriteLine(e.Message);
//                 return null;
//             }
//         }
//
//         public static byte[] CompressJson(string json)
//         {
//             var bytes = Encoding.UTF8.GetBytes(json);
//             var compressedBytes = CompressByteArray(bytes);
//             return compressedBytes;
//         }
//
//         public static string DecompressJson(byte[] compressedBytes)
//         {
//             var bytes = DecompressByteArray(compressedBytes);
//             var json = Encoding.UTF8.GetString(bytes);
//             return json;
//         }
//         
//         public static byte[] CompressByteArray(byte[] uncompressedData)
//         {
//             
//             using (var outputStream = new MemoryStream())
//             {
//                 using (var gZipStream = new GZipStream(outputStream, CompressionMode.Compress))
//                     gZipStream.Write(uncompressedData, 0, uncompressedData.Length);
//  
//                 var outputBytes = outputStream.ToArray();
//                 return outputBytes;
//             }
//         }
//
//         public static byte[] DecompressByteArray(byte[] compressedData)
//         {
//             using (var inputStream = new MemoryStream(compressedData))
//             using (var gZipStream = new GZipStream(inputStream, CompressionMode.Decompress))
//             using (var outputStream = new MemoryStream())
//             {
//                 gZipStream.CopyTo(outputStream);
//                 var outputBytes = outputStream.ToArray();
//                 return outputBytes;
//             }
//         }
//
//         public static string ToRomanNumbers(int number)
//         {
//             switch (number)
//             {
//                 case 0: return "";
//                 case 1: return "I";
//                 case 2: return "II";
//                 case 3: return "III";
//                 case 4: return "IV";
//                 case 5: return "V";
//                 default: return "";
//             }
//         }
//         
//         private static void AddToResource(List<ResourceCost> list, List<ResourceCost> cost)
//         {
//             foreach (var resourceCost in cost)
//             {
//                 var row = list.FirstOrDefault(x => x.MineralId == resourceCost.MineralId);
//                 if (row == null)
//                 {
//                     row = new ResourceCost() { MineralId = resourceCost.MineralId, Cost = 0};
//                     list.Add(row);
//                 }
//                 row.Cost += resourceCost.Cost;
//             }
//         }
//
//         public static IEnumerator WaitAndExecute(float delay, Action action)
//         {
//             yield return new WaitForSeconds(delay);
//             action?.Invoke();
//         }
//
//         public static bool CheckIfShipIsFlyable(CharacterInstance charInstance, ShipInstance shipInstance)
//         {
//             var bonuses = new ShipBonusSheet(null);
//             bonuses.Init(charInstance, shipInstance);
//
//             var shipInfo = GameConstants.Instance.AOSData.Ships.First(x => x.Id == shipInstance.ItemId);
//             
//             var power = (float)shipInfo.BasePower;
//             var cpu = (float)shipInfo.BaseCpu;
//
//             if(bonuses.BonusDict.ContainsKey(Attributes.SHIP_POWER_MULTIPLY))
//                 power *= ShipBonusSheet.PercentToMultiplier(bonuses.BonusDict[Attributes.SHIP_POWER_MULTIPLY]);
//             if(bonuses.BonusDict.ContainsKey(Attributes.SHIP_CPU_MULTIPLY))
//                 cpu *= ShipBonusSheet.PercentToMultiplier(bonuses.BonusDict[Attributes.SHIP_CPU_MULTIPLY]);
//
//             var powerUse = 0f;
//             var cpuUse = 0f;
//
//             foreach (var t in shipInstance.TacticalModules)
//             {
//                 var tInfo = GameConstants.Instance.AOSData.TacticalModules.First(x => x.Id == t.ItemId);
//                 powerUse += tInfo.PowerReq;
//                 cpuUse += tInfo.CpuReq;
//             }
//             
//             foreach (var a in shipInstance.ActiveModules)
//             {
//                 var aInfo = GameConstants.Instance.AOSData.ActiveModules.First(x => x.Id == a.ItemId);
//                 powerUse += aInfo.PowerReq;
//                 cpuUse += aInfo.CpuReq;
//             }
//
//             return (powerUse < power) && (cpuUse < cpu);
//         }
//         
//         public static void PopulateResourceCost(ProductionJob job, DataModel data)
//         {
//             job.Resources.Clear();
//             job.ProductionPoints = 0;
//
//             if (!job.IsRetrofit)
//             {
//                 var shipInfo = data.Ships.First(x => x.BlueprintId == job.ShipId);
//                 var shipCost = shipInfo.CalculateCost(data.Tiers, data.OwnerCostMultipliers);
//                 job.ProductionPoints += Mathf.RoundToInt(60f*shipInfo.CalculateBaseCost());
//                 AddToResource(job.Resources, shipCost);
//             }
//
//             foreach (var t in job.Tactical)
//             {
//                 if (t.IsBuilt) continue;
//
//                 var tInfo = data.TacticalModules.First(x => x.BlueprintId == t.Id);
//                 var tCost = tInfo.CalculateCost(data.Tiers, data.OwnerCostMultipliers);
//                 job.ProductionPoints += Mathf.RoundToInt(60f*tInfo.CalculateBaseCost());
//                 AddToResource(job.Resources,tCost);
//             }
//
//             foreach (var a in job.Active)
//             {
//                 if (a.IsBuilt) continue;
//
//                 var aInfo = data.ActiveModules.First(x => x.BlueprintId == a.Id);
//                 var aCost = aInfo.CalculateCost(data.Tiers, data.OwnerCostMultipliers);
//                 job.ProductionPoints += Mathf.RoundToInt(60f*aInfo.CalculateBaseCost());
//                 AddToResource(job.Resources,aCost);
//             }
//
//             foreach (var p in job.Passive)
//             {
//                 if (p.IsBuilt) continue;
//
//                 var pInfo = data.PassiveModules.First(x => x.BlueprintId == p.Id);
//                 var pCost = pInfo.CalculateCost(data.Tiers, data.OwnerCostMultipliers);
//                 job.ProductionPoints += Mathf.RoundToInt(60f*pInfo.CalculateBaseCost());
//                 AddToResource(job.Resources,pCost);
//             }
//
//             job.ProductionPointsLeft = job.ProductionPoints;
//         }
//     }
// }