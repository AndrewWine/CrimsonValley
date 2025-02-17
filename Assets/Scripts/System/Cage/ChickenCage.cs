using UnityEngine;

public class ChickenCage : Cage
{
    protected override void OnEnable()
    {
        feedingTimer = 200;  // Gán giá trị mặc định cho gà
        harvestTimer = 2000;
        base.OnEnable();
    }
}
