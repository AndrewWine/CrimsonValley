using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestButtonPressed : MonoBehaviour
{
    private PlayerBlackBoard blackboard;

    private void Start()
    {
        blackboard = GetComponentInParent<PlayerBlackBoard>();
    }
    public void PressSowButton()
    {
        blackboard.harvestButtonPress = true;
    }
}
