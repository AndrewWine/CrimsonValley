using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributeManager : MonoBehaviour
{
    [Header("Element")]
    private PlayerBlackBoard blackboard;

    private void Start()
    {
            blackboard = GetComponent<PlayerBlackBoard>();
    }


}
