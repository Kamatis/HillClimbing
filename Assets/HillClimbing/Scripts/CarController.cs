using UnityEngine;
using UnityEngine.EventSystems;

public class CarController : MonoBehaviour
{
    [Header("Config")]
    public float motorPower = 1500f;
    public float brakePower = -15f;
    public float slowSpeed = 0.5f;
    public float maxSpeed = 10f;
    public float currentSpeed;
    public float currentMotorPower;

    public float rotationSpeed = 50f;

    public float groundDistance = 2f;

    public LayerMask groundMask;
    public float brake = 200f;
    

    [Header("Data")]
    public bool isGrounded;
    public bool isRotating;

    [Header("Dependencies")]
    [SerializeField]
    private Rigidbody2D frontWheel;
    [SerializeField]
    private Rigidbody2D backWheel;
    [SerializeField]
    private Rigidbody2D carBody;
    [SerializeField]
    private WheelJoint2D motorWheel;

    private float direction = 0f;
    private JointMotor2D motor;
    private float currentRotation;
    private float lastRotation;

    private void Start()
    {
        motor = motorWheel.motor;
        currentMotorPower = motorPower;
    }

    private void Update()
    {
        //direction = Input.GetAxis("Horizontal");
        CheckGrounded();

        if(isRotating)
        {
            currentRotation += transform.rotation.eulerAngles.z - lastRotation;
            lastRotation = transform.rotation.eulerAngles.z;
        }

        if(!isGrounded)
        {
            isRotating = true;
            currentRotation = 0f;
            lastRotation = transform.rotation.eulerAngles.z;
        } else if(isRotating)
        {
            isRotating = false;
            if (Mathf.Abs(currentRotation) > 8f)
            {
                GameManager.Instance.Wheelie();
            }
        }
    }

    private void FixedUpdate()
    {
        if(currentSpeed > maxSpeed)
        {
            motorPower = 0f;
        } else
        {
            motorPower = currentMotorPower;
        }

        if(direction > 0)
        {
            if(isGrounded)
            {
                motor.motorSpeed = Mathf.Lerp(motor.motorSpeed, -motorPower, Time.fixedDeltaTime * 1.5f);
            }
        }

        if(direction < 0)
        {
            if(currentSpeed < -maxSpeed)
            {
                if(isGrounded)
                {
                    motor.motorSpeed = Mathf.Lerp(motor.motorSpeed, 0f, Time.fixedDeltaTime * 3f);
                }
            } else
            {
                if(isGrounded)
                {
                    motor.motorSpeed = Mathf.Lerp(motor.motorSpeed, motorPower, Time.fixedDeltaTime * 1.5f);
                }
            }
        } else
        {
            if(isGrounded)
            {
                motor.motorSpeed = Mathf.Lerp(motor.motorSpeed, 0f, Time.fixedDeltaTime * slowSpeed);
            }
        }

        motorWheel.motor = motor;

        if(CanRotate())
        {
            Rotate();
        }
    }

    private void LateUpdate()
    {
        currentSpeed = carBody.velocity.magnitude;
        if(direction < 0f)
        {
            currentSpeed = -currentSpeed;
        }
    }

    private void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 1000f, groundMask);
        float distance = Mathf.Abs(hit.point.y - transform.position.y);

        isGrounded = distance < groundDistance;
    }

    private void Rotate()
    {
        carBody.AddTorque(rotationSpeed * direction);
    }

    private bool CanRotate()
    {
        return !isGrounded;
    }

    public void Accelerate(BaseEventData e)
    {
        direction = 1f;
    }

    public void Brake(BaseEventData e)
    {
        direction = -1f;
    }

    public void Release(BaseEventData e)
    {
        direction = 0f;
    }
}
