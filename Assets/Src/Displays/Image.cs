namespace Displays
{
    using Unity.VisualScripting;
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
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            spriteRenderer.sprite = sprite;
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            SetTexture(userData as Texture2D);
        }

        protected override void OnRecycle()
        {
            base.OnRecycle();
            spriteRenderer = null;
        }
    }
}