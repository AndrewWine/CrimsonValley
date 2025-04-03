using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraJoystickController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header(" Elements ")]
    [SerializeField] private RectTransform joystickOutline;
    [SerializeField] private RectTransform joystickKnob;
    [SerializeField] private CinemachineVirtualCamera virtualCamera; // Camera Cinemachine

    [Header(" Settings ")]
    [SerializeField] private float moveFactor = 1f;
    private Vector2 clickedPosition;
    private Vector2 move;
    private bool canControl;
    private float maxDistanceOfKnob = 100f; // Giới hạn di chuyển của joystickKnob
    [SerializeField] private float cameraRotationSpeed = 2f; // Tốc độ xoay camera

    private Transform cameraTransform; // Transform của camera
    private float currentYaw;   // Xoay ngang (trục Y)
    private float currentPitch; // Xoay dọc (trục X)
    [SerializeField] private float minPitch = -30f; // Giới hạn góc thấp nhất
    [SerializeField] private float maxPitch = 60f;  // Giới hạn góc cao nhất

    void Start()
    {
        HideJoystick();

        if (virtualCamera != null)
        {
            cameraTransform = virtualCamera.transform;
            currentYaw = cameraTransform.eulerAngles.y;
            currentPitch = cameraTransform.eulerAngles.x;
        }
        else
        {
            Debug.LogError("⚠️ Chưa gán CinemachineVirtualCamera vào CameraJoystickController!");
        }
    }

    void Update()
    {
        if (canControl)
        {
            RotateCamera();
        }
    }

    private void RotateCamera()
    {
        if (cameraTransform == null || move == Vector2.zero) return;

        // Xoay camera theo trục Y (yaw - quay trái/phải)
        currentYaw += move.x * cameraRotationSpeed * Time.deltaTime;

        // Xoay camera theo trục X (pitch - nhìn lên/xuống)
        currentPitch -= move.y * cameraRotationSpeed * Time.deltaTime;
        currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch); // Giới hạn góc

        // Cập nhật góc quay
        cameraTransform.rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        clickedPosition = eventData.position;
        joystickOutline.position = clickedPosition;
        joystickKnob.position = clickedPosition;
        ShowJoystick();
        canControl = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canControl) return;

        Vector2 currentPosition = eventData.position;
        Vector2 direction = currentPosition - clickedPosition;

        if (direction.magnitude > maxDistanceOfKnob)
        {
            direction = direction.normalized * maxDistanceOfKnob;
        }

        joystickKnob.position = clickedPosition + direction;
        move = (direction / maxDistanceOfKnob) * moveFactor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        HideJoystick();
    }

    private void ShowJoystick()
    {
        joystickOutline.gameObject.SetActive(true);
    }

    private void HideJoystick()
    {
        joystickOutline.gameObject.SetActive(false);
        canControl = false;
        move = Vector2.zero;
    }

    public Vector2 GetMoveVector()
    {
        return move;
    }

    public void ClickOnJoystrickZoneCallback()
    {
        clickedPosition = Input.mousePosition;
        joystickOutline.position = clickedPosition;
        joystickKnob.position = joystickOutline.position;
        ShowJoystick();
        canControl = true;
    }
}




