using HarmonyLib;
using TBCTE_AP.Utils;

namespace TBCTE_AP.Patches;

// Patches the method called when the player opens a container
[HarmonyPatch(typeof(ContainerController), nameof(ContainerController.Open))]
class ContainerPatch
{
    static bool Prefix(ContainerController __instance)
    {
        if (Plugin.EnableRandomization)
        {
            var itemObject = __instance.itemObject;
            if (itemObject != null && itemObject.CanSpawn())
            {
                string itemId = itemObject.Index;
                ArchipelagoConsole.LogMessage("Container opened! " + itemObject.GetName() + " " + itemObject.Index);
                if (Plugin.ItemIdToLocation.ContainsKey(itemId))
                {
                    // Play the sound effect
                    __instance.openAudio.Play();

                    // Report the location as collected
                    ArchipelagoConsole.LogMessage("Reporting collection of " + Plugin.ItemIdToLocation[itemId]);
                    Plugin.ArchipelagoClient.CollectFrom(Plugin.ItemIdToLocation[itemId]);

                    var interactionController = __instance.interactionController;
                    if (interactionController != null)
                    {
                        UnityEngine.Object.Destroy(interactionController.gameObject);
                    }

                    // Skip the default behavior
                    return false;
                }
            }

            // Not a location tracked by Archipelago - proceed normally
            return true;
        }

        // Proceed normally
        return true;
    }
}