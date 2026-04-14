
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Displays
{
    public class Hero : EntityLogic
    {

        private float _x = 0;
        private float _y = 0;

        /// <summary>
        /// 移动到指定位置。
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MoveTo(float x, float y)
        {
            _x = x;
            _y = y;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            Vector3 current = transform.position;
            transform.position = new Vector3(current.x + (_x - current.x) * 0.5f, current.y + (_y - current.y) * 0.5f, 0);
        }
    }
}
