using MoreCustomizations;
using MoreCustomizations.Data;

namespace BobaHats;

public class MoreCustomizationsCompat
{
    private static bool _loaded = false;
    
    public static void LoadHats()
    {
        if (_loaded) return;
        
        Plugin.Instance?.Logger.LogInfo($"Loading hats for More Customizations compatibility: proof: {MoreCustomizationsPlugin.Singleton}");
        Plugin.Instance?._harmony.PatchAll(typeof(MoreCustomizationsCompat));
        //MoreCustomizationsPlugin.Singleton.LoadAllCustomizations();
        InsertIntoDictionary();
        Plugin.Instance?.Logger.LogInfo("More Customizations compatibility loaded.");

        _loaded = true;
    }

    private static void InsertIntoDictionary()
    {
        Plugin.Instance?.Logger.LogInfo("LoadAllCustomizations patching!");
        var mutable = MoreCustomizationsPlugin.AllCustomizationsData
            .ToDictionary(
                kv => kv.Key,
                kv => kv.Value.ToList()
            );
            
        foreach (var kv in mutable)
            Plugin.Instance?.Logger.LogInfo($"Pre-Insert Customization type {kv.Key} has {kv.Value.Count} customizations.");

        if (Plugin.Instance?.Hats != null)
        {
            foreach (Hat hat in Plugin.Instance.Hats)
            {
                var newHat = ScriptableObject.CreateInstance<CustomHat_V1>();

                newHat.name = hat.Name;
                newHat.Icon = hat.Icon;
                newHat.Prefab = hat.Prefab;
                
                var renderer = hat.Prefab.GetComponentInChildren<Renderer>();
                if (renderer != null)
                {
                    newHat.MainTexture = renderer.sharedMaterial.mainTexture;
                    newHat.SubTexture = renderer.sharedMaterial.mainTexture;
                }
                
                Transform hatTransform = hat.Prefab.transform;
                Vector3 localEuler = hatTransform.localEulerAngles;
                
                localEuler += new Vector3(-90f, 0f, 0f);
                newHat.EulerAngleOffset = localEuler;
                
                mutable[Customization.Type.Hat].Add(newHat);
            }
        }
        
        MoreCustomizationsPlugin.AllCustomizationsData = mutable.ToDictionary(
            kv => kv.Key,
            kv => kv.Value as IReadOnlyList<CustomizationData>
        );
            
        foreach (var kv in MoreCustomizationsPlugin.AllCustomizationsData)
            Plugin.Instance?.Logger.LogInfo($"Post-Insert Customization type {kv.Key} has {kv.Value.Count} customizations.");
    }
}