using System;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class LimbSpawner : MonoBehaviour
{
    public GameObject limbPrefab; // Prefab of the limb object
    public GameObject body; // Reference to the body GameObject

    private GameObject currentLimb;
    private bool isDragging = false;
    private RaycastHit2D rayHit;
    private List<KeyCode> availableControls = new List<KeyCode> { KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.J, KeyCode.K, KeyCode.L };
    private Dictionary<GameObject, Tuple<FixedJoint2D, KeyCode>> spawnedLimbs = new Dictionary<GameObject, Tuple<FixedJoint2D, KeyCode>>();

    void Update()
    {
        // Spawn a new limb when the mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            if (availableControls.Count > 0)
            {
                SpawnLimb();
            }
        }

        // Drag the limb if it exists and the mouse button is held down
        if (currentLimb != null && isDragging)
        {
            DragLimb();
        }

        // Release the limb when the mouse button is released
        if (Input.GetMouseButtonUp(0) && currentLimb != null)
        {
            ReleaseLimb();
        }

        // Check for the "R" key press to flip the limb prefab
        if (isDragging && Input.GetKeyDown(KeyCode.Mouse1))
        {
            FlipLimbPrefab();
        }
    }

    void SpawnLimb()
    {
        // Instantiate a new limb at the mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Ensure the limb is spawned at the same z-coordinate
        currentLimb = Instantiate(limbPrefab, mousePosition, Quaternion.identity);
        isDragging = true;
    }

    void DragLimb()
    {
        // Update the limb position to follow the mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Ensure the limb is at the same z-coordinate
        currentLimb.transform.position = mousePosition;

        // Raycast from the attachment point to the body's surface
        RaycastHit2D hit = Physics2D.Raycast(currentLimb.transform.Find("Arm").transform.Find("AttachmentPoint").position, body.transform.position- currentLimb.transform.Find("Arm").transform.Find("AttachmentPoint").position);
        // Rotate the limb to align with the surface normal
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        currentLimb.transform.rotation = rotation;
        rayHit = hit;
    }

    void ReleaseLimb()
    {
        // Detach the limb
        isDragging = false;
        FixedJoint2D closestJoint= CreateFixedJoint(rayHit.point, rayHit.collider.gameObject);

        closestJoint.connectedBody = currentLimb.transform.Find("Arm").GetComponent<Rigidbody2D>();
        closestJoint.connectedBody.transform.TransformPoint(closestJoint.connectedAnchor);


        if (closestJoint != null)
        {
            // Move the limb's attachment point to the position of the closest joint
            Transform attachmentPoint = currentLimb.transform.Find("Arm").transform.Find("AttachmentPoint");
            currentLimb.transform.position += closestJoint.connectedBody.transform.TransformPoint(closestJoint.connectedAnchor) - attachmentPoint.position;
            Debug.Log("POINT Y: " + closestJoint.connectedBody.transform.TransformPoint(closestJoint.connectedAnchor).y);
            Debug.Log("POINT Y: " + body.transform.position.y);
            Debug.Log("Attaching");

            // Attach the limb to the closest joint
            closestJoint.connectedBody = currentLimb.transform.Find("Arm").GetComponent<Rigidbody2D>();
        }

        // Activate Rigidbody2D components
        Rigidbody2D armRigidbody = currentLimb.transform.Find("Arm").GetComponent<Rigidbody2D>();
        Rigidbody2D forearmRigidbody = currentLimb.transform.Find("Forearm").GetComponent<Rigidbody2D>();

        armRigidbody.bodyType = RigidbodyType2D.Dynamic;
        forearmRigidbody.bodyType = RigidbodyType2D.Dynamic;

        // Assign a control key from the available controls
        if (availableControls.Count > 0)
        {
            KeyCode controlKey = availableControls[0];
            currentLimb.GetComponentInChildren<CapsuleJoint>().control = controlKey;
            availableControls.RemoveAt(0);
            Tuple<FixedJoint2D, KeyCode> newTuple = new Tuple<FixedJoint2D, KeyCode>(closestJoint, controlKey);
            spawnedLimbs.Add(currentLimb, newTuple);
        }

        currentLimb = null;
    }

    FixedJoint2D CreateFixedJoint(Vector2 jointPosition, GameObject connectedObject)
    {
        // Check if the connected object has a Rigidbody2D component
        Rigidbody2D connectedRigidbody = connectedObject.GetComponent<Rigidbody2D>();

        Debug.Log("Connected " + connectedObject.name);
        // Create a FixedJoint2D component
        FixedJoint2D fixedJoint = connectedObject.AddComponent<FixedJoint2D>();

        // Set the connected body and connected anchor
        //fixedJoint.connectedBody = connectedRigidbody;
        fixedJoint.connectedAnchor = connectedRigidbody.transform.InverseTransformPoint(jointPosition);
        fixedJoint.anchor = connectedObject.transform.InverseTransformPoint(jointPosition);

        Debug.Log("2D Fixed Joint created at hit point: " + jointPosition);
        return fixedJoint;
    }

    void FlipLimbPrefab()
    {
        // Flip the limb prefab by negating the scale in the X-axis
        Vector3 scale = currentLimb.transform.localScale;
        scale.x *= -1f;
        currentLimb.transform.localScale = scale;
        CapsuleJoint arm = currentLimb.GetComponentInChildren<CapsuleJoint>();
        arm.motorSpeed *= -1;
        JointAngleLimits2D limts = arm.transform.GetComponent<HingeJoint2D>().limits;
        limts.min *= -1;
        arm.transform.GetComponent<HingeJoint2D>().limits = limts;
    }

    public void Die(GameObject limb)
    {
        KeyCode releasedControl = spawnedLimbs[limb].Item2;
        spawnedLimbs[limb].Item1.breakForce = 0;
        availableControls.Add(releasedControl);
        spawnedLimbs.Remove(limb);
    }
}
