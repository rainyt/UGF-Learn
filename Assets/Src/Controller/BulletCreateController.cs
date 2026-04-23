using Data;
using Displays;
using Game;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityGameFramework.Runtime;

namespace Controller
{
    /// <summary>
    /// 创建子弹控制器
    /// </summary>
    public class BulletCreateController : BaseController
    {


        /// <summary>
        /// 创建子弹的ID
        /// </summary>
        public int CreateBulletId = 0;

        public BulletCreateController(int CreateBulletId)
        {
            this.CreateBulletId = CreateBulletId;
            var bulletData = GameEntry.Luban.Tables.Tbbullets.Get(CreateBulletId);
            this.FixedDeltaTime = bulletData.Interval;
        }

        public override void OnFixedUpdate(float elapseSeconds)
        {
            base.OnFixedUpdate(elapseSeconds);
            OnCreateBullet(-90);
        }

        /// <summary>
        /// 创建子弹
        /// </summary>
        virtual public void OnCreateBullet(float sendRotation)
        {
            GameEntry.Entity.ShowEntity<BaseBullet>("Assets/Images/Bullet.prefab", "Bullets", new BulletData { ParentHero = ParentHero, Id = CreateBulletId, SendRotation = sendRotation });
        }
    }


}