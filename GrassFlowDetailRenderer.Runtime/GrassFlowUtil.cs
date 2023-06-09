using System.Linq;
using GrassFlow;
using UnityEngine;

public static class GrassFlowUtil
{
  public static GrassFlowRenderer GrassFlowRenderer =>
    Object.FindObjectOfType<GrassFlowRenderer>();

  public static GrassMesh FindGrassMesh(Terrain terrain)
  {
    return GrassFlowRenderer
      .terrainMeshes
      .FirstOrDefault(gfrTerrainMesh =>
        gfrTerrainMesh.terrainObject == terrain);
  }

  public static bool TrySetColorMapForTerrain(Terrain terrain, Texture2D colorMap)
  {
    var gMesh = FindGrassMesh(terrain);
    if (!gMesh) return false;
    gMesh.colorMap = colorMap;
    return true;
  }

  public static bool TrySetParamMapForTerrain(Terrain terrain, Texture2D colorMap)
  {
    var gMesh = FindGrassMesh(terrain);
    if (!gMesh) return false;
    gMesh.paramMap = colorMap;
    return true;
  }
}
