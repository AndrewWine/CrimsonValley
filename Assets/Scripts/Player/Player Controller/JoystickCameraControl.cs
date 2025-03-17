using UnityEngine;

public class JoystickCameraControl : MonoBehaviour
{
    [Header("Joystick Settings")]
    [SerializeField] private RectTransform joystickOutline; // Bao ngoài joystick
    [SerializeField] private RectTransform joysticKnob;     // Nút điều khiển của joystick
    public RectTransform JoystickOutline => joystickOutline;
    public RectTransform JoysticKnob => joysticKnob;

    [Header("Joystick Movement Settings")]
    [SerializeField] private float moveFactor = 1f;         // Hệ số điều chỉnh độ di chuyển của joystick
    private Vector3 clickedPosition;
    private Vector3 move;
    private bool canControl;
    private float maxdistanceOfKnob = 0.3f;

    [Header("Camera Rotation Settings")]
    public float rotationSpeed = 2f;  // Tốc độ quay của camera
    [SerializeField] private Cinemachine.CinemachineVirtualCamera virtualCamera; // Cinemachine Virtual Camera
    [SerializeField] private Transform player; // Đối tượng nhân vật (Player)
    private Transform cameraTransform;

    void Start()
    {
        if (virtualCamera != null)
        {
            cameraTransform = virtualCamera.transform; // Lấy transform của camera
        }

        HideJoystick();
    }

    void Update()
    {
        if (canControl)
        {
            ControlJoystick();
        }

        // Điều khiển camera quay
        if (cameraTransform != null)
        {
            Vector3 moveInput = GetMoveVector();
            if (moveInput.magnitude > 0.1f)
            {
                RotateCamera(moveInput);
                RotatePlayer(moveInput); // Xoay nhân vật theo hướng camera
            }
        }
    }

    public void ClickOnJoystrickZoneCallback()
    {
        // Sử dụng touch hoặc chuột
        if (Input.touchCount > 0)
        {
            clickedPosition = Input.GetTouch(0).position; // Chạm đầu tiên
        }
        else
        {
            clickedPosition = Input.mousePosition; // Chuột
        }

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
        Vector3 currentPosition;

        // Kiểm tra có chạm hay không
        if (Input.touchCount > 0)
        {
            currentPosition = Input.GetTouch(0).position; // Lấy vị trí của chạm đầu tiên
        }
        else
        {
            currentPosition = Input.mousePosition; // Sử dụng chuột nếu không có chạm
        }

        // Tính toán vector di chuyển
        Vector3 direction = currentPosition - clickedPosition;

        float moveMagnitude = direction.magnitude * moveFactor / Screen.width;
        moveMagnitude = Mathf.Min(moveMagnitude, joystickOutline.rect.width / 2);

        move = direction.normalized * maxdistanceOfKnob * moveMagnitude;

        Vector3 targetPosition = clickedPosition + move;
        joysticKnob.position = targetPosition;

        // Kiểm tra nếu chuột hoặc chạm kết thúc, ẩn joystick
        if (Input.touchCount == 0 && !Input.GetMouseButton(0))
        {
            HideJoystick();
        }
    }


    // Trả về vector di chuyển của joystick
    public Vector3 GetMoveVector()
    {
        return move;
    }

    // Xoay camera theo input từ joystick
    private void RotateCamera(Vector3 moveInput)
    {
        float horizontalRotation = moveInput.x * rotationSpeed * Time.deltaTime;
        float verticalRotation = moveInput.y * rotationSpeed * Time.deltaTime;

        // Điều khiển camera qua transform
        cameraTransform.Rotate(Vector3.up * horizontalRotation, Space.World);  // Xoay theo trục Y (ngang)
        cameraTransform.Rotate(Vector3.left * verticalRotation, Space.Self);   // Xoay theo trục X (dọc)
    }

    // Xoay nhân vật theo input từ joystick
    private void RotatePlayer(Vector3 moveInput)
    {
        // Tính góc quay cho nhân vật dựa trên input từ joystick
        float horizontalRotation = moveInput.x * rotationSpeed * Time.deltaTime;

        if (player != null)
        {
            // Xoay nhân vật theo hướng của camera
            player.Rotate(Vector3.up * horizontalRotation, Space.World); // Xoay theo trục Y của nhân vật
        }
    }

    public void ClickOnCameraJoystrickZoneCallback()
    {
        clickedPosition = Input.mousePosition;
        joystickOutline.position = clickedPosition;
        joysticKnob.position = joystickOutline.position;
        ShowJoystick();
        canControl = true;
    }
}
