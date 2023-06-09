using UnityEngine;

namespace Procedural.SplatRenderer.Brushes
{
  internal class SetColor : BaseBlit, IBlit
  {
    private static readonly int Color = Shader.PropertyToID("_Color");

    public SetColor(Color color)
    {
      material =
        new(Shader.Find("Hidden/ProceduralTerrainMap/SetColor"));
      material.SetColor(Color, color);
    }
  }
}
