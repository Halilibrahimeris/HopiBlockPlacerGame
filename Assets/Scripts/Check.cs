using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check : MonoBehaviour
{
    public bool CheckRes = false;
    private CubeMovement cubeMovement;
    private void Start()
    {
        cubeMovement = GetComponentInParent<CubeMovement>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CanSlice" && other.gameObject.GetComponent<CubeMovement>() != null)
        {
            if(other.GetComponent<CubeMovement>().Id == (GameManager.Instance.placer.i - 1))
                CheckRes = true;
        }

    }
}
