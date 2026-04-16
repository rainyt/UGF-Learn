namespace Game
{
    using GameFramework.Fsm;
    using GameFramework.Procedure;
    using Scenes;
    using UnityEngine;
    using UnityGameFramework.Runtime;

    public class ProcedureLaunch : ProcedureBase
    {
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Debug.Log("ProcedureLaunch OnEnter");
            GameEntry.Entity.ShowEntity<StartGame>("Asets/Images/UI/StartGame.prefab", "Stage");
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            Debug.Log("ProcedureLaunch OnUpdate");
        }

    }

}