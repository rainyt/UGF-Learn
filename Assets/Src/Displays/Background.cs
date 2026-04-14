using Data;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Displays
{
    public class Background : Image
    {

        public BackgroundData Data;

        private float outScreenY = 0;

        private float resetY = 0;

        protected override void OnShow(object userData)
        {
            Data = (BackgroundData)userData;
            base.OnShow(Data.Texture);
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = -10;
            resetY = spriteRenderer.size.y * 100 - 1;
            outScreenY = -spriteRenderer.size.y;
            this.AddVelocity(0, resetY * Data.Index);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            AddVelocity(0, -30 * elapseSeconds);
            // 地图复位循环滚动
            if (this.transform.position.y < outScreenY)
            {
                this.transform.position = new Vector3(0, 0, 0);
                this.AddVelocity(0, resetY);
            }
        }
    }
}
