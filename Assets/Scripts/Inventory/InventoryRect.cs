using System.Collections;
using UnityEngine;

public class InventoryRect : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] GameObject Inventory;
    [SerializeField] GameObject InventoryRectTransform;
    [SerializeField] GameObject SeedContainer;
    [SerializeField] GameObject ToolContainer;
    [SerializeField] GameObject MaterialContainer;
    [SerializeField] float moveSpeed = 2f;  // Tốc độ di chuyển sang phải
    [SerializeField] float offScreenPosition = 1920f;  // Vị trí ngoài màn hình (dành cho màn hình 1920px, bạn có thể điều chỉnh cho phù hợp với độ phân giải khác)

    private Vector3 originalPosition;  // Vị trí ban đầu của Inventory

    private void Start()
    {
        originalPosition = InventoryRectTransform.transform.localPosition;
        Inventory.SetActive(false);  // Ban đầu ẩn Inventory
        SeedContainer.gameObject.SetActive(true);
        ToolContainer.gameObject.SetActive(false);
        MaterialContainer.gameObject.SetActive(false);
    }

    public void OpenSeedContainer()
    {
        SeedContainer.gameObject.SetActive(true);
        ToolContainer.gameObject.SetActive(false);
        MaterialContainer.gameObject.SetActive(false);

    }

    public void OpenToolContainer()
    {
        SeedContainer.gameObject.SetActive(false);
        ToolContainer.gameObject.SetActive(true);
        MaterialContainer.gameObject.SetActive(false);
    }
    public void OpenMaterialContainer()
    {
        SeedContainer.gameObject.SetActive(false);
        ToolContainer.gameObject.SetActive(false);
        MaterialContainer.gameObject.SetActive(true);
    }


    public void OpenInventory()
    {
        Inventory.SetActive(true);
        InventoryRectTransform.SetActive(true);
        InventoryRectTransform.transform.localPosition = new Vector3(-Screen.width, InventoryRectTransform.transform.localPosition.y, InventoryRectTransform.transform.localPosition.z);  // Đặt Inventory ra ngoài màn hình
        StartCoroutine(MoveInventoryIn());
    }

    public void CloseInventory()
    {
        TooltipManager.Instance.HideTooltip();
        StartCoroutine(MoveInventoryOut());
    }

    // Coroutine di chuyển Inventory vào màn hình
    private IEnumerator MoveInventoryIn()
    {
        Vector3 targetPosition = originalPosition;
        while (Vector3.Distance(InventoryRectTransform.transform.localPosition, targetPosition) > 0.1f)
        {
            InventoryRectTransform.transform.localPosition = Vector3.Lerp(InventoryRectTransform.transform.localPosition, targetPosition, Time.deltaTime * moveSpeed);
            yield return null;
        }
    }

    // Coroutine di chuyển Inventory ra ngoài màn hình
    private IEnumerator MoveInventoryOut()
    {
        Vector3 targetPosition = new Vector3(offScreenPosition, InventoryRectTransform.transform.localPosition.y, InventoryRectTransform.transform.localPosition.z);

        // Di chuyển Inventory sang phải đến hết màn hình
        while (InventoryRectTransform.transform.localPosition.x < offScreenPosition)
        {
            InventoryRectTransform.transform.localPosition = Vector3.MoveTowards(InventoryRectTransform.transform.localPosition, targetPosition, Time.deltaTime * moveSpeed);
            yield return null;
        }

        // Sau khi di chuyển hết, ẩn Inventory
        Inventory.SetActive(false);
        InventoryRectTransform.SetActive(false);
    }
}
