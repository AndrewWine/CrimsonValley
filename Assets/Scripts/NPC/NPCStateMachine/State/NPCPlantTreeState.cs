using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPlantTreeState : NPCState
{
    public override void Enter()
    {
        base.Enter();
        blackboard.animator.Play("PlantTree");
    }
}
