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
            transform.position = new Vector3(x, y, y);
        }

        /// <summary>
        /// 设置显示位置，基于屏幕坐标。
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        virtual public void SetToLocation(float x, float y)
        {
            Vector3 location = new Vector3(x, y, y);
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
            transform.position = transform.position + new Vector3(x / 100f, y / 100f, y / 100f);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            _fixedDeltaTime += elapseSeconds;
            if (_fixedDeltaTime >= FixedDeltaTime)
            {
                _fixedDeltaTime -= FixedDeltaTime;
                OnFixedUpdate(FixedDeltaTime);
            }
        }

        public float FixedDeltaTime = 1f / 30f;

        private float _fixedDeltaTime = 0f;

        /// <summary>
        /// 固定更新，可以设置FixedDeltaTime的固定频率处理
        /// </summary>
        /// <param name="elapseSeconds"></param>
        virtual public void OnFixedUpdate(float elapseSeconds)
        {

        }

    }
}