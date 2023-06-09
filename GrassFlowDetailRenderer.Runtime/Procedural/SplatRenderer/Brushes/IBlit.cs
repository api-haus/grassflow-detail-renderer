using UnityEngine;

namespace Procedural.SplatRenderer.Brushes
{
  internal interface IBlit
  {
    void Blit(RenderTexture from, RenderTexture to);
  }
}
