using System;
using System.Collections.Generic;
using UnityEngine;

namespace Procedural
{
  public class ProceduralDetailMapSettings : ScriptableObject
  {
    public enum EResolution
    {
      R256 = 256,
      R512 = 512,
      R1024 = 1024,
      R2048 = 2048,
      R4096 = 4096
    }

    public List<Texture2D> terrainTextures = new();
    public Texture2D noiseTex;

    public EResolution resolution = EResolution.R1024;
    public GrassFlowParameterMapOptions grassFlowParameterMapOptions = new();

    [Serializable]
    public class TerrainLayerRange
    {
      public bool enabled;
      public int terrainLayerIndex;
      public Vector2 range;
    }

    [Serializable]
    public class GrassFlowParameterMapOptions
    {
      public List<TerrainLayerRange> includeLayers = new();
      public List<TerrainLayerRange> excludeLayers = new();

      // public NoiseOptions densityNoise = new();
      public HeightRule densityHeight = new();

      // public NoiseOptions heightNoise = new();
      public HeightRule heightHeight = new();

      // public NoiseOptions flattedNoise = new();
      public HeightRule flattedHeight = new();

      // public NoiseOptions windStrengthNoise = new();
      public HeightRule windStrengthHeight = new();

      public int seed;
    }

    [Serializable]
    public class HeightRule
    {
      public bool enabled;
      public float minHeight;
      public float maxHeight;
      public Vector2 range;
    }

    // TODO:
    [Serializable]
    public class NoiseOptions
    {
      public enum NoiseType
      {
        BlueNoise
      }

      public NoiseType type = NoiseType.BlueNoise;

      public bool enabled;
      public Vector2 valueRange;
      public float noiseScale;
      public Vector2 noiseOffset;
      public int seed;
    }
  }
}
