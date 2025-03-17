using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Element")]
    public GameObject playerCamera;
    public GameObject IndoorCamera;

    private void Start()
    {
        playerCamera.gameObject.SetActive(true);
        IndoorCamera.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        CheckCropFieldState.changeCameraAngel += OnChangeAngelOfCamera;
    }

    private void OnDisable()
    {
        CheckCropFieldState.changeCameraAngel -= OnChangeAngelOfCamera;
    }

    private void OnChangeAngelOfCamera(bool check)
    {
        playerCamera.gameObject.SetActive(!check);
        IndoorCamera.gameObject.SetActive(check);
    }



}
