
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Displays
{
    public class Hero : EntityLogic
    {

        /// <summary>
        /// 移动到指定位置。
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MoveTo(float x, float y)
        {
            transform.position = new Vector3(x, y, 0);
        }
    }
}
