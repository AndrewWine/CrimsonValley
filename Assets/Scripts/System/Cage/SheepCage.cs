using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepCage : Cage
{
    protected override void start()
    {
        feedingTimer = 200;  // Gán giá trị mặc định cho gà
        harvestTimer = 2000;
        base.start();
    }
}
