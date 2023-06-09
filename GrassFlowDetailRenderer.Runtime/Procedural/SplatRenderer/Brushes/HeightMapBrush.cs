using UnityEngine;

namespace Procedural.SplatRenderer.Brushes
{
  internal class HeightMapBrush : BaseBlit, IBlit
  {
    private static readonly int HeightMap = Shader.PropertyToID("_HeightMap");
    private static readonly int TargetChannel = Shader.PropertyToID("_TargetChannel");
    private static readonly int HeightValue = Shader.PropertyToID("_HeightValue");
    private static readonly int HeightRule = Shader.PropertyToID("_HeightRule");

    public HeightMapBrush(
      RenderTexture heightMap,
      int targetChannel,
      float heightValue,
      Vector4 heightRule)
    {
      material =
        new(Shader.Find("Hidden/ProceduralTerrainMap/HeightmapBrush"));
      material.SetTexture(HeightMap, heightMap);
      material.SetInt(TargetChannel, targetChannel);
      material.SetFloat(HeightValue, heightValue);
      material.SetVector(HeightRule, heightRule);
    }
  }
}
