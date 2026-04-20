
using Data;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Displays
{

    /// <summary>
    /// 敌人显示对象
    /// </summary>
    public class Enemy : BaseDisplay
    {

        public int Health = 5;

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            this.SetToLocation(Screen.width * Random.value, Screen.height + 100);
            this.GetComponent<Animation>().Play();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            this.AddVelocity(0, -10 * elapseSeconds);
            if (this.transform.position.y < Utils.System.TOP_BOUNDARY)
            {
                GameEntry.Entity.HideEntity(this.Entity.Id);
            }
        }

        public void Hurt(int damage)
        {
            this.Health -= damage;
            if (this.Health <= 0)
            {
                // 产生一个自身的爆炸效果
                GameEntry.Entity.ShowEntity<HitEffect>("Assets/Images/BoomEffect.prefab", "Stage", new HitEffectData { X = this.transform.position.x, Y = this.transform.position.y, Scale = 1.5f });
                GameEntry.Entity.HideEntity(this.Entity.Id);
                // 发送一个死亡事件
            }
        }
    }
}