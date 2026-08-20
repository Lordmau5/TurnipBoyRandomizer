using BepInEx5ArchipelagoPluginTemplate.templates;
using BepInEx5ArchipelagoPluginTemplate.templates.Utils;
using HarmonyLib;

// Patches the method called when the player uses the mailbox
[HarmonyPatch(typeof(MailboxController), nameof(MailboxController.Use))]
class MailboxPatch
{
    // Log requirements for available mail
    static bool Prefix(MailboxController __instance)
    {
        if (Plugin.EnableRandomization)
        {
            var mail = __instance.CheckForMail();
            if (mail != null)
            {
                ItemObject itemObject = mail.ItemObject;
                if (itemObject != null && itemObject.CanSpawn())
                {
                    string itemId = itemObject.Index;
                    ArchipelagoConsole.LogMessage("Mailbox opened! " + itemObject.GetName() + " " + itemObject.Index);
                    if (Plugin.ItemIdToLocation.ContainsKey(itemId))
                    {
                        // Report the location as collected
                        ArchipelagoConsole.LogMessage("Reporting collection of " + Plugin.ItemIdToLocation[itemId]);
                        Plugin.ArchipelagoClient.CollectFrom(Plugin.ItemIdToLocation[itemId]);

                        // Set the item's "picked up" flag to true
                        itemObject.Pickup();

                        __instance.RefreshMailbox();

                        // Skip the default behavior
                        return false;
                    }

                    // Not a location tracked by Archipelago - proceed normally
                    return true;
                }
            }
        }

        // Proceed normally
        return true;

    }
}