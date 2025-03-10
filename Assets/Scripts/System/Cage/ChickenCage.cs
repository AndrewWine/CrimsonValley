using UnityEngine;

public class ChickenCage : Cage
{
    protected override void start()
    {
        feedingTimer = 200;  // Gán giá trị mặc định cho gà
        harvestTimer = 2000;
        base.start();
    }
}
