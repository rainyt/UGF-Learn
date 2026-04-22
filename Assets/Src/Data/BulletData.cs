using Displays;

namespace Data
{
    /// <summary>
    /// 子弹数据。
    /// </summary>
    public struct BulletData
    {
        /// <summary>
        /// 子弹ID，可通过数据表中读取子弹的信息。
        /// </summary>
        public int Id;

        /// <summary>
        /// 父英雄。
        /// </summary>
        public Hero ParentHero;


        /// <summary>
        /// 子弹发射角度
        /// </summary>
        public float SendRotation;
    }
}