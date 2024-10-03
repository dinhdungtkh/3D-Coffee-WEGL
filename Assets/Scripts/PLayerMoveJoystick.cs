using UnityEngine;
using UnityEngine.AI;

public class PLayerMoveJoystick : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] protected VariableJoystick joyStick;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Rigidbody rb;
    [SerializeField] protected float rotationSpeed = 720f;
    [SerializeField]  private float _moveSpeed;
    public float moveSpeed { get => _moveSpeed; }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    public void PlayerMove()
    {
        float hInput = joyStick.Direction.x;
        float xInput = joyStick.Direction.y;

        if (Mathf.Abs(hInput) > 0.01f || Mathf.Abs(hInput) < 0.01f || Mathf.Abs(xInput) > 0.1f || Mathf.Abs(xInput) < 0.1f)
        {
            rb.velocity = new Vector3(hInput * moveSpeed,0, xInput * moveSpeed);
            _animator.SetBool("isWalking", true);
            if (rb.velocity != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(rb.velocity.normalized *Time.deltaTime);
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
            _animator.SetBool("isWalking", false);
        }
    }
  
}
