using UnityEngine;

namespace Procedural.SplatRenderer.Brushes
{
  internal class SplatChannelCopy : BaseBlit, IBlit
  {
    public const int Include = 0;
    public const int Exclude = 1;

    private static readonly int Range = Shader.PropertyToID("_Range");
    private static readonly int Operation = Shader.PropertyToID("_Operation");
    private static readonly int SplatTexture = Shader.PropertyToID("_SplatTex");
    private static readonly int SourceChannel = Shader.PropertyToID("_SourceChannel");
    private static readonly int TargetChannel = Shader.PropertyToID("_TargetChannel");

    public SplatChannelCopy(Texture2D splatMap, int sourceChannel, int targetChannel,
      int operation, Vector2 range)
    {
      material =
        new(Shader.Find("Hidden/ProceduralTerrainMap/SplatChannelCopy"));
      material.SetTexture(SplatTexture, splatMap);
      material.SetInt(SourceChannel, sourceChannel);
      material.SetInt(TargetChannel, targetChannel);
      material.SetInt(Operation, operation);
      material.SetVector(Range, range);
    }
  }
}
