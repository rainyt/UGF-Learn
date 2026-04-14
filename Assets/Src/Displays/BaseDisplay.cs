using UnityEngine;
using UnityGameFramework.Runtime;

namespace Displays
{
    public class BaseDisplay : EntityLogic
    {
        /// <summary>
        /// 设置显示位置，基于世界坐标。
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        virtual public void SetToPosition(float x, float y)
        {
            transform.position = new Vector3(x, y, 0);
        }

        /// <summary>
        /// 设置显示位置，基于屏幕坐标。
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        virtual public void SetToLocation(float x, float y)
        {
            Vector3 location = new Vector3(x, y, 0);
            location = Camera.main.ScreenToWorldPoint(location);
            SetToPosition(location.x, location.y);
        }

        /// <summary>
        /// 添加显示速度
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        virtual public void AddVelocity(float x, float y)
        {
            transform.Translate(new Vector3(x / 100f, y / 100f, 0), Space.World);
        }

    }
}