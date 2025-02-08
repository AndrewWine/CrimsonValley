using System;
using UnityEngine;

public class CheckCropFieldState : MonoBehaviour
{
    [Header("Actions")]
    public static Action<bool> EnableSowBTTN;
    public static Action<bool> EnableWaterBTTN;
    public static Action<bool> EnableHarvestBTTN;
    public static Action<bool> UnlockCropField;


    [Header("Elements")]
    private CropField currentCropField; // Chỉ lưu một CropField duy nhất
    private LineRenderer lineRenderer;
    
    private void Start()
    {
        // Tạo LineRenderer nếu chưa có
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        SetupLineRenderer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Croptile"))
        {
            CropField cropField = other.GetComponent<CropField>();
            if (cropField != null)
            {
                if (currentCropField == null)
                {
                    currentCropField = cropField;
                    UnlockCropField?.Invoke(true);
                }

                UpdateButtons(cropField.state, true);
                DrawOutline(cropField); // Vẽ outline khi bật UIButton
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Croptile"))
        {
            CropField cropField = other.GetComponent<CropField>();
            if (cropField == currentCropField)
            {
                currentCropField = null;
                UnlockCropField?.Invoke(false);
                UpdateButtons(TileFieldState.Empty, false);

                lineRenderer.enabled = false; // Tắt outline khi rời đi
            }
        }
    }

    private void UpdateButtons(TileFieldState state, bool enable)
    {
        EnableSowBTTN?.Invoke(state == TileFieldState.Empty && enable);
        EnableWaterBTTN?.Invoke(state == TileFieldState.Sown && enable);
        EnableHarvestBTTN?.Invoke(state == TileFieldState.Ripened && enable);

        lineRenderer.enabled = enable; // Bật LineRenderer khi có UI Button bật
    }

    private void SetupLineRenderer()
    {
        lineRenderer.positionCount = 8; // 4 góc + 4 góc nối nhau
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.loop = true;

        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.material.color = Color.green;
        lineRenderer.enabled = false;
    }

    private void DrawOutline(CropField cropField)
    {
        if (cropField == null || lineRenderer == null) return;

        Bounds bounds = cropField.GetComponent<Collider>().bounds;
        Vector3 min = bounds.min;
        Vector3 max = bounds.max;

        Vector3[] corners = new Vector3[8]
        {
            new Vector3(min.x, max.y, min.z),
            new Vector3(max.x, max.y, min.z),
            new Vector3(max.x, max.y, max.z),
            new Vector3(min.x, max.y, max.z),
            new Vector3(min.x, min.y, min.z),
            new Vector3(max.x, min.y, min.z),
            new Vector3(max.x, min.y, max.z),
            new Vector3(min.x, min.y, max.z)
        };

        lineRenderer.positionCount = 5;
        lineRenderer.SetPositions(new Vector3[]
        {
            corners[0], corners[1], corners[2], corners[3], corners[0]
        });

        lineRenderer.enabled = true;
    }

    public CropField GetCurrentCropField()
    {
        return currentCropField;
    }
}
