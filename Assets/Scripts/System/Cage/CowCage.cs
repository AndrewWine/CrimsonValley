using UnityEngine;

public class CowCage : Cage
{
    public override void HandleFeeding(int nutrition)
    {
        base.HandleFeeding(nutrition);
        AudioManager.instance.PlaySFX(2, CagePos);

    }

    public override void HarvestItem()
    {
        base.HarvestItem();
        AudioManager.instance.PlaySFX(2, CagePos);

    }

    public override void ResetHarvestTimer()
    {
        base.ResetHarvestTimer();
        AudioManager.instance.PlaySFX(2, CagePos);

    }
}