using HarmonyLib;

namespace TBCTE_AP.Patches;

// Patcher for NPC quests
[HarmonyPatch(typeof(QuestNPCInteraction), nameof(QuestNPCInteraction.Interact))]
class QuestInteractPatch
{
    // Log the quest step's requirements
    static bool Prefix(QuestNPCInteraction __instance)
    {
        if (__instance.name == "Tots NPC Variant")
        {
            // Make sure Tots doesn't take away the Soil Sword, potentially leaving the player
            // without a weapon.
            var steps = __instance.steps;
            if (steps.Length > 0)
            {
                steps[0].TakeItems = false;
            }
        }

        // Proceed normally
        return true;
    }
}