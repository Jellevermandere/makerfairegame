using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(MeshRenderer))]
public class Ball : MonoBehaviour
{
    [SerializeField]
    private LayerMask goalLayer;
    [SerializeField]
    private RacerVisualListScriptableObject racerVisualList;
    [SerializeField]
    private Material baseMaterial;

    public int owner = -1;

    private MeshRenderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeOwner(int nr)
    {
        owner = nr;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((goalLayer.value & (1 << other.gameObject.layer)) > 0)
        {

            Goal ownerGoal = other.GetComponent<Goal>();
            ownerGoal.AddScore(1);
            owner = ownerGoal.GetPlayerNr();
            Debug.Log(owner);

            if (owner > -1 && owner < racerVisualList.racerVisuals.Count)
                rend.material = racerVisualList.racerVisuals[owner].racerMaterial;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((goalLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            owner = -1;
            Goal ownerGoal = other.GetComponent<Goal>();
            ownerGoal.AddScore(-1);
            rend.material = baseMaterial;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
    }
}
