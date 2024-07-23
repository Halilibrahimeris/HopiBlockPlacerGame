using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    public Transform PointA;
    public Transform PointB;
    public bool CanMove = true;
    public int Id = 0;
    public float Speed;
    public Transform[] t;

    private bool goLeft;
    private void Start()
    {
        if (PointA != null)
        {
            transform.position = PointA.position;
        }
    }
    void Update()
    {
        if (CanMove)
        {
            if (PointA != null && PointB != null)
            {
                if (transform.position.x <= PointB.position.x && goLeft == false)
                {
                    transform.position += Vector3.right * Speed * Time.deltaTime;
                    if(transform.position.x >= PointB.position.x)
                    {
                        goLeft = true;
                    }
                }
                else if(transform.position.x >= PointA.position.x && goLeft == true)
                {
                    transform.position += Vector3.left * Speed * Time.deltaTime;
                    if(transform.position.x <= PointA.position.x)
                    {
                        goLeft = false;
                    }
                }
                //transform.position = Vector3.Lerp(PointA.position, PointB.position, Mathf.PingPong(Time.time / Speed, 1));
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
