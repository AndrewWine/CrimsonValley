using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : PlayerState
{
    [Header(" Elements ")]
    [SerializeField] private MobileJoystick joystick;
    private CharacterController characterController;

    private float fallVelocity = 0f;
    private float gravity = -9.81f;

    void Start()
    {
        characterController = GetComponentInParent<CharacterController>();
    }

    private void ManageMovement()
    {
        Vector3 moveVector = joystick.GetMoveVector();
        moveVector.z = moveVector.y;
        moveVector.y = 0; // Giữ nguyên y để xử lý trọng lực sau

        Vector3 movement = moveVector.normalized * blackboard.speed * Time.fixedDeltaTime;

        // Áp dụng trọng lực
        if (!characterController.isGrounded)
        {
            fallVelocity += gravity * Time.fixedDeltaTime; // Tăng tốc độ rơi
        }
        else
        {
            fallVelocity = -2f; // Reset nhẹ để giữ nhân vật dính nền
        }

        movement.y = fallVelocity * Time.fixedDeltaTime; // Cập nhật Y

        // Di chuyển nhân vật
        characterController.Move(movement);

        // Cập nhật animation
        ManageAnimations(movement);
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void ManageAnimations(Vector3 moveVector)
    {
        if (moveVector.magnitude > 0.1f)
        {
            blackboard.animator.SetFloat("moveSpeed", moveVector.magnitude * blackboard.speed);

            // Đảm bảo nhân vật chỉ xoay theo trục Y (không bị nghiêng)
            Vector3 direction = new Vector3(moveVector.x, 0, moveVector.z).normalized;
            blackboard.animator.transform.rotation = Quaternion.LookRotation(direction);

            // Chạy animation tương ứng
            if (moveVector.magnitude * blackboard.speed >= blackboard.runThreshold)
            {
                blackboard.speed = 5f;
                blackboard.animator.Play("Run");
            }
            else
            {
                blackboard.speed = 3f;
                blackboard.animator.Play("Walking");
            }
        }
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
        ManageMovement();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Nếu người chơi không điều khiển joystick => chuyển sang trạng thái Idle
        if (joystick.GetMoveVector().magnitude <= 0.1f)
        {
            stateMachine.ChangeState(blackboard.idlePlayer);
        }
    }
}
