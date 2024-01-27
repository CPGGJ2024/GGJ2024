using UnityEngine;

public class CapsuleJoint : MonoBehaviour
{
    private HingeJoint2D hingeJoint;
    public int motorSpeed = 400;

    void Start()
    {
        // Add HingeJoint2D component to the current GameObject
        hingeJoint = gameObject.GetComponent<HingeJoint2D>();

        if (hingeJoint == null)
        {
            // Display a warning if the HingeJoint2D component is not found
            Debug.LogWarning("HingeJoint2D component not found on GameObject: " + gameObject.name);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FlexLimb(motorSpeed);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            FlexLimb(-motorSpeed);
        }
    }

    void FlexLimb(int newSpeed)
    {
        JointMotor2D newMotor = hingeJoint.motor;
   
        newMotor.motorSpeed = newSpeed;

        // Apply the modified motor to the HingeJoint2D component
        hingeJoint.motor = newMotor;
    }
}
