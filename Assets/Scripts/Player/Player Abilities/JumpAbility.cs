
using UnityEngine;

public class JumpAbility : MonoBehaviour
{
    private PlayerBlackBoard blackboard;

    private void Start()
    {
        blackboard = GetComponentInParent<PlayerBlackBoard>();
    }

    private void OnEnable()
    {
        AnimationTriggerCrop.onJumping += OnJumpTrigger;
    }

    private void OnDisable()
    {
        AnimationTriggerCrop.onJumping -= OnJumpTrigger;

    }

    private  void OnJumpTrigger()
    {
        // Áp dụng lực nhảy cho Rigidbody
        Rigidbody rb = blackboard.playerTransform.GetComponent<Rigidbody>();
        if (rb != null)
        {

            rb.velocity = new Vector3(rb.velocity.x, blackboard.JumpForce, rb.velocity.z);
            Debug.Log("Jumpingggg");
        }
        else
        {
            Debug.LogError("Rigidbody chưa được gắn cho player!");
        }
    }
}
