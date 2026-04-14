using UnityEngine;

namespace Displays
{
    public class Background : Image
    {
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = -10;
        }
    }
}
