using UnityEngine;

public class ShadowCulling : MonoBehaviour
{
    public Camera cameraConfig; // Kéo camera chính vào đây trong Inspector

    private Renderer objectRenderer;
    private Collider objectCollider;

    void Start()
    {
        cameraConfig = Camera.main;
        objectRenderer = GetComponent<Renderer>();
        objectCollider = GetComponent<Collider>();

        if (objectRenderer != null)
        {
            objectRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }
    }

    void Update()
    {
        if (objectRenderer == null || objectCollider == null) return;

        if (IsObjectInView(cameraConfig, objectCollider.bounds))
        {
            objectRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
        else
        {
            objectRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }
    }

    bool IsObjectInView(Camera cam, Bounds bounds)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
        return GeometryUtility.TestPlanesAABB(planes, bounds);
    }
}
