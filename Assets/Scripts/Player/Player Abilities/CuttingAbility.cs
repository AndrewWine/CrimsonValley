using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Playables;

public class CuttingAbility : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] public Transform checkGameObject;
    [Header(" Settings ")]
    private bool canCut;
    private Tree targetTree;
    private PlayerBlackBoard blackBoard;

    [Header("Actions")]
    public static System.Action<Transform, float> Cutting;

    void Start()
    {
        blackBoard = GetComponentInParent<PlayerBlackBoard>();
        CutState.notifyCutting += OnCuttingButtonPressed;
    }

    private void OnDestroy()
    {
        CutState.notifyCutting -= OnCuttingButtonPressed;
    }

    public void OnCuttingButtonPressed()
    {
        if (targetTree != null)
        {
            PlayerStatusManager.Instance.UseStamina(1); // Mỗi lần dùng công cụ trừ 10 Stamina
            targetTree.TakeDamage(blackBoard.Axedamage);
        }
        else
        {
            Debug.Log("No tree detected!");
        }
    }

    // Kiểm tra xem có đối tượng nào trong vùng trigger không
    private void OnTriggerStay(Collider other)
    {
        Debug.Log($"Detected: {other.gameObject.name}"); // Kiểm tra va chạm với đối tượng nào

        if (other.CompareTag("Tree"))
        {
            targetTree = other.GetComponent<Tree>();

            if (targetTree != null)
            {
                Debug.Log($"Tree detected: {other.gameObject.name}");
            }
            else
            {
                Debug.LogWarning("TreeChopping component NOT found on Tree!");
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Tree"))
        {
            //Debug.Log($"Exited tree: {other.gameObject.name}");
            targetTree = null;
        }
    }

}
