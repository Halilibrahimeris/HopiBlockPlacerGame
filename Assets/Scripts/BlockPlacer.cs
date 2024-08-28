using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlacer : MonoBehaviour
{
    public GameObject blockPrefab;
    public GameObject currentBlock;
    public GameObject previousBlock;
    public GameObject Checker;
    public GameObject PerfectPlacement;

    public int i = 0;
    public int matIndex = 1;
    private int ColorIndex;

    [Space]
    public List<GameObject> SpawnedBlock;
    [Space]
    [SerializeField] private Transform[] Limits = new Transform[2];
    [Space]
    [SerializeField] private SlicerSpawner spawner;
    [Space]
    public Material[] randomMat = new Material[5];
    public Color[] BGColor;
    [Space]
    public float blockHeight = 1f; // Blok y�ksekli�i
    public float BlockSpeed = 1f;

    public bool CanTouch = true;
    private bool OneTime = true;
    [SerializeField]private bool isHorizantal;
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
            spawner.SpawnSlicer(isHorizantal);
            PlaceBlock();
            SoundManager.Instance.PlayMainSound(SoundManager.BlockSoundTypes.Click);
        }
        if (CanResize)
        {
            ResizeCurrent(currentBlock, previousBlock);
            CanResize = false;
        }
    }

    private void BGColorChange()
    {
        Camera.main.backgroundColor = BGColor[ColorIndex];
        ColorIndex++;
        if (ColorIndex == BGColor.Length)
            ColorIndex = 0;
    }

    public void SpawnBlock(bool needUp)
    {
        i++;

        currentBlock = Instantiate(blockPrefab);
        
        currentBlock.name = currentBlock.name + i.ToString();
        SpawnedBlock.Add(currentBlock);
        ChangeMaterial();

        currentBlock.GetComponent<CubeMovement>().Id = i;

        var Movement = currentBlock.GetComponent<CubeMovement>();
        if (i % 2 == 0)
        {
            Movement.isHorizontalMove = true;
            Movement.Speed = BlockSpeed;
            Movement.PointA = Limits[0];
            Movement.PointB = Limits[1];
        }
        else
        {
            Movement.isHorizontalMove = false;
            Movement.Speed = BlockSpeed;
            Movement.PointC = Limits[2];
            Movement.PointD = Limits[3];
        }

        isHorizantal = Movement.isHorizontalMove;

        if (previousBlock != null)
        {
            currentBlock.transform.position = new Vector3(
                previousBlock.transform.position.x,
                previousBlock.transform.position.y + blockHeight,
                previousBlock.transform.position.z
            );
        }
        else
        {
            // �lk blok i�in ba�lang�� pozisyonu belirle
            currentBlock.transform.position = transform.position;
        }
        if (OneTime)
        {
            OneTime = false;
            return;
        }
        if (needUp)
        {
            // Kameray� yukar� kayd�r
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
        CalculateHangover();
        previousBlock = currentBlock;
        SpawnBlock(true);
    }

    public void CalculateHangover() 
    {
        GameObject checker = Instantiate(Checker, previousBlock.transform.position, Quaternion.identity);
        checker.name = "Checker" + i;
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
            Restart();
            BGColorChange();
            //reloadscene
        }
        if (TriggerCheck(checker.GetComponent<BoxCollider>(), currentBlock))
        {
            Debug.Log("Tam olarak i�inde");
            SpawnPerfectPlacementItem();
        }
        else
        {
            Debug.Log("Tam olarak i�inde degil");
        }
    }
    private bool TriggerCheck(BoxCollider triggerCollider, GameObject objectToCheck)
    {
        Bounds objectBounds = objectToCheck.GetComponent<Renderer>().bounds;

        // Trigger collider'�n bounds alan�
        Bounds triggerBounds = triggerCollider.bounds;

        // Nesnenin bounds'�n�n trigger bounds'� i�inde olup olmad���n� kontrol et
        if (triggerBounds.Contains(objectBounds.min) && triggerBounds.Contains(objectBounds.max))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void SpawnPerfectPlacementItem()
    {
        GameObject Spawnitem = Instantiate(PerfectPlacement, previousBlock.transform.position, Quaternion.identity);
        Spawnitem.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        Spawnitem.transform.SetParent(previousBlock.transform);
        Spawnitem.transform.localPosition = new Vector3(0f, -(previousBlock.transform.localScale.y / 2), 0f);
        var main = Spawnitem.GetComponent<ParticleSystem>().main;
        main.startColor = previousBlock.GetComponent<MeshRenderer>().materials[0].color;
    }
    public void Restart()
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
        GameManager.Instance.BaseCube.GetComponent<MeshRenderer>().material = randomMat[matIndex];
        CanTouch = true;
        SpawnBlock(false);
    }
    private void ResizeCurrent(GameObject currentBlock, GameObject previousBlock)
    {
        currentBlock.transform.localScale = previousBlock.transform.localScale;
        if(isHorizantal)
            currentBlock.transform.position = new Vector3(currentBlock.transform.position.x, currentBlock.transform.position.y, previousBlock.transform.position.z);
        else
            currentBlock.transform.position = new Vector3(previousBlock.transform.position.x,currentBlock.transform.position.y,currentBlock.transform.position.z);
    }
}
