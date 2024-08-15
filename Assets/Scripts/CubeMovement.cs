using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    public Transform PointA;
    public Transform PointB;

    public Transform PointC;
    public Transform PointD;

    public bool CanMove = true;
    public bool isHorizontalMove = true;

    public int Id = 0;
    public float Speed;
    public Transform[] t;

    private bool goLeft;
    private bool goUpper;
    private void Start()
    {
        if (PointA != null && isHorizontalMove)
        {
            transform.position = PointA.position;
        }
        else
        {
            transform.position = PointC.position;
        }
    }
    void Update()
    {
        if (CanMove)
        {
            if (isHorizontalMove)
            {
                if (PointA != null && PointB != null)
                {
                    if (transform.position.x <= PointB.position.x && goLeft == false)
                    {
                        transform.position += Vector3.right * Speed * Time.deltaTime;
                        if (transform.position.x >= PointB.position.x)
                        {
                            goLeft = true;
                        }
                    }
                    else if (transform.position.x >= PointA.position.x && goLeft == true)
                    {
                        transform.position += Vector3.left * Speed * Time.deltaTime;
                        if (transform.position.x <= PointA.position.x)
                        {
                            goLeft = false;
                        }
                    }
                }
            }
            else
            {
                if (transform.position.z >= PointD.position.z && goUpper == false)
                {
                    transform.position += Vector3.back * Speed * Time.deltaTime;
                    if (transform.position.z <= PointD.position.z)
                    {
                        goUpper = true;
                    }
                }
                else if (transform.position.z <= PointC.position.z && goUpper == true)
                {
                    transform.position += Vector3.forward * Speed * Time.deltaTime;
                    if (transform.position.z >= PointC.position.z)
                    {
                        goUpper = false;
                    }
                }
            }

        }
    }
    public void ClearChilds()
    {
        t = gameObject.transform.GetComponentsInChildren<Transform>();
        
        for (int i = 0; i < t.Length; i++) 
        {
            if (t[i].gameObject.name == this.gameObject.name)
            {

            }

            else
            {
                Destroy(t[i].gameObject);
            }
        }
        
    }
}
