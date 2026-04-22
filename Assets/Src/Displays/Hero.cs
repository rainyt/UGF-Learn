
using System;
using Data;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Displays
{
    /// <summary>
    /// 飞机英雄实体
    /// </summary>
    public class Hero : BaseDisplay
    {

        Animator _animation;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            _animation = GetComponent<Animator>();
        }

        private float _x = 0;
        private float _y = 0;

        private float _resumeTime = 0;

        public float FireInterval = 0.2f;

        private float _fireTime = 0;

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

        override public void SetToPosition(float x, float y)
        {
            MoveTo(x, y);
            base.SetToPosition(x, y);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            SetToLocation(Screen.width * 0.5f, Screen.height * 0.15f);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            Vector3 current = transform.position;

            float moveXDistance = _x - current.x;
            float moveX = moveXDistance * 0.5f;
            float moveY = (_y - current.y) * 0.5f;

            this.SetToPosition(current.x + moveX, current.y + moveY);

            if (_animation != null)
            {
                if (Math.Abs(moveXDistance) < 0.05f)
                {
                    if (_resumeTime <= 0)
                    {
                        _animation.SetFloat("Blend", 0);
                    }
                    else
                    {
                        _resumeTime -= elapseSeconds;
                    }
                }
                else if (moveXDistance < 0)
                {
                    _resumeTime = 0.15f;
                    _animation.SetFloat("Blend", -1);
                }
                else
                {
                    _resumeTime = 0.15f;
                    _animation.SetFloat("Blend", 1);
                }
                // Debug.Log("Blend: " + _animation.GetFloat("Blend"));
            }

            /// 间隔发射子弹
            _fireTime += elapseSeconds;
            if (_fireTime >= FireInterval)
            {
                _fireTime = 0;
                FireBullet();
            }
        }

        /// <summary>
        /// 发射子弹。
        /// </summary>
        public void FireBullet()
        {
            GameEntry.Entity.ShowEntity<BaseBullet>("Assets/Images/Bullet.prefab", "Bullets", new BulletData { ParentHero = this, Id = 1002 });
        }
    }
}
