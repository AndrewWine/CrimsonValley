using UnityEngine;
using Cinemachine;

public class LODFollowCinemachine : MonoBehaviour
{
    private LODGroup lodGroup;
    private Camera cineCam;

    void Start()
    {
        lodGroup = GetComponent<LODGroup>();

        // Tìm Cinemachine Virtual Camera đang active
        CinemachineBrain brain = FindObjectOfType<CinemachineBrain>();
        if (brain && brain.ActiveVirtualCamera != null)
        {
            cineCam = brain.GetComponent<Camera>(); // Lấy Camera từ CinemachineBrain
        }
    }

    void Update()
    {
        if (lodGroup != null && cineCam != null)
        {
            lodGroup.localReferencePoint = cineCam.transform.position;
        }
    }
}
