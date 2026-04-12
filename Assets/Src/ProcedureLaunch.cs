namespace Game
{
    using Displays;
    using GameFramework.Fsm;
    using GameFramework.Procedure;
    using UnityEngine;
    using UnityGameFramework.Runtime;
    using Utils;
    public class ProcedureLaunch : ProcedureBase
    {
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Debug.Log($"ProcedureLaunch OnEnter, IL2CPP: {System.IsIL2CPP}");

            // 开始加载图片
            AssetsManager assetsManager = new AssetsManager();
            assetsManager.LoadPackage("DefaultPackage", true);
            assetsManager.LoadFile("Assets/Images/loading.jpeg");
            assetsManager.LoadFile("Assets/Images/logo.png");
            assetsManager.LoadFile("Assets/Displays/BaseImage.prefab");
            assetsManager.OnProgress((progress) =>
            {
                Debug.Log($"AssetsManager OnProgress: {progress}");
            });
            assetsManager.Start((success, message) =>
            {
                if (success)
                {
                    Debug.Log("Assets loaded successfully: " + message);
                    // 新增一张图片到舞台上
                    GameEntry.Entity.ShowEntity<Image>(1, "Assets/Displays/BaseImage.prefab", "Stage", assetsManager.GetTexture2D("loading"));
                    GameEntry.Entity.ShowEntity<Image>(2, "Assets/Displays/BaseImage.prefab", "Stage", assetsManager.GetTexture2D("logo"));
                }
                else
                {
                    Debug.LogError("Failed to load assets: " + message);
                }
            });
        }
    }

}