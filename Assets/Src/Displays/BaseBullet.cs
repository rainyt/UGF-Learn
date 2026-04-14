using Data;

namespace Displays
{
    /// <summary>
    /// 基础子弹显示。
    /// </summary>
    public class BaseBullet : BaseDisplay
    {

        /// <summary>
        /// 子弹数据。
        /// </summary>
        public BulletData Data;

        /// <summary>
        /// 子弹速度。每秒移动的距离。
        /// </summary>
        public float Speed = 360;

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            this.Data = (BulletData)userData;
            this.transform.position = Data.ParentHero.transform.position;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            AddVelocity(0, Speed * elapseSeconds);
        }
    }
}