using HarmonyLib;
using TBCTE_AP.Utils;

namespace TBCTE_AP.Patches;

// Do some logging on item pickup
[HarmonyPatch(typeof(PlayerController), nameof(PlayerController.PickupItem))]
class PickupPatch
{
    static bool Prefix(ItemObject _itemObject, bool _effect)
    {
        ArchipelagoConsole.LogMessage("Item Pickup! " + _itemObject.GetName() + " " + _itemObject.Index);

        // Proceed normally
        return true;
    }
}