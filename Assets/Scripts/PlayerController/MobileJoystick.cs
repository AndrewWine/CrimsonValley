using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileJoystick : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private RectTransform joystickOutline;
    [SerializeField] private RectTransform joysticKnob;
    public RectTransform JoystickOutline => joystickOutline;
    public RectTransform JoysticKnob => joysticKnob;
    // Start is called before the first frame update

    [Header(" Setting ")]
    [SerializeField] private float moveFactor;
    private Vector3 clickedPosition;
    private Vector3 move;
    private bool canControl;
    private float maxdistanceOfKnob = 0.3f;
    void Start()
    {
        HideJoystick();
    }

    // Update is called once per frame
    void Update()
    {
        if(canControl)
            ControlJoystick();
    }

    public void ClickOnJoystrickZoneCallback()
    {
        clickedPosition = Input.mousePosition;
        joystickOutline.position = clickedPosition;
        joysticKnob.position = joystickOutline.position;
        ShowJoystick();
        canControl = true;
    }    

    private void ShowJoystick()
    {
        joystickOutline.gameObject.SetActive(true);
    }

    private void HideJoystick()
    {
        joystickOutline.gameObject.SetActive(false);
        canControl = false;
        move = Vector3.zero;
    }

    private void ControlJoystick()
    {
        Vector3 currentPosition = Input.mousePosition;
        Vector3 direction = currentPosition - clickedPosition;

        float moveMagnitude = direction.magnitude * moveFactor / Screen.width;

        moveMagnitude = Mathf.Min(moveMagnitude, joystickOutline.rect.width / 2);

        move = direction.normalized * maxdistanceOfKnob * moveMagnitude;

        Vector3 targetPosition = clickedPosition + move;

        joysticKnob.position = targetPosition;

        if(Input.GetMouseButtonUp(0)) 
            HideJoystick();

    }
    
    public Vector3 GetMoveVector()
    {
        return move;
    }
}
