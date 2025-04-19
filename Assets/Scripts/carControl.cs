using UnityEngine;

public class CarControl : MonoBehaviour
{
    public float motorTorque = 2000;
    public float brakeTorque = 2000;
    public float maxSpeed = 20;
    public float steeringRange = 30;
    public float steeringRangeAtMaxSpeed = 10;
    public float centreOfGravityOffset = -1f;

    public float baseMotorTorque = 2000; // Базовый motorTorque (то, что в редакторе)
    public float baseMaxSpeed = 20;      // Базовая maxSpeed (то, что в редакторе)

    private float motorTorqueModifier = 0f;  // Модификатор motorTorque
    private float maxSpeedModifier = 0f;   // Модификатор maxSpeed

    public UI ui;
    private float totalDistance = 0f;   // Общее пройденное расстояние
    private Vector3 lastPosition;       // Предыдущая позиция игрока

    WheelScript[] wheels;
    Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        // Adjust center of mass vertically, to help prevent the car from rolling
        rigidBody.centerOfMass += Vector3.up * centreOfGravityOffset;

        // Find all child GameObjects that have the WheelControl script attached
        wheels = GetComponentsInChildren<WheelScript>();

        // Инициализируем базовые значения
        baseMotorTorque = motorTorque;
        baseMaxSpeed = maxSpeed;
        lastPosition = transform.position;

        if (ui == null)
        {
            Debug.LogError("UIManager не назначен в CarControl!");
        }
    }
    public void ApplyBuff(float motorTorqueModifier, float maxSpeedModifier, string buffText)
    {
        
        // Применяем модификаторы
        this.motorTorqueModifier += motorTorqueModifier;
        this.maxSpeedModifier += maxSpeedModifier;

        // Обновляем текущие значения
        motorTorque = baseMotorTorque + this.motorTorqueModifier;
        maxSpeed = baseMaxSpeed + this.maxSpeedModifier;

        // Ограничиваем значения (опционально)
        motorTorque = Mathf.Max(motorTorque, 0); // Мотор не может быть отрицательным
        maxSpeed = Mathf.Max(maxSpeed, 1); // Скорость не может быть отрицательной (или можно позволить задний ход
        Debug.Log("Motor Torque: " + motorTorque + ", Max Speed: " + maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        float vInput = Input.GetAxis("Vertical");
        float hInput = Input.GetAxis("Horizontal");

        // Calculate current speed in relation to the forward direction of the car
        // (this returns a negative number when traveling backwards)
        float forwardSpeed = Vector3.Dot(transform.forward, rigidBody.linearVelocity);

        // Calculate how close the car is to top speed
        // as a number from zero to one
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, forwardSpeed);

        // Use that to calculate how much torque is available 
        // (zero torque at top speed)
        float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);

        // �and to calculate how much to steer 
        // (the car steers more gently at top speed)
        float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

        // Check whether the user input is in the same direction 
        // as the car's velocity
        bool isAccelerating = Mathf.Sign(vInput) == Mathf.Sign(forwardSpeed);

        foreach (var wheel in wheels)
        {
            // Apply steering to Wheel colliders that have "Steerable" enabled
            if (wheel.steerable)
            {
                wheel.WheelCollider.steerAngle = hInput * currentSteerRange;
            }

            if (isAccelerating)
            {
                // Apply torque to Wheel colliders that have "Motorized" enabled
                if (wheel.motorized)
                {
                    wheel.WheelCollider.motorTorque = vInput * currentMotorTorque;
                }
                wheel.WheelCollider.brakeTorque = 0;
            }
            else
            {
                // If the user is trying to go in the opposite direction
                // apply brakes to all wheels
                wheel.WheelCollider.brakeTorque = Mathf.Abs(vInput) * brakeTorque;
                wheel.WheelCollider.motorTorque = 0;
            }
        }


        if (rigidBody.linearVelocity.magnitude > (maxSpeed / 3.6f))
        {
            rigidBody.linearVelocity = rigidBody.linearVelocity.normalized * (maxSpeed / 3.6f);
        }

        UiUpdate(forwardSpeed);
        
    }
    private void UiUpdate(float forwardSpeed)
    {
        float distanceThisFrame = Vector3.Distance(transform.position, lastPosition);
        totalDistance += distanceThisFrame;
        lastPosition = transform.position;

        if (ui != null)
        {
            ui.UpdateUI(Mathf.Abs(forwardSpeed) * 3.6f, totalDistance); // Передаем скорость в км/ч
        }
    }
}
