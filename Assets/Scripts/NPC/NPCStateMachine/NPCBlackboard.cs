using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBlackboard : EntityBlackboard
{
    [Header("Elements")]
    public DayNightCycle dayNightCycle;
    [Header("State")]
    public NPCIdle nPCIdle;
    public NPCMoveState nPCMove;
    public NPCChopState nPCChop;
    public NPCPickAxeState nPCPickAxe;
    public NPCPlantTreeState nPCPlantTree;

}
