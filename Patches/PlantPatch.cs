using BepInEx5ArchipelagoPluginTemplate.templates;
using HarmonyLib;

// Soil patches that normally grow a unique checked item don't check the inventory before spawning
[HarmonyPatch(typeof(PlantController), "Grow")]
class PlantPatch
{
    static void Prefix(PlantController __instance)
    {
        if (Plugin.EnableRandomization)
        {
            var crop = __instance.crop;
            var itemObject = crop.DoNotSpawnIfHaveThisItemObject;
            if (itemObject && Plugin.ItemIdToLocation.ContainsKey(itemObject.Index))
            {
                crop.DoNotSpawnIfHaveThisItemObject = null;
            }
        }
    }
}