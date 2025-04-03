using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningAbility : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] public Transform checkGameObject;
    [Header(" Settings ")]
    private bool canCut;
    private OreRock targetOreRock;
    private PlayerBlackBoard blackBoard;

    private void OnEnable()
    {
        MiningState.NotifyMining += OnMiningButtonPressed;
    }

    private void OnDisable()
    {
        MiningState.NotifyMining -= OnMiningButtonPressed;
    }
    void Start()
    {
        blackBoard = GetComponentInParent<PlayerBlackBoard>();
    }


    public void OnMiningButtonPressed()
    {
        if (targetOreRock != null)
        {

            targetOreRock.TakeDamage(blackBoard.Pickaxedamage);

        }
        else
        {
            return;
        }
    }

    // Kiểm tra xem có đối tượng nào trong vùng trigger không
    private void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Ore"))
        {
            targetOreRock = other.GetComponent<OreRock>();

            if (targetOreRock != null)
            {
            }
            else
            {
                return;
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Ore"))
        {
            Debug.Log($"Exited Ore: {other.gameObject.name}");
            targetOreRock = null;
        }
    }
}
