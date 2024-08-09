using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlicerSpawner : MonoBehaviour
{
    public GameObject Slicer;
    public GameObject Cube;

    public void SpawnSlicer(bool isHorizantal)
    {

        if (Slicer == null || Cube == null)
        {
            Debug.LogError("Slicer or Cube is not assigned.");
            return;
        }

        // Cube'un transformunu al
        Transform cubeTransform = Cube.transform;

        // Cube'un üst yüzeyindeki kenarlarýn pozisyonunu hesapla
        Vector3 topCenter = cubeTransform.position + new Vector3(0, cubeTransform.localScale.y/2, 0);

        //Küp yatay olarak ilerliyor ise
        if (isHorizantal)
        {
            // Cube'un üst yüzeyindeki kenarlarýn pozisyonunu hesapla
            Vector3 topEdgeLeft = topCenter + new Vector3(-cubeTransform.localScale.x / 2, 0, 0);
            Vector3 topEdgeRight = topCenter + new Vector3(cubeTransform.localScale.x / 2, 0, 0);

            // Slicer nesnesini instantiate et ve konumunu ayarla
            GameObject SlicerLeft = Instantiate(Slicer, topEdgeLeft, Quaternion.identity);
            GameObject SlicerRight = Instantiate(Slicer, topEdgeRight, Quaternion.identity);

            // isLeft or Right and isHorizantal
            SlicerLeft.GetComponent<Slicer>().isHorizantal = true;
            SlicerLeft.GetComponent<Slicer>().isLeft = true;
            SlicerRight.GetComponent<Slicer>().isHorizantal=true;

            // Slicer'ý Cube'un y ekseni boyunca yukarý doðru uzat
            Vector3 scale = new Vector3(0f, Cube.transform.localScale.y, Cube.transform.localScale.z);
            SlicerLeft.transform.localScale = scale;
            SlicerRight.transform.localScale = scale;

            //Konumlarýný yeniden dzenle
            float slicerYScale = scale.y / 2;
            SlicerLeft.transform.position = new Vector3(SlicerLeft.transform.position.x, SlicerLeft.transform.position.y + slicerYScale + 0.01f, SlicerLeft.transform.position.z);
            SlicerRight.transform.position = new Vector3(SlicerRight.transform.position.x, SlicerRight.transform.position.y + slicerYScale + 0.01f, SlicerRight.transform.position.z);

            SlicerLeft.transform.SetParent(GameManager.Instance.placer.previousBlock.transform);
            SlicerRight.transform.SetParent(GameManager.Instance.placer.previousBlock.transform);

            SlicerLeft.GetComponent<Slicer>().canSlice = true;
            SlicerLeft.transform.position = new Vector3(SlicerLeft.transform.position.x, SlicerLeft.transform.position.y + 0.01f, SlicerLeft.transform.position.z);

            SlicerRight.GetComponent<Slicer>().canSlice = true;
            SlicerRight.transform.position = new Vector3(SlicerRight.transform.position.x, SlicerRight.transform.position.y + 0.01f, SlicerRight.transform.position.z);
        }
        //küp yatay olarak ilerliyor ise
        else
        {
            Vector3 topEdgeFront = topCenter + new Vector3(0, 0, cubeTransform.localScale.z / 2);
            Vector3 topEdgeBack = topCenter + new Vector3(0, 0, -cubeTransform.localScale.z / 2);

            // Slicer nesnesini instantiate et ve konumunu ayarla
            GameObject SlicerFront = Instantiate(Slicer, topEdgeFront, Quaternion.identity);
            GameObject SlicerBack = Instantiate(Slicer, topEdgeBack, Quaternion.identity);

            // isLeft or Right and isHorizantal
            SlicerFront.GetComponent<Slicer>().isFront = true;

            // Slicer'ý Cube'un y ekseni boyunca yukarý doðru uzat
            Vector3 scale = new Vector3(0f, Cube.transform.localScale.y, Cube.transform.localScale.z);
            SlicerFront.transform.localScale = scale;
            SlicerBack.transform.localScale = scale;

            //Konumlarýný yeniden dzenle
            float slicerYScale = scale.y / 2;
            SlicerFront.transform.position = new Vector3(SlicerFront.transform.position.x, SlicerFront.transform.position.y + slicerYScale + 0.01f, SlicerFront.transform.position.z);
            SlicerBack.transform.position = new Vector3(SlicerBack.transform.position.x, SlicerBack.transform.position.y + slicerYScale + 0.01f, SlicerBack.transform.position.z);

            SlicerFront.transform.SetParent(GameManager.Instance.placer.previousBlock.transform);
            SlicerBack.transform.SetParent(GameManager.Instance.placer.previousBlock.transform);

            SlicerFront.GetComponent<Slicer>().canSlice = true;
            SlicerFront.transform.position = new Vector3(SlicerFront.transform.position.x, SlicerFront.transform.position.y + 0.01f, SlicerFront.transform.position.z);

            SlicerBack.GetComponent<Slicer>().canSlice = true;
            SlicerBack.transform.position = new Vector3(SlicerBack.transform.position.x, SlicerBack.transform.position.y + 0.01f, SlicerBack.transform.position.z);
        }




    }
   
}
