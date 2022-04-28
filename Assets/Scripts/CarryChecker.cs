using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryChecker : MonoBehaviour
{
    [SerializeField]
    private LayerMask ballLayer;
    [SerializeField]
    private RacerController racerController;

    private void FixedUpdate()
    {
        racerController.hasBall = Physics.OverlapSphere(transform.position, transform.lossyScale.x, ballLayer).Length > 0;

    }
    /*
    private void OnTriggerStay(Collider other)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if ((ballLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            racerController.hasBall = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((ballLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            racerController.hasBall = false;
        }
    }
    */
}
