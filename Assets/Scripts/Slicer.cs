using UnityEngine;

public class Slicer : MonoBehaviour
{
    public bool canSlice = false;
    public bool isLeft;
    public bool isHorizantal;
    public bool isFront;
    public bool doOneTime = true;
    private Vector3 endPoint;

    public GameObject current;
    public GameObject previous;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "CanSlice" && canSlice && other.GetComponent<CubeMovement>().Id == (GameManager.Instance.placer.i -1))
        {
            // Dokunulan noktayý al
            Vector3 contactPoint = other.ClosestPoint(transform.position);
            CalculateSlice(other.gameObject, contactPoint);
        }
        
    }

    private void CalculateSlice(GameObject obj, Vector3 contactPoint)
    {
        #region forXAxis
        if (isHorizantal)
        {
            if (isLeft && doOneTime)
            {
                //set endpoint of obj to left
                endPoint.x = (obj.transform.position.x) - (obj.transform.localScale.x / 2);
                //calculate scale
                GameObject newobj = Instantiate(GameManager.Instance.SpawnObj);
                newobj.GetComponent<MeshRenderer>().materials[0] = obj.GetComponent<MeshRenderer>().materials[0];
                float xScale = Mathf.Abs(endPoint.x - contactPoint.x);
                newobj.transform.localScale = new Vector3(xScale, obj.transform.localScale.y, obj.transform.localScale.z);
                //calculate pos
                float xPos = (endPoint.x + contactPoint.x) / 2;
                newobj.transform.position = new Vector3(xPos, obj.transform.position.y, obj.transform.position.z);
                //calculate orj obj scale
                xScale = obj.transform.localScale.x - newobj.transform.localScale.x;
                obj.transform.localScale = new Vector3(xScale, obj.transform.localScale.y, obj.transform.localScale.z);
                obj.transform.position = new Vector3((newobj.transform.localScale.x / 2) + obj.transform.position.x, obj.transform.position.y, obj.transform.position.z);
                newobj.AddComponent<Rigidbody>();
                newobj.AddComponent<BoxCollider>();
                ChangeMat(newobj);
                Destroy(newobj, 1f);
                doOneTime = false;
            }
            else if (!isLeft && doOneTime)
            {
                //set endpoint of obj to right
                endPoint.x = (obj.transform.position.x) + (obj.transform.localScale.x / 2);
                //calculate new Obj Scale
                GameObject newobj = Instantiate(GameManager.Instance.SpawnObj);
                float xScale = Mathf.Abs(endPoint.x - contactPoint.x);
                newobj.transform.localScale = new Vector3(xScale, obj.transform.localScale.y, obj.transform.localScale.z);
                //calculate new obj pos
                float xPos = (endPoint.x + contactPoint.x) / 2;
                newobj.transform.position = new Vector3(xPos, obj.transform.position.y, obj.transform.position.z);
                //calculate orj obj scale
                xScale = (obj.transform.localScale.x - newobj.transform.localScale.x);
                obj.transform.localScale = new Vector3(xScale, obj.transform.localScale.y, obj.transform.localScale.z);
                obj.transform.position = new Vector3(obj.transform.position.x - (newobj.transform.localScale.x / 2), obj.transform.position.y, obj.transform.position.z);
                newobj.AddComponent<Rigidbody>();
                newobj.AddComponent<BoxCollider>();
                ChangeMat(newobj);
                Destroy(newobj, 1f);
                doOneTime = false;
            }
        }

        #endregion

        #region forzAxis
        else
        {
            if (!isFront && doOneTime)
            {
                Debug.Log("Bozuk taraf çalýþtý");
                // obj'nin uç noktasýný sol tarafa (arka) ayarla
                endPoint.z = (obj.transform.position.z) - (obj.transform.localScale.z / 2);
                Debug.Log("Endpoint Z ayarlandý: " + endPoint.z);
                // Ölçeði hesapla
                GameObject newobj = Instantiate(GameManager.Instance.SpawnObj);
                Debug.Log("Yeni obj spawnlandý");
                newobj.GetComponent<MeshRenderer>().materials[0] = obj.GetComponent<MeshRenderer>().materials[0];
                float zScale = Mathf.Abs(endPoint.z - contactPoint.z);
                Debug.Log("zScale hesaplandý: " + zScale);
                newobj.transform.localScale = new Vector3(obj.transform.localScale.x, obj.transform.localScale.y, zScale);
                Debug.Log("Yeni obj'nin scale'i hesaplandý: " + newobj.transform.localScale);
                // Pozisyonu hesapla
                float zPos = (endPoint.z + contactPoint.z) / 2;
                newobj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, zPos);
                // Orijinal objenin ölçeðini hesapla
                zScale = obj.transform.localScale.z - newobj.transform.localScale.z;
                obj.transform.localScale = new Vector3(obj.transform.localScale.x, obj.transform.localScale.y, zScale);
                obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, (newobj.transform.localScale.z / 2) + obj.transform.position.z);
                newobj.AddComponent<Rigidbody>();
                ChangeMat(newobj);
                Destroy(newobj, 1f);
                doOneTime = false;
            }
            else
            {
                // obj'nin uç noktasýný sað tarafa (ön) ayarla
                endPoint.z = (obj.transform.position.z) + (obj.transform.localScale.z / 2);
                // Yeni objenin ölçeðini hesapla
                GameObject newobj = Instantiate(GameManager.Instance.SpawnObj);
                float zScale = Mathf.Abs(endPoint.z - contactPoint.z);
                newobj.transform.localScale = new Vector3(obj.transform.localScale.x, obj.transform.localScale.y, zScale);
                // Yeni objenin pozisyonunu hesapla
                float zPos = (endPoint.z + contactPoint.z) / 2;
                newobj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, zPos);
                // Orijinal objenin ölçeðini hesapla
                zScale = (obj.transform.localScale.z - newobj.transform.localScale.z);
                obj.transform.localScale = new Vector3(obj.transform.localScale.x, obj.transform.localScale.y, zScale);
                obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, obj.transform.position.z - (newobj.transform.localScale.z / 2));
                newobj.AddComponent<Rigidbody>();
                ChangeMat(newobj);
                Destroy(newobj, 1f);
                doOneTime = false;
            }
        }
        
        #endregion

        GameManager.Instance.placer.CanResize = true;
    }

    private void ChangeMat(GameObject newobj)
    {
        BlockPlacer place = GameManager.Instance.placer;
        int index = place.matIndex - 1;
        if(index < 0)
        {
            index = 0;
        }
        if(index > place.randomMat.Length)
            index = place.randomMat.Length;
        newobj.GetComponent<MeshRenderer>().material = place.randomMat[index];
    }
}
