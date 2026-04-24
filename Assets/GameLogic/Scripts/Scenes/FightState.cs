using Events;
using FairyGUI;
using FrameworkCore.UI;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace Scenes
{
    /// <summary>
    /// 战斗中的状态栏支持
    /// </summary>
    public class FightState : FairyUIFormLogic
    {

        public int scoreValue = 0;

        /// <summary>
        /// 分数文本显示
        /// </summary>
        public GTextField score;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            score.text = "0";
            GameEntry.Event.Subscribe(GameUIEvent.KILL_ENEMY, OnKillEnemy);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Unsubscribe(GameUIEvent.KILL_ENEMY, OnKillEnemy);
        }

        private void OnKillEnemy(object sender, GameEventArgs args)
        {
            scoreValue++;
            score.text = scoreValue.ToString();
        }

    }
}