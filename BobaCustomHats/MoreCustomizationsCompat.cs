using MoreCustomizations;

namespace BobaHats;

public class MoreCustomizationsCompat
{
    private static bool _loaded = false;
    
    public static void LoadHats()
    {
        if (_loaded) return;
        
        Plugin.Instance?.Logger.LogInfo($"Loading hats for More Customizations compatibility: proof: {MoreCustomizationsPlugin.Singleton}");
        
        
    }
}