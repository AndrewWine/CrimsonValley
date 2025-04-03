using UnityEngine;

public class CameraRotationController : MonoBehaviour
{
    public Transform player;
    public Transform cameraTransform;
    public float rotationSpeed = 100f;

    private Vector2 joystickInput;

    private void Update()
    {
        // Lấy input từ joystick điều khiển camera (chỉnh lại theo input của bạn)
        joystickInput.x = Input.GetAxis("RightStickHorizontal");
        joystickInput.y = Input.GetAxis("RightStickVertical");

        if (joystickInput.magnitude > 0.1f)
        {
            float horizontalRotation = joystickInput.x * rotationSpeed * Time.deltaTime;
            player.Rotate(Vector3.up * horizontalRotation);
        }
    }


}
