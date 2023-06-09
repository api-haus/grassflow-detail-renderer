using Procedural.SplatRenderer.Brushes;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Procedural.SplatRenderer
{
  public class SplatRenderer
  {
    private RenderTexture _bufferA;
    private RenderTexture _bufferB;
    public ProceduralDetailMapSettings settings;

    public UnityEngine.Terrain terrain;

    private RenderTexture IntermediateTexture()
    {
      return new RenderTexture((int) settings.resolution, (int) settings.resolution, 0);
    }

    public Texture2D Render()
    {
      _bufferA = IntermediateTexture();
      _bufferB = IntermediateTexture();

      var opts = settings.grassFlowParameterMapOptions;

      new SetColor(
        new Color(1, 1, 1, 1)
      ).Blit(_bufferA, _bufferB);

      new SplatChannelCopy(
        settings.noiseTex,
        (int) GrassParamMapChannels.WindStrength,
        (int) GrassParamMapChannels.WindStrength,
        SplatChannelCopy.Include,
        new Vector2(0, 1)
      ).Blit(_bufferA, _bufferB);
      new SplatChannelCopy(
        settings.noiseTex,
        (int) GrassParamMapChannels.Flattedness,
        (int) GrassParamMapChannels.Flattedness,
        SplatChannelCopy.Include,
        new Vector2(0, 1)
      ).Blit(_bufferA, _bufferB);

      foreach (var includeLayer in opts.includeLayers)
      {
        if (!includeLayer.enabled) continue;
        new SplatChannelCopy(
          SourceForTerrainLayer(includeLayer.terrainLayerIndex),
          includeLayer.terrainLayerIndex % 4,
          (int) GrassParamMapChannels.Density,
          SplatChannelCopy.Include,
          includeLayer.range
        ).Blit(_bufferA, _bufferB);
        new SplatChannelCopy(
          SourceForTerrainLayer(includeLayer.terrainLayerIndex),
          includeLayer.terrainLayerIndex % 4,
          (int) GrassParamMapChannels.Height,
          SplatChannelCopy.Include,
          includeLayer.range
        ).Blit(_bufferA, _bufferB);
        new SplatChannelCopy(
          SourceForTerrainLayer(includeLayer.terrainLayerIndex),
          includeLayer.terrainLayerIndex % 4,
          (int) GrassParamMapChannels.Flattedness,
          SplatChannelCopy.Exclude,
          includeLayer.range
        ).Blit(_bufferA, _bufferB);
      }

      foreach (var excludeLayer in opts.excludeLayers)
      {
        if (!excludeLayer.enabled) continue;
        new SplatChannelCopy(
          SourceForTerrainLayer(excludeLayer.terrainLayerIndex),
          excludeLayer.terrainLayerIndex % 4,
          (int) GrassParamMapChannels.Density,
          SplatChannelCopy.Exclude,
          excludeLayer.range
        ).Blit(_bufferA, _bufferB);
        new SplatChannelCopy(
          SourceForTerrainLayer(excludeLayer.terrainLayerIndex),
          excludeLayer.terrainLayerIndex % 4,
          (int) GrassParamMapChannels.Height,
          SplatChannelCopy.Exclude,
          excludeLayer.range
        ).Blit(_bufferA, _bufferB);
        new SplatChannelCopy(
          SourceForTerrainLayer(excludeLayer.terrainLayerIndex),
          excludeLayer.terrainLayerIndex % 4,
          (int) GrassParamMapChannels.Flattedness,
          SplatChannelCopy.Include,
          excludeLayer.range
        ).Blit(_bufferA, _bufferB);
      }

      ApplyHeightRule(terrain, opts.densityHeight, GrassParamMapChannels.Density);
      ApplyHeightRule(terrain, opts.heightHeight, GrassParamMapChannels.Height);
      ApplyHeightRule(terrain, opts.flattedHeight, GrassParamMapChannels.Flattedness);
      ApplyHeightRule(terrain, opts.windStrengthHeight, GrassParamMapChannels.WindStrength);

      _bufferA.Release();
      return ReadAndReleaseRT(_bufferB);
    }

    private void ApplyHeightRule(
      UnityEngine.Terrain targetTerrain,
      ProceduralDetailMapSettings.HeightRule heightRule,
      GrassParamMapChannels targetChannel)
    {
      if (!heightRule.enabled) return;
      var terrainData = targetTerrain.terrainData;
      new HeightMapBrush(
        terrainData.heightmapTexture,
        (int) targetChannel,
        terrainData.heightmapScale.y,
        new Vector4(
          heightRule.minHeight,
          heightRule.maxHeight,
          heightRule.range.x,
          heightRule.range.y
        )
      ).Blit(_bufferA, _bufferB);
    }

    private static Texture2D ReadAndReleaseRT(RenderTexture rt)
    {
      var target = new Texture2D(rt.width, rt.height, rt.graphicsFormat, TextureCreationFlags.None);
      var oldRt = RenderTexture.active;

      RenderTexture.active = rt;
      target.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
      target.Apply();

      RenderTexture.active = oldRt;
      rt.Release();

      return target;
    }

    private Texture2D SourceForTerrainLayer(int terrainLayer)
    {
      return terrain.terrainData.alphamapTextures[terrainLayer / 4];
    }

    // ReSharper disable once InconsistentNaming
    private enum RGBAChannel
    {
      R = 0,
      G = 1,
      B = 2,
      A = 3
    }

    private enum GrassParamMapChannels
    {
      Density = RGBAChannel.R,
      Height = RGBAChannel.G,
      Flattedness = RGBAChannel.B,
      WindStrength = RGBAChannel.A
    }
  }
}
