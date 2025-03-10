using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : PlayerState
{

    [Header(" Elements ")]
    [SerializeField] private MobileJoystick joystick;
    private CharacterController characterController;


    void Start()
    {
        characterController = GetComponentInParent<CharacterController>();
    }


    private void ManageMovement()
    {
        Vector3 moveVector = joystick.GetMoveVector();
        moveVector.z = moveVector.y;
        moveVector.y = 0;

        Vector3 movement = moveVector.normalized * blackboard.speed * Time.fixedDeltaTime;

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



    public void ManageAnimations(Vector3 moveVector)
    {
        if (moveVector.magnitude > 0)
        {
            blackboard.animator.SetFloat("moveSpeed", moveVector.magnitude * blackboard.speed);
            blackboard.animator.transform.forward = moveVector.normalized;

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
        else
        {
            stateMachine.ChangeState(blackboard.idlePlayer);
        }
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
        ManageMovement();

    }
}
