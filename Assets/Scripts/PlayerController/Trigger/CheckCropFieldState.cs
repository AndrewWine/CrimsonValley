using System;

using UnityEngine;

public class CheckGameObject : MonoBehaviour
{
    [Header("Actions")]
    public static Action<bool> EnableSowBTTN;
    public static Action<bool> EnableWaterBTTN;
    public static Action<bool> EnableHarvestBTTN;
    public static Action<bool> EnableSleepBTTN;
    public static Action<bool> UnlockCropField;
    public static Action<CropField> cropFieldDetected;
    public static Action<bool> changeCameraAngel;

    [Header("Elements")]
    private CropField currentCropField; // Chỉ lưu một CropField duy nhất
    private OreRock ore;
    private LineRenderer lineRenderer;
    private PlayerBlackBoard blackBoard;
    public bool buildingGameObject;
    public bool cropTile;

    public GameObject currentGameObject;
    private void Start()
    {
        // Tạo LineRenderer nếu chưa có
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        blackBoard = GetComponentInParent<PlayerBlackBoard>();
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
                    currentGameObject = currentCropField.gameObject;
                    cropTile = true;

                    //UnlockCropField?.Invoke(true);
                }

                UpdateButtons(cropField.state, true);
                DrawOutline(cropField); // Vẽ outline khi bật UIButton
            }
            cropFieldDetected?.Invoke(cropField);//HoeAbility
           
         

        }

        else if(other.CompareTag("Ore"))
        {
            OreRock ore = other.GetComponent<OreRock>();
        }

    

        else if(other.CompareTag("Indoor"))
        {
            changeCameraAngel?.Invoke(true);
            EnableSleepBTTN?.Invoke(true);

        }

        else if( other.CompareTag("Cage") || other.CompareTag("Decore"))
        {
            buildingGameObject = true;
            currentGameObject = other.gameObject;

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
                //UnlockCropField?.Invoke(false);
                UpdateButtons(TileFieldState.Empty, false);


                lineRenderer.enabled = false; // Tắt outline khi rời đi
            }
            cropTile = false;
            currentGameObject = null;
        }
    

        else if (other.CompareTag("Indoor"))
        {
            changeCameraAngel?.Invoke(false);
            EnableSleepBTTN?.Invoke(false);

        }

        else if ( other.CompareTag("Cage") || other.CompareTag("Decore"))
        {
            buildingGameObject = false;
            currentGameObject = null;

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

    public OreRock GetCurrentOreRock()
    {
        return ore;
    }
}
