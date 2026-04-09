namespace Game
{

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
                }
                else
                {
                    Debug.LogError("Failed to load assets: " + message);
                }
            });
        }
    }

}