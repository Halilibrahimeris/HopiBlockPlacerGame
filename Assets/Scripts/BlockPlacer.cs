using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlacer : MonoBehaviour
{
    public GameObject blockPrefab;
    public GameObject currentBlock;
    public GameObject previousBlock;
    public GameObject Checker;

    [HideInInspector]public int i = 0;
    [HideInInspector]public int matIndex = 0;

    [Space]
    public List<GameObject> SpawnedBlock;
    [Space]
    [SerializeField] private Transform[] Limits = new Transform[2];
    [Space]
    [SerializeField] private SlicerSpawner spawner;
    [Space]
    public Material[] randomMat = new Material[5];
    [Space]
    public float blockHeight = 1f; // Blok yüksekliði
    public float BlockSpeed = 1f;

    public bool CanTouch = true;
    private bool OneTime = true;
    [HideInInspector] public bool CanResize = false;

    [SerializeField]private Vector3 BlockPlacerStartPos;
    private Vector3 MainCamStartPos;

    void Start()
    {
        blockPrefab.transform.localScale = previousBlock.transform.localScale;
        SpawnBlock(true);
        transform.position = new Vector3(transform.position.x, previousBlock.transform.localScale.y, transform.position.z);
        BlockPlacerStartPos = this.transform.position;
        MainCamStartPos = Camera.main.transform.position;
        blockHeight = previousBlock.transform.localScale.y;
        MainCamStartPos.y -= blockHeight;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && CanTouch)
        {
            GameManager.Instance.ScreenClick();
            spawner.Cube = previousBlock;
            spawner.SpawnSlicer();
            PlaceBlock();
        }
        if (CanResize)
        {
            ResizeCurrent(currentBlock, previousBlock);
            CanResize = false;
        }
    }

    public void SpawnBlock(bool needUp)
    {
        i++;
        currentBlock = Instantiate(blockPrefab);
        
        currentBlock.name = currentBlock.name + i.ToString();
        SpawnedBlock.Add(currentBlock);
        ChangeMaterial();

        currentBlock.GetComponent<CubeMovement>().Id += i;

        var Movement = currentBlock.GetComponent<CubeMovement>();
        Movement.Speed = BlockSpeed;
        Movement.PointA = Limits[0];
        Movement.PointB = Limits[1];

        if (previousBlock != null)
        {
            currentBlock.transform.position = new Vector3(
                transform.position.x,
                previousBlock.transform.position.y + blockHeight,
                transform.position.z
            );
        }
        else
        {
            // Ýlk blok için baþlangýç pozisyonu belirle
            currentBlock.transform.position = transform.position;
        }
        if (OneTime)
        {
            OneTime = false;
            return;
        }
        if (needUp)
        {
            // Kamerayý yukarý kaydýr
            Camera.main.transform.position = new Vector3(
                Camera.main.transform.position.x,
                Camera.main.transform.position.y + blockHeight + 0.01f,
                Camera.main.transform.position.z
            );
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y + blockHeight,
                transform.position.z
                );
        }
    }

    private void ChangeMaterial()
    {
        currentBlock.GetComponent<MeshRenderer>().material = randomMat[matIndex];
        matIndex++;
        if (matIndex == randomMat.Length)
            matIndex = 0;
    }

    void PlaceBlock()
    {
        currentBlock.GetComponent<CubeMovement>().CanMove = false;
        Debug.Log(previousBlock.name + " Calculate'e girdi");
        CalculateHangover();
        Debug.Log("Calculateden çýktý");
        previousBlock = currentBlock;
        Debug.Log("previous deðiþti");
        SpawnBlock(true);
    }

    public void CalculateHangover() 
    {
        GameObject checker = Instantiate(Checker, previousBlock.transform.position, Quaternion.identity);
        checker.transform.localScale = previousBlock.transform.localScale;
        checker.transform.position = previousBlock.transform.position;
        checker.transform.SetParent(previousBlock.transform);
        checker.transform.localPosition = Vector3.zero;
        checker.GetComponent<BoxCollider>().center = new Vector3(0, 1f, 0);
        checker.SetActive(true);
        StartCoroutine(check(checker));
    }
    private IEnumerator check(GameObject checker)
    {
        yield return new WaitForSeconds(0.1f);
        if (checker.GetComponent<Check>().CheckRes)
        {
            GameManager.Instance.IncraseScore();
            if ((GameManager.Instance.GetScore() % 5) == 0)
                BlockSpeed += 0.2f;
        }
        else
        {
            CanTouch = false;
            for (int i = 0; i < SpawnedBlock.Count; i++) 
            {
                Destroy(SpawnedBlock[i]);
            }
            SpawnedBlock.Clear();
            transform.position = BlockPlacerStartPos;
            Camera.main.transform.position = MainCamStartPos;
            previousBlock = GameManager.Instance.BaseCube;
            GameManager.Instance.BaseCube.GetComponent<CubeMovement>().ClearChilds();
            CanTouch = true;
            SpawnBlock(false);
            //reloadscene
        }
    }

    private void ResizeCurrent(GameObject currentBlock, GameObject previousBlock)
    {
        Debug.Log("Önceki previus block adý: " + previousBlock.name);
        Debug.Log("Þu anki current block adý: " + currentBlock);
        currentBlock.transform.localScale = previousBlock.transform.localScale;
    }
}
