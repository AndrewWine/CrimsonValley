using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingAbility : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] public Transform checkGameObject;
    private PlayerAnimator playerAnimator;

    [Header(" Settings ")]
    private bool canCut;
    private Tree targetTree;


    [Header("Actions")]
    public static System.Action<Transform, int> Cutting;

    void Start()
    {
        playerAnimator = GetComponent<PlayerAnimator>();

        ActionButton.Cutting += OnCuttingButtonPressed;
    }

    private void OnDestroy()
    {
        ActionButton.Cutting -= OnCuttingButtonPressed;
    }

    public void OnCuttingButtonPressed()
    {
        if (targetTree != null)
        {
            targetTree.TakeDamage(2);
            Debug.Log("Call Cutting");
            playerAnimator.PlayCuttingAnimation();
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
            Debug.Log($"Exited tree: {other.gameObject.name}");
            targetTree = null;
        }
    }

}
