using System;
using Procedural;
using Procedural.SplatRenderer;
using Procedural.Terrain;
using Unity.VisualScripting.YamlDotNet.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GrassFlowDetailRenderer.GrassFlowDetailRenderer.Editor.Procedural
{
  [CustomEditor(typeof(ProceduralDetailMapSettings))]
  public class ProceduralDetailMapSettingsInspector : UnityEditor.Editor
  {
    public static ProceduralDetailMapSettings CreateAndSaveSettings()
    {
      var settings = CreateInstance<ProceduralDetailMapSettings>();
      settings.name = "ProceduralDetailMapSettings";

      var assetPath = "Assets/";
      assetPath = EditorUtility.SaveFolderPanel("Asset destination folder", assetPath, "");
      if (string.IsNullOrEmpty(assetPath))
      {
        throw new ArgumentException("Please pick destination to save the asset");
      }

      assetPath = assetPath.Replace(Application.dataPath, "Assets");
      assetPath += "/" + settings.name + ".asset";

      AssetDatabase.CreateAsset(settings, assetPath);

      settings =
        (ProceduralDetailMapSettings) AssetDatabase.LoadAssetAtPath(assetPath,
          typeof(ProceduralDetailMapSettings));

      return settings;
    }

    public static ProceduralDetailMapSettings SaveToAsset(ProceduralDetailMapSettings settings)
    {
      var assetPath = "Assets/";
      assetPath = EditorUtility.SaveFolderPanel("Asset destination folder", assetPath, "");
      if (assetPath == string.Empty) return settings;

      assetPath = assetPath.Replace(Application.dataPath, "Assets");
      assetPath += "/" + settings.name + ".asset";
      Debug.Log("Saved to <i>" + assetPath + "</i>");

      AssetDatabase.CreateAsset(settings, assetPath);

      settings =
        (ProceduralDetailMapSettings) AssetDatabase.LoadAssetAtPath(assetPath,
          typeof(ProceduralDetailMapSettings));

      return settings;
    }

    public static void Render(TerrainGrassMapRenderer script)
    {
      var settings = script.settings;

      if (settings.terrainTextures.Count == 0)
        // Create 16 empty textures
        foreach (var terrain in script.terrainObjects)
        {
          var texture = new Texture2D((int) settings.resolution, (int) settings.resolution,
            TextureFormat.RGBA32, false)
          {
            name = terrain.name
          };
          AssetDatabase.AddObjectToAsset(texture, settings);
          settings.terrainTextures.Add(texture);
        }

      for (var i = 0; i < script.terrainObjects.Count; i++)
      {
        var terrain = script.terrainObjects[i];
        var renderer = new SplatRenderer
        {
          terrain = terrain,
          settings = settings
        };
        var texture = renderer.Render();
        texture.name = terrain.name;

        EditorUtility.CopySerialized(texture, settings.terrainTextures[i]);
        DestroyImmediate(texture);

        GrassFlowUtil.TrySetParamMapForTerrain(terrain, settings.terrainTextures[i]);
      }

      AssetDatabase.SaveAssets();
      EditorUtility.SetDirty(GrassFlowUtil.GrassFlowRenderer);

      GrassFlowUtil.GrassFlowRenderer.Refresh();
    }
  }
}
