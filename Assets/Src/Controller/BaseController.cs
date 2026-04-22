using Displays;
using UnityEngine;

namespace Controller
{
    public class BaseController
    {

        public Hero ParentHero;

        /// <summary>
        /// 是否为主控制器，每个英雄只能有一个主控制器，其他为子控制器
        /// </summary>
        public bool isMainController = false;

        public float NowTime = 0;

        public float FixedDeltaTime = 0.2f;

        private float _fixedDeltaTime = 0;

        public BaseController()
        {
        }

        public virtual void OnUpdate(float elapseSeconds)
        {
            NowTime += elapseSeconds;
            _fixedDeltaTime += elapseSeconds;
            if (_fixedDeltaTime >= FixedDeltaTime)
            {
                _fixedDeltaTime -= FixedDeltaTime;
                OnFixedUpdate(FixedDeltaTime);
            }
        }

        public virtual void OnFixedUpdate(float elapseSeconds)
        {

        }

    }
}