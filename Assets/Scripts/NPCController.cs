using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField]
    [Tooltip("The value to normalize the Rotation direction")]
    private float rotationMultiplier = 90;
    [SerializeField]
    [Tooltip("The value to normalize the Rotation direction")]
    private float forwardMultiplier = 3;
    [SerializeField]
    private float happyGoalDistance = 1;
    [SerializeField]
    private float backTime = 1f;
    [SerializeField]
    private Vector3 baseRobotTargetPos = new Vector3(0, 0.2f, 0.35f);
    [SerializeField]
    [Tooltip("Degrees per second")]
    private float maxRobotSpeed = 90;
    // x = rotation, Y is forward backwards
    [SerializeField]
    private Vector2 robotOffset = Vector2.zero;
    [SerializeField]
    private Vector2 robotMaxAngles = new Vector2(45, 30);

    [SerializeField]
    private int racerNr = 1;
    [SerializeField]
    private bool UseRobot = false;

    [Header("Scene Objects")]
    [SerializeField]
    private SimpleIKController iKController;
    [SerializeField]
    private Transform ikTarget;
    [SerializeField]
    private InputController inputController;
    [SerializeField]
    private Spawner spawner;
    [SerializeField]
    private GameManager gm;


    private Transform targetBall;
    private RacerInput input;

    private bool goingBack = false;
    private float stillBackTime = 0;
    [SerializeField]
    private Vector2 inputDir = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        input = inputController.playerInputs[racerNr];

        if (UseRobot) Invoke("CalibrateOffset", 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.playing)
        {
            if (goingBack)
            {
                stillBackTime -= Time.deltaTime;

                if (stillBackTime > 0)
                {
                    inputDir = Vector2.down;
                }
                else goingBack = false;
            }
            if(!goingBack)
            {
                Vector3 moveDirection = spawner.racers[racerNr].forwardTransform.InverseTransformPoint(GetTarget().position);
                float xDir = Vector3.SignedAngle(moveDirection, Vector3.forward, Vector3.up);
                inputDir = new Vector2(Mathf.Clamp(-xDir / rotationMultiplier, -1, 1), Mathf.Clamp(Mathf.Abs(moveDirection.z) / forwardMultiplier, -1, 1));
            }

            if(!UseRobot) input.moveDirection = inputDir;
            else
            {
                ikTarget.position = iKController.transform.parent.TransformPoint(baseRobotTargetPos);
                iKController.extremityRotation = Vector2.MoveTowards(iKController.extremityRotation, new Vector2((-robotOffset.y + inputDir.y) * robotMaxAngles.y, (-robotOffset.x + inputDir.x) * robotMaxAngles.x + 90), 90 * Time.deltaTime);
                // input.moveDirection = inputDir;
            }



        }
    }
    [ContextMenu("Calibrate")]
    public void CalibrateOffset()
    {
        robotOffset = input.moveDirection;
    }

    Transform GetTarget()
    {
        if (spawner.racers[racerNr].hasBall)
        {
            if (Vector3.Distance(spawner.racers[racerNr].goal.position, spawner.racers[racerNr].transform.position) > happyGoalDistance)
                return spawner.racers[racerNr].goal;
            else
            {
                Debug.Log("Delivered, going back");
                stillBackTime = backTime;
                goingBack = true;
                targetBall = null;
            }
        }
        if (targetBall)
        {
            return targetBall;
        }
        else
        {
            float minDistance = Mathf.Infinity;
            for (int i = 0; i < spawner.balls.Count; i++)
            {
                float newDist = Vector3.SqrMagnitude(spawner.racers[racerNr].transform.position - spawner.balls[i].position);
                if (newDist < minDistance)
                {
                    if(spawner.balls[i].GetComponent<Ball>().owner != racerNr)
                    {
                        targetBall = spawner.balls[i];
                        minDistance = newDist;
                    }
                    
                }
            }
            if (targetBall == null) targetBall = spawner.racers[racerNr].goal;
            return targetBall;
        }
    }


}
