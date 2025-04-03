using UnityEngine;

public class NPCSleep : NPCState
{
    public override void Enter()
    {
        base.Enter();
        Debug.Log("NPC bắt đầu ngủ, tắt GameObject...");
        gameObject.SetActive(false); // Tắt NPC khi đi ngủ
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("NPC thức dậy, bật GameObject...");
        if (blackboard.dayNightCycle.timeOfDay >= 0.25f)
        {
            gameObject.SetActive(true); // Bật lại NPC khi thức dậy
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }
}
