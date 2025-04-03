using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepCage : Cage
{
    public override void HandleFeeding(int nutrition)
    {
        base.HandleFeeding(nutrition);
        AudioManager.instance.PlaySFX(5, CagePos);

    }

    public override void HarvestItem()
    {
        base.HarvestItem();
        AudioManager.instance.PlaySFX(5, CagePos);

    }

    public override void ResetHarvestTimer()
    {
        base.ResetHarvestTimer();
        AudioManager.instance.PlaySFX(5, CagePos);

    }
}
