using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [BoxGroup("MouseSettings"),SerializeField,Range(10,100)]
    private float _sensitivity;
    [MinMaxSlider(-100.0f, 100.0f),BoxGroup("MouseSettings"),SerializeField]
    private Vector2 _angleClamp;
    [BoxGroup("MouseSettings"),SerializeField,Required]
    private Transform _pivot;
    [BoxGroup("MouseSettings"),SerializeField,Required]
    private Transform _cameraFollowPoint;
    [BoxGroup("MouseSettings"),Range(1,10),SerializeField]
    private float _cameraDistance;
    [BoxGroup("MouseSettings"),SerializeField]
    private Vector2 _cameraDistanceRange;
    
    [BoxGroup("Movement Settings")]
    public float moveSpeed = 1;
    [BoxGroup("Movement Settings")]
    public float gravityForce = -9.8f;
    [BoxGroup("Movement Settings")]
    public float jumpForce;
    [BoxGroup("Movement Settings"),SerializeField]
    private float _groundCheckDistance = 1.1f;
    [BoxGroup("Movement Settings"),Tag,SerializeField]
    private string _groundCheckTag;
    
    [SerializeField,BoxGroup("Animations")] 
    private Animator _animator;
    [SerializeField,BoxGroup("Animations")] 
    private Transform _modelTransform;
    [SerializeField,BoxGroup("Animations"),AnimatorParam("_animator")] 
    private string _groundedParam;
    [SerializeField,BoxGroup("Animations"),AnimatorParam("_animator")] 
    private string _velocityParam;
    
    [HideInInspector]
    public bool isGrounded;
    
    private Vector3 _move = Vector3.zero;
    private Vector2 _mouse = Vector2.zero;
    private Transform _transform;
    private Rigidbody _rigidbody;
    void Start()
    {
        _rigidbody =  GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {   
        Vector3 moveDirection = transform.TransformDirection(_move);
        moveDirection *= moveSpeed;
        
        isGrounded = IsGrounded();
        _rigidbody.linearVelocity = new Vector3(moveDirection.x, _rigidbody.linearVelocity.y, moveDirection.z);

        transform.Rotate(new Vector3(0,_mouse.x * _sensitivity * Time.deltaTime,0));
        
        if(_rigidbody.linearVelocity.x + _rigidbody.linearVelocity.z == 0)
        {
            _modelTransform.Rotate(new Vector3(0, -_mouse.x * _sensitivity * Time.deltaTime, 0));
        }
        else
        {
            Vector2 vector2 = new Vector2(-_rigidbody.linearVelocity.x,-_rigidbody.linearVelocity.z);
            float angle;
            if (vector2.x < 0)
            {
                angle = 360 - (Mathf.Atan2(vector2.x, vector2.y) * Mathf.Rad2Deg * -1);
            }
            else
            {
                angle =  Mathf.Atan2(vector2.x, vector2.y) * Mathf.Rad2Deg;
            }
            _modelTransform.eulerAngles = new Vector3(0,angle , 0);
        }
        
        _cameraFollowPoint.localPosition =new Vector3(0,0,-_cameraDistance) ;
        
        Vector3 cameraRotation = new Vector3(Mathf.Clamp(-_mouse.y, -30f, 30f), 0, 0) * (_sensitivity * Time.deltaTime );
        cameraRotation = _pivot.eulerAngles + cameraRotation;
        cameraRotation.x = ClampAngle(cameraRotation.x, _angleClamp.x, _angleClamp.y);
        
        _pivot.eulerAngles = cameraRotation;
        
        if(_animator)
        {
            _animator.SetBool(_groundedParam, isGrounded);
            _animator.SetFloat(_velocityParam, _rigidbody.linearVelocity.magnitude);
        }
    }

    private bool IsGrounded()
    {
        Physics.Raycast(transform.position, -Vector3.up, out RaycastHit hit, _groundCheckDistance);
        if(hit.collider) return hit.collider.CompareTag(_groundCheckTag);
        return false;
    }

    void OnMove(InputValue value)
    {
        _move = new Vector3(value.Get<Vector2>().x, 0, value.Get<Vector2>().y);
    }

    private void OnScrollView(InputValue input)
    {
        _cameraDistance += input.Get<float>()/5 * -1;
        _cameraDistance = Mathf.Clamp(_cameraDistance, _cameraDistanceRange.x, _cameraDistanceRange.y);
    }
    
    private void OnJump(InputValue input)
    {
        if (isGrounded)
        {
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    
    private void OnLook(InputValue input)
    {
        _mouse = input.Get<Vector2>();
    }
    
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360 + min);
        return Mathf.Min(angle, max);
    }
}
