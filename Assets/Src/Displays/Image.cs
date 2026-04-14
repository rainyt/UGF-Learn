namespace Displays
{
    using Unity.VisualScripting;
    using UnityEngine;
    using UnityGameFramework.Runtime;
    public class Image : BaseDisplay
    {

        public void SetTexture(Texture2D texture)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            spriteRenderer.sprite = sprite;
        }

        protected override void OnShow(object userData)
        {
            Debug.Log("Image OnShow " + userData);
            base.OnShow(userData);
            SetTexture(userData as Texture2D);
        }

        protected override void OnRecycle()
        {
            base.OnRecycle();
        }
    }
}