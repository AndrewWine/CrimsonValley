using UnityEngine;

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

        if (other.CompareTag("Tree"))
        {
            targetTree = other.GetComponent<Tree>();
            blackBoard.isTree = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Tree"))
        {
            //Debug.Log($"Exited tree: {other.gameObject.name}");
            targetTree = null;
            blackBoard.isTree = false;

        }
    }

}
