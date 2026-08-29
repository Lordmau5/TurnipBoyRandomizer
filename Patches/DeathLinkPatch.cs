using HarmonyLib;

namespace TBCTE_AP.Patches;

// Patches the SetCurrentHealth method and potentially sends out a death link event
[HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.SetCurrentHealth))]
class DeathLinkPatch
{
    public static bool wasTriggeredByDeathLink = false;

    [HarmonyPrefix]
    static void Prefix(int _amount)
    {
        if (_amount == 0 && !wasTriggeredByDeathLink)
        {
            Plugin.ArchipelagoClient?.DeathLinkHandler?.SendDeathLink();
        }

        wasTriggeredByDeathLink = false;
    }
}