namespace Game
{
    using Data;
    using Events;
    using FairyGUI;
    using GameFramework.Event;
    using GameFramework.Fsm;
    using GameFramework.Procedure;
    using Scenes;
    using UnityEngine;
    using UnityGameFramework.Runtime;
    using Utils;

    public class ProcedureLaunch : ProcedureBase
    {
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            // 初始化项目的分辨率处理
            FGUI.Init(180, 320);

            AssetsManager.Instance.LoadPackage("DefaultPackage", true);
            AssetsManager.Instance.LoadFairyUI("Assets/FGUI/Package1");
            AssetsManager.Instance.Start((success, message) =>
            {
                if (success)
                {
                    onAssetsLoaded();
                }
                else
                {
                    Debug.LogError(message);
                }

            });
            GameEntry.Event.Subscribe(UIEvent.START_GAME, OnClickStartGame);
            Debug.Log("ProcedureLaunch OnEnter");
        }
        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.UI.CloseAllLoadedUIForms();
            GameEntry.Entity.HideAllLoadedEntities();
            GameEntry.Event.Unsubscribe(UIEvent.START_GAME, OnClickStartGame);
        }

        private bool isStartGame = false;

        private void OnClickStartGame(object sender, GameEventArgs e)
        {
            isStartGame = true;
        }

        private void onAssetsLoaded()
        {
            Debug.Log("ProcedureLaunch onAssetsLoaded");

            // GameEntry.UI.OpenUIForm("Assets/Images/UI/StartGame.prefab", "Stage");
            // GameEntry.Entity.<StartGame>("Assets/Images/UI/StartGame.prefab", "Stage");
            GObject startView = UIPackage.CreateObject("Package1", "StartView");
            GRoot.inst.AddChild(startView);
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (isStartGame)
            {
                Debug.Log("ProcedureLaunch StartGame OnUpdate");
                this.ChangeState<ProcedureGame>(procedureOwner);
            }
        }

        public void OnClickStartGame()
        {
            // procedureOwner.ChangeState<ProcedureMain>();
        }

    }

}