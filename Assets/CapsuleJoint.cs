using System;
using System.Collections;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class CapsuleJoint : MonoBehaviour
{
    private HingeJoint2D limbHingeJoint;
    public int motorSpeed = 400;
    public KeyCode control = KeyCode.Space;
    public LimbSpawner limbSpawner;
    public AudioClip[] boneBreakSounds;

    void Start()
    {
        limbSpawner = GameObject.Find("LimbSpawner").GetComponent<LimbSpawner>();
        // Add HingeJoint2D component to the current GameObject
        limbHingeJoint = gameObject.GetComponent<HingeJoint2D>();

        if (limbHingeJoint == null)
        {
            // Display a warning if the HingeJoint2D component is not found
            Debug.LogWarning("HingeJoint2D component not found on GameObject: " + gameObject.name);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(control) || Input.GetKeyDown(KeyCode.Space))
        {
            FlexLimb(motorSpeed);
        }
        if (Input.GetKeyUp(control) || Input.GetKeyUp(KeyCode.Space))
        {
            StartCoroutine(FlexLimbForDuration(-motorSpeed, 0.5f));
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            SoundManager.instance.PlayRandomSoundFXClip(boneBreakSounds, transform, 1f);
            limbSpawner.Die(transform.parent.gameObject);
            limbHingeJoint.enabled = false;
            Destroy(transform.parent.gameObject, 5f);
        }
    }

    void FlexLimb(int newSpeed)
    {
        JointMotor2D newMotor = limbHingeJoint.motor;
   
        newMotor.motorSpeed = newSpeed;

        // Apply the modified motor to the HingeJoint2D component
        limbHingeJoint.motor = newMotor;
    }

    IEnumerator FlexLimbForDuration(int newSpeed, float duration)
    {
        FlexLimb(newSpeed);
        yield return new WaitForSeconds(duration);
        FlexLimb(0);
    }
}
