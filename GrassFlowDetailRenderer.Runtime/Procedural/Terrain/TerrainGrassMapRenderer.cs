using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Procedural.Terrain
{
  public class TerrainGrassMapRenderer : MonoBehaviour
  {
    public List<UnityEngine.Terrain> terrainObjects = new();
    public ProceduralDetailMapSettings settings;

    private void OnValidate()
    {
      terrainObjects = terrainObjects.Select(x => x).ToList();
      if (terrainObjects.Count == 0) terrainObjects = FindTerrains().ToList();
    }

    private IEnumerable<UnityEngine.Terrain> FindTerrains()
    {
      List<UnityEngine.Terrain> terrains = new()
      {
        transform.GetComponent<UnityEngine.Terrain>(),
        transform.GetComponentInParent<UnityEngine.Terrain>()
      };
      terrains.AddRange(transform.GetComponentsInChildren<UnityEngine.Terrain>());
      return terrains.Where(obj => obj);
    }
  }
}
