namespace Displays
{
    using UnityEngine;
    using UnityGameFramework.Runtime;
    public class Image : EntityLogic
    {
        private SpriteRenderer spriteRenderer;

        public void SetTexture(Texture2D texture)
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            }
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            spriteRenderer.sprite = sprite;
        }
    }
}