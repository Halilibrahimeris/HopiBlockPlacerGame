using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlacer : MonoBehaviour
{
    public GameObject blockPrefab;
    public GameObject currentBlock;
    public GameObject previousBlock;
    public GameObject Checker;
    public int i = 0;
    [Space]
    public List<GameObject> SpawnedBlock;
    [Space]
    [SerializeField] private Transform[] Limits = new Transform[2];
    [Space]
    [SerializeField] private SlicerSpawner spawner;
    [Space]
    [SerializeField] private Material[] randomMat = new Material[5];
    [Space]
    public float blockHeight = 1f; // Blok yüksekliði
    public float BlockSpeed = 1f;

    public bool CanTouch = true;
    private bool OneTime = true;

    private Vector3 BlockPlacerStartPos;
    private Vector3 MainCamStartPos;

    void Start()
    {
        SpawnBlock();
        BlockPlacerStartPos = this.transform.position;
        MainCamStartPos = Camera.main.transform.position;
        BlockPlacerStartPos.y -= (blockHeight * 2);
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
    }

    public void SpawnBlock()
    {
        i++;
        currentBlock = Instantiate(blockPrefab);
        SpawnedBlock.Add(currentBlock);
        currentBlock.transform.localScale = previousBlock.transform.localScale;
        currentBlock.name = currentBlock.name + i.ToString();
        currentBlock.GetComponent<MeshRenderer>().material = randomMat[Random.Range(0,randomMat.Length)];
        currentBlock.GetComponent<CubeMovement>().Id += i;
        var Movement = currentBlock.GetComponent<CubeMovement>();
        Movement.Speed = BlockSpeed;
        Movement.PointA = Limits[0];
        Movement.PointB = Limits[1];
        if (previousBlock != null)
        {
            currentBlock.transform.position = new Vector3(
                transform.position.x,
                previousBlock.transform.position.y + blockHeight+ 0.01f,
                transform.position.z
            );
        }
        else
        {
            // Ýlk blok için baþlangýç pozisyonu belirle
            currentBlock.transform.position = transform.position;
        }

        // Kamerayý yukarý kaydýr
        Camera.main.transform.position = new Vector3(
            Camera.main.transform.position.x,
            Camera.main.transform.position.y + blockHeight + 0.01f,
            Camera.main.transform.position.z
        );
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y + blockHeight + 0.01f,
            transform.position.z
            );
        if (OneTime)
        {
            OneTime = false;
            return;
        }
        GameManager.Instance.IncraseScore();
        BlockSpeed += 0.2f;
    }

    void PlaceBlock()
    {
        currentBlock.GetComponent<CubeMovement>().CanMove = false;
        Debug.Log(previousBlock.name + " Calculate'e girdi");
        CalculateHangover();
        Debug.Log("Calculateden çýktý");
        previousBlock = currentBlock;
        Debug.Log("previous deðiþti");
        SpawnBlock();
    }

    public void CalculateHangover() 
    {
        GameObject checker = Instantiate(Checker, previousBlock.transform.position, Quaternion.identity);
        checker.transform.localScale = previousBlock.transform.localScale;
        checker.transform.SetParent(previousBlock.transform);
        checker.GetComponent<BoxCollider>().center = new Vector3(0, 2, 0);
        checker.SetActive(true);
        StartCoroutine(check(checker));
    }
    private IEnumerator check(GameObject checker)
    {
        yield return new WaitForSeconds(0.1f);
        if (checker.GetComponent<Check>().CheckRes)
        {
            //devam
        }
        else
        {
            CanTouch = false;
            Debug.Log("YandýnBrom");
            for (int i = 0; i < SpawnedBlock.Count; i++) 
            {
                Destroy(SpawnedBlock[i]);
            }
            SpawnedBlock.Clear();
            this.transform.position = BlockPlacerStartPos;
            Camera.main.transform.position = MainCamStartPos;
            previousBlock = GameManager.Instance.BaseCube;
            GameManager.Instance.BaseCube.GetComponent<CubeMovement>().ClearChilds();
            GameManager.Instance.StartCoroutine(GameManager.Instance.ResetCounter(this));
            CanTouch = true;
            BlockSpeed += 5;
            //reloadscene
        }
    }
}
