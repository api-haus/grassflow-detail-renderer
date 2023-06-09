using UnityEngine;

namespace Procedural.SplatRenderer.Brushes
{
  internal class BaseBlit
  {
    protected Material material;

    public virtual void Blit(RenderTexture from, RenderTexture to)
    {
      // Blit from->to with material
      Graphics.Blit(from, to, material);
      // Blit buffers to->from for double-buffering
      Graphics.Blit(to, from);
    }
  }
}
