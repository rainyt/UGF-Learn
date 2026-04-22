using Data;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Displays
{
    /// <summary>
    /// 击中特效显示
    /// </summary>
    public class HitEffect : BaseLogic
    {

        private Animator __animator;

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            var hitEffectData = (HitEffectData)userData;
            // 设置特效位置
            transform.position = new Vector3(hitEffectData.X, hitEffectData.Y, -5);
            transform.localScale = new Vector3(hitEffectData.Scale, hitEffectData.Scale, 1);
            __animator = GetComponent<Animator>();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (__animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                GameEntry.Entity.HideEntity(this.Entity.Id);
            }
        }
    }
}