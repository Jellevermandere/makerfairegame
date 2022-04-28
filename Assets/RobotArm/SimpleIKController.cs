using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach this script to the upper limb
/// </summary>
public class SimpleIKController : MonoBehaviour
{
    [SerializeField]
    private bool simple = false;

    [SerializeField]
    private Transform lowerLimb;
    [SerializeField]
    private Transform upperLimb;
    [SerializeField]
    private Transform extremity;
    [SerializeField]
    private Transform extremityEnd;

    [SerializeField]
    private float extremityOffset = 0f;

    [SerializeField]
    private bool mirrorPole = true, controlextremityRotation = false;

    [SerializeField]
    public Vector2 extremityRotation = Vector2.zero;

    [SerializeField]
    private Transform target;
    [SerializeField]
    private Transform pole;

    [SerializeField]
    private float upperLimbLength;
    [SerializeField]

    private float lowerLimbLength;
    [SerializeField]

    private float maxLength;
    [SerializeField]

    private float currentDistance;

    private float offsetAngle;
    private Vector3 aim;
    [SerializeField]
    public float[] angles = { 0f, 0f, 0f, 0f, 0f };


    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        if (simple) SolveAngleIk();
        else solveIK();
    }

    void Setup()
    {
        upperLimbLength = Vector3.Distance(upperLimb.position, lowerLimb.position);
        lowerLimbLength = Vector3.Distance(lowerLimb.position, extremity.position);

        maxLength = upperLimbLength + lowerLimbLength;

    }

    void solveIK()
    {
        if(mirrorPole) MirrorPole();

        Vector3 wristTargetPos = target.position;

        if (controlextremityRotation)
        {
            wristTargetPos -= target.forward * extremityOffset;
        }
        else
        {
            //wristTargetPos -= Vector3.Normalize(target.position - transform.position) * extremityOffset;
            Vector3 offsetDirection = (Vector3.Scale(new Vector3(-1,0,-1), pole.position - transform.position) * Mathf.Cos(extremityRotation.x * Mathf.Deg2Rad) + Vector3.up * Mathf.Sin(extremityRotation.x * Mathf.Deg2Rad)).normalized;
            Debug.DrawLine(transform.position, offsetDirection);

            wristTargetPos -= offsetDirection * extremityOffset;
        }

        aim = pole.position - (transform.position + wristTargetPos) / 2.0f;
        transform.LookAt(wristTargetPos, aim);

        currentDistance = Vector3.Distance(transform.position, wristTargetPos);

        if (currentDistance < maxLength)
        {
            offsetAngle = - Mathf.Rad2Deg * Mathf.Acos((Mathf.Pow(currentDistance, 2) + Mathf.Pow(upperLimbLength, 2) - Mathf.Pow(lowerLimbLength, 2)) / (2 * currentDistance * upperLimbLength));

            transform.Rotate(Vector3.right, offsetAngle, Space.Self);

        }

        lowerLimb.LookAt(wristTargetPos, aim);

        extremity.LookAt(target, Vector3.up);

        Debug.DrawLine(lowerLimb.position, pole.position, Color.blue);
        Debug.DrawLine(transform.position, wristTargetPos, Color.red);
    }

    void solveSimpleIK()
    {
        if (mirrorPole) MirrorPole();

        Vector3 wristTargetPos = target.position;

        if (controlextremityRotation)
        {
            wristTargetPos -= target.forward * extremityOffset;
        }
        else
        {
            //wristTargetPos -= Vector3.Normalize(target.position - transform.position) * extremityOffset;
            Vector3 offsetDirection = (Vector3.Scale(new Vector3(-1, 0, -1), pole.position - transform.position) * Mathf.Cos(extremityRotation.x * Mathf.Deg2Rad) + Vector3.up * Mathf.Sin(extremityRotation.x * Mathf.Deg2Rad)).normalized;
            Debug.DrawLine(transform.position, offsetDirection);

            wristTargetPos -= offsetDirection * extremityOffset;
        }

        // base rotation

        aim = pole.position - (transform.position + wristTargetPos) / 2.0f;


        transform.rotation = Quaternion.LookRotation(Vector3.Scale(new Vector3(1, 0, 1), aim), Vector3.up);

        upperLimb.LookAt(wristTargetPos, Vector3.up);

        currentDistance = Vector3.Distance(transform.position, wristTargetPos);

        if (currentDistance < maxLength)
        {
            offsetAngle = -Mathf.Rad2Deg * Mathf.Acos((Mathf.Pow(currentDistance, 2) + Mathf.Pow(upperLimbLength, 2) - Mathf.Pow(lowerLimbLength, 2)) / (2 * currentDistance * upperLimbLength));

            upperLimb.Rotate(Vector3.right, offsetAngle, Space.Self);

        }

        lowerLimb.LookAt(wristTargetPos, Vector3.up);

        extremity.LookAt(target, Vector3.up);
        

        Debug.DrawLine(lowerLimb.position, pole.position, Color.blue);
        Debug.DrawLine(transform.position, wristTargetPos, Color.red);
    }

    bool SolveAngleIk()
    {

        Vector3 targetPosition = target.position - transform.position;

        //Check if position is valid (e.g. if the Y position > 0)
        if (targetPosition.y < 0)
        {
            //return false;
        }

        if (targetPosition.magnitude < (upperLimbLength - lowerLimbLength))
        {
            return false;
        }

        //Offset the target position with the end offset & rotation
        Vector3 groundDirection = Vector3.Scale(new Vector3(1,0,1),targetPosition);
        float endRad = -extremityRotation.x * Mathf.Deg2Rad;
        Vector3 offsetDirection = Vector3.Normalize(groundDirection) * Mathf.Cos(endRad) + Vector3.up * Mathf.Sin(endRad);
        Vector3 wristTargetPos = targetPosition - Vector3.Normalize(offsetDirection) * extremityOffset;
        groundDirection = Vector3.Scale(new Vector3(1, 0, 1), wristTargetPos);

        if (wristTargetPos.magnitude < (upperLimbLength - lowerLimbLength))
        {
            return false;
        }

        //Step 1: aim the base (0 deg is right, rotating counter clockwise)
        float baseYaw = Mathf.Atan2(-targetPosition.x, targetPosition.z);

        // Step 2: Determine the distance to the target point
        //if further than the lenght: go straight
        //else: calculate the elbow and base angle
        float targetDistance = wristTargetPos.magnitude;
        //float basePitch = groundDirection.magnitude == 0 ? (Mathf.PI / 2f) : Vector3.SignedAngle(transform.forward, wristTargetPos, -transform.right) * Mathf.Deg2Rad; //(Mathf.Atan(wristTargetPos.y / groundDirection.magnitude));
        float basePitch = groundDirection.magnitude == 0 ? (Mathf.PI / 2f) : Vector3.Angle(transform.forward, wristTargetPos) * Mathf.Deg2Rad; //(Mathf.Atan(wristTargetPos.y / groundDirection.magnitude));
        float elbowPitch = Mathf.PI;

        if ((upperLimbLength + lowerLimbLength) > targetDistance) {
            //the target is withing reach of the robot arm
            //print("The distance is within reach")
            float a = upperLimbLength;
            float b = lowerLimbLength;
            float c = targetDistance;
            basePitch += Mathf.Acos((Mathf.Pow(b, 2) - Mathf.Pow(c, 2) - Mathf.Pow(a, 2)) / (-2 * a * c));
            elbowPitch = Mathf.Acos((Mathf.Pow(c, 2) - Mathf.Pow(a, 2) - Mathf.Pow(b, 2)) / (-2 * a * b));
        }
        // Step 3: aim the wrist at the target
        // first set the wrist horizontal
        // then add the endrotation
        float wristPitch = -basePitch - elbowPitch + Mathf.PI;
        //print("wristPitch:", math.degrees(wristPitch))
        //if(wristPitch < 0): wristPitch = -wristPitch
        wristPitch += -extremityRotation.x * Mathf.Deg2Rad + Mathf.PI / 2f;

        // Step 4: apply all the angles to the controllers
        angles[0] = baseYaw * Mathf.Rad2Deg ;
        angles[1] = rad_to_servo(basePitch, true);
        angles[2] = rad_to_servo(elbowPitch);
        angles[3] = rad_to_servo(wristPitch, true);
        angles[4] = extremityRotation.y;

        transform.rotation = Quaternion.Euler(0, -angles[0],0);
        upperLimb.localRotation = Quaternion.Euler(angles[1]-180, 0, 0);
        lowerLimb.localRotation = Quaternion.Euler(-angles[2], 0, 0);
        extremity.localRotation = Quaternion.Euler(angles[3]-180, 0, 0);
        extremityEnd.localRotation = Quaternion.Euler(0,0,angles[4]);



        return true;
    }

    void MirrorPole()
    {
        pole.position = Vector3.Scale(target.position - transform.position,new Vector3(-1, 1, -1)) + transform.position;
    }

    float rad_to_servo(float rad, bool sup = false) {
        if (sup) return Mathf.Max(0, Mathf.Min(180, 180 - rad * Mathf.Rad2Deg));

        else return Mathf.Max(0, Mathf.Min(180, rad * Mathf.Rad2Deg));
    }



}
