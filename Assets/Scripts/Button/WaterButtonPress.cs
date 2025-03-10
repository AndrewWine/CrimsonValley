using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterButtonPress : MonoBehaviour
{
    private PlayerBlackBoard blackboard;

    private void Start()
    {
        blackboard = GetComponentInParent<PlayerBlackBoard>();
    }
    public void PressWaterButton()
    {
        blackboard.waterButtonPressed = true;
    }
}
