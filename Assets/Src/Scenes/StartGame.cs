using Data;
using Events;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Scenes
{

    public class StartGame : FairyUIFormLogic
    {
        [SerializeField] private Button btn_start = null;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            Debug.Log("StartGame OnOpen:" + btn_start);

            if (btn_start != null)
            {
                Debug.Log("AddListener OnClickStartGame");
                btn_start.onClick.AddListener(() =>
                {
                    Debug.Log("StartGame OnClickStartGame");
                    GameEntry.Event.Fire(this, UIEvent.Create(UIEvent.START_GAME));
                });
            }
            else
            {
                Debug.LogError("StartGame btn_start is null");
            }
        }
    }
}