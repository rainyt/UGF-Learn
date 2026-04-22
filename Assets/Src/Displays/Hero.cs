
using System;
using System.Collections.Generic;
using Controller;
using Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Displays
{
    /// <summary>
    /// 飞机英雄实体
    /// </summary>
    public class Hero : BaseLogic
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

        /// <summary>
        /// 控制器列表
        /// </summary>
        public List<BaseController> Controllers = new List<BaseController>();

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            SetToLocation(Screen.width * 0.5f, Screen.height * 0.15f);
            AddController(new BulletCreateController(1001));
        }

        /// <summary>
        /// 添加控制器
        /// </summary>
        /// <param name="controller"></param>
        public void AddController(BaseController controller)
        {
            controller.ParentHero = this;
            Controllers.Add(controller);
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

            Controllers.ForEach(controller => controller.OnUpdate(elapseSeconds));
        }

    }
}
