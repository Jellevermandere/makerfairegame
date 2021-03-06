using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RacerController : MonoBehaviour
{
    [Header("Player Parameters")]
    [SerializeField]
    public int playerNr = 0;
    [SerializeField]
    private float playerAcceleration = 100;
    [SerializeField]
    private float maxSpeed = 20;
    [SerializeField]
    private float rotationSpeed = 100;
    [SerializeField]
    private float sidewaysFrictionForce = 100;
    [SerializeField]
    private float gravityForce = 20;
    [SerializeField]
    private Vector3 forwardTransformOffset = new Vector3(0f, 0.5f, 0f);
    [Header("Scene objects")]
    [SerializeField]
    public Transform forwardTransform;
    [SerializeField]
    public Transform goal;
    public InputController inputController;
    [SerializeField]
    private RacerVisualListScriptableObject racerVisualList;
    [SerializeField]
    private MeshRenderer bodyMesh;


    public bool hasBall = false;
    public bool inGoal = false;
    public int score = 0;

    private RacerInput playerInput = new RacerInput();
    private Rigidbody rb;
    private Vector3 forwardDirection = Vector3.forward;
    public GameManager gm;


    // Start is called before the first frame update
    void Start()
    {
        if(inputController)
            playerInput = inputController.GetPlayerInput(playerNr);

        rb = GetComponent<Rigidbody>();

        forwardDirection = transform.forward;
        SetPlayerColor();
    }

    // Update is called once per frame
    void Update()
    {

        RotateForward();
    }

    private void FixedUpdate()
    {
        ApplyGravity();
        MovePlayer();
    }

    void MovePlayer()
    {
        if (!gm.playing) return;
        rb.AddForce(forwardDirection * playerInput.moveDirection.y * playerAcceleration * Time.deltaTime, ForceMode.VelocityChange);
        Debug.DrawRay(transform.position, forwardDirection * playerInput.moveDirection.y * playerAcceleration * Time.deltaTime);
        // get the angle difference between the current forward speed and the forward direction
        rb.AddForce(GetForwardCorrection() * sidewaysFrictionForce * Time.deltaTime, ForceMode.VelocityChange);
        Debug.DrawRay(transform.position, GetForwardCorrection() * sidewaysFrictionForce * Time.deltaTime);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
    }

    private Vector3 GetForwardCorrection()
    {
        return  forwardTransform.TransformVector(Vector3.Scale(forwardTransform.InverseTransformVector(rb.velocity), new Vector3(-1,0,0)));
    }

    private void ApplyGravity()
    {
        rb.AddForce(Vector3.down * Time.deltaTime * gravityForce, ForceMode.VelocityChange);
    }

    void RotateForward()
    {
        Vector3 oldRotation = forwardTransform.rotation.eulerAngles;
        forwardTransform.rotation = Quaternion.Euler(0, oldRotation.y + playerInput.moveDirection.x * rotationSpeed * Time.deltaTime, 0);
        forwardDirection = forwardTransform.forward;

        forwardTransform.position = transform.position + forwardTransformOffset;
    }

    void SetPlayerColor()
    {
        bodyMesh.material = racerVisualList.racerVisuals[playerNr].racerMaterial;
    }

}
