namespace BobaHats;

public class MoreCustomizationsCompatPatch
{
    // TODO: Remove probably
    [HarmonyPatch(typeof(PassportManager), "Awake")]
    [HarmonyPostfix, HarmonyAfter("MoreCustomizations")]
    private static void AfterPassportManagerAwake_Postfix(PassportManager __instance)
    {
        Plugin.Instance.OnLoadHats();
    }
}