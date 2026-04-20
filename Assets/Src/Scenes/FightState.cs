using FairyGUI;
using FrameworkCore.UI;

namespace Scenes
{
    /// <summary>
    /// 战斗中的状态栏支持
    /// </summary>
    public class FightState : FairyUIFormLogic
    {

        /// <summary>
        /// 分数文本显示
        /// </summary>
        public GTextField score;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            score.text = "0";
        }
    }
}