using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public BlockPlacer placer;
    public GameObject SpawnObj;
    public GameObject BaseCube;
    public GameObject ScreenClickObj;
    [Space]
    public TextMeshProUGUI RestartCounter;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI GameTimerText;
    [Space]
    public float StartTime = 30;
    private float CurrentTime = 30;
    private int Score = 0;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }

    private void Update()
    {
        if(CurrentTime != 0)
        {
            StartTime -= Time.deltaTime;
            CurrentTime = Mathf.FloorToInt(StartTime);
            GameTimerText.text = "Timer: " + Mathf.FloorToInt(StartTime).ToString();
        }
        else if(CurrentTime <= 0)
        {
            EndGame();
        }
    }
    private void EndGame()
    {
        placer.CanTouch = false;
    }
    public void IncraseScore()
    {
        Score += 1;
        ScoreText.text = "Score: " + Score.ToString();
    }
    public void ScreenClick()
    {
        ScreenClickObj.SetActive(false);
    }
    public IEnumerator ResetCounter(BlockPlacer placer)
    {
        RestartCounter.gameObject.SetActive(true);
        RestartCounter.text = "2";
        yield return new WaitForSeconds(1);
        RestartCounter.text = "1";
        yield return new WaitForSeconds(1);
        RestartCounter.text = "GO";
        yield return new WaitForSeconds(0.2f);
        RestartCounter.gameObject.SetActive(false);
        placer.SpawnBlock();
    }
}
