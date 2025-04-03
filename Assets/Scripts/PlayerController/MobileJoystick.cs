using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header(" Elements ")]
    [SerializeField] private RectTransform joystickOutline;
    [SerializeField] private RectTransform joystickKnob;
    [SerializeField] private Transform playerTransform; // Nhân vật

    [Header(" Setting ")]
    [SerializeField] private float moveFactor;
    private Vector2 clickedPosition;
    private Vector2 move;
    private bool canControl;
    private float maxDistanceOfKnob = 100f; // Giới hạn di chuyển của joystickKnob
    [SerializeField] private float rotationSpeed; // Tốc độ xoay nhân vật

    void Start()
    {
        HideJoystick();
    }

    void Update()
    {
        if (playerTransform != null && move != Vector2.zero)
        {
            // Xoay nhân vật theo hướng joystick
            float rotationAmount = move.x * rotationSpeed * Time.deltaTime;
            playerTransform.Rotate(0, rotationAmount, 0);
        }
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
