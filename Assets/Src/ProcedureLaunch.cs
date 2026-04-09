namespace Game
{
    using Displays;
    using GameFramework.Fsm;
    using GameFramework.Procedure;
    using UnityEngine;
    using UnityGameFramework.Runtime;
    public class ProcedureLaunch : ProcedureBase
    {
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Debug.Log("ProcedureLaunch OnEnter");

            // 开始加载图片
            AssetsManager assetsManager = new AssetsManager();
            assetsManager.LoadFile("Assets/Images/loading.jpeg");
            assetsManager.onProgress((progress) =>
            {
                Debug.Log($"AssetsManager onProgress: {progress}");
            });
            assetsManager.Start((success, message) =>
            {
                if (success)
                {
                    Debug.Log("Assets loaded successfully: " + message);
                    // 新增一张图片到舞台上
                    GameEntry.Entity.ShowEntity<Image>(1, "test", "Stage", assetsManager.GetTexture2D("loading"));
                }
                else
                {
                    Debug.LogError("Failed to load assets: " + message);
                }
            });
        }
    }

}