using UnityEngine;

public class WaterFlow : MonoBehaviour
{
    public Renderer waterRenderer;
    public float scrollSpeed = 0.5f;

    void Update()
    {
        float offset = Time.time * scrollSpeed;
        waterRenderer.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
