
using UnityEngine;

namespace Displays
{

    /// <summary>
    /// 敌人显示对象
    /// </summary>
    public class Enemy : BaseDisplay
    {

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
        }
    }
}