namespace Game
{
    using System.Xml.Serialization;
    using Data;
    using Events;
    using FairyGUI;
    using FrameworkCore;
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
            AssetsManager.Instance.LoadLuBanData("Assets/AllConfigs.bytes");
            AssetsManager.Instance.LoadFile("Assets/Bullets/Bullets.spriteatlasv2");
            AssetsManager.Instance.Start((success, message) =>
            {
                if (success)
                {
                    // 初始化表格测试
                    GameEntry.Luban.Init(AssetsManager.Instance.GetLuBanData("AllConfigs").LoadData);
                    var bulletData = GameEntry.Luban.Tables.Tbbullets.Get(1001);
                    Debug.Log($"子弹数据: {bulletData.Name} {bulletData.Desc}");
                    onAssetsLoaded();
                }
                else
                {
                    Debug.LogError($"ProcedureLaunch OnEnter: {message}");
                }

            });
            GameEntry.Event.Subscribe(GameUIEvent.START_GAME, OnClickStartGame);
            Debug.Log("ProcedureLaunch OnEnter");
        }
        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.UI.CloseAllLoadedUIForms();
            GameEntry.Entity.HideAllLoadedEntities();
            GameEntry.Event.Unsubscribe(GameUIEvent.START_GAME, OnClickStartGame);
        }

        private bool isStartGame = false;

        private void OnClickStartGame(object sender, GameEventArgs e)
        {
            isStartGame = true;
        }

        private void onAssetsLoaded()
        {
            Debug.Log("ProcedureLaunch onAssetsLoaded");

            // GameEntry.UI.OpenUIForm<StartGame>("Assets/FGUI/FairyGUI.prefab", "Stage", new FairyViewData
            // {
            //     packageName = "Assets/FGUI/Package1",
            //     componentName = "StartView",
            // });

            GameEntry.UI.OpenUIForm<StartView>("Assets/FGUI/Package1", "Stage");

            // GameEntry.Entity.<StartGame>("Assets/Images/UI/StartGame.prefab", "Stage");
            // GObject startView = UIPackage.CreateObject("Package1", "StartView");
            // GRoot.inst.AddChild(startView);
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