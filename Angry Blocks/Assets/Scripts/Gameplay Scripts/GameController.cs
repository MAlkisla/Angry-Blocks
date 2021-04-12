using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public List<GameObject> levels;

    private GameObject level1;
    private GameObject level2;

    private Vector2 level1Pos;
    private Vector2 level2Pos;

    public GameObject[] block;

    private ShotCountText shotCountText;
    
    public int shotCount;
    public int ballsCount;

    public Text ballsCountText;

    private GameObject ballsContainer;
    public GameObject gameOver;

    void Awake()
    {
        shotCountText = GameObject.Find("ShotCountText").GetComponent<ShotCountText>();
        ballsCountText = GameObject.Find("BallCountText").GetComponent<Text>();
        ballsContainer = GameObject.Find("BallsContainer");
    }

    void Start()
    {
        PlayerPrefs.DeleteKey("Level");
        ballsCount = PlayerPrefs.GetInt("BallsCount", 5);
        ballsCountText.text = ballsCount.ToString();
        Physics2D.gravity = new Vector2(0, -17);
        SpawnLevel();
        GameObject.Find("Cannon").GetComponent<Animator>().SetBool("MoveIn", true);

    }

    void Update()
    {
        if (ballsContainer.transform.childCount == 0 && shotCount == 4)
        {
            gameOver.SetActive(true);
            GameObject.Find("Cannon").GetComponent<Animator>().SetBool("MoveIn", false);
        }
        CheckBlocks();
    }

    void SpawnNewLevel(int numberLevel1, int numberLevel2, int min, int max)
    {
        if (shotCount > 1)
        {
            Camera.main.GetComponent<CameraTransition>().RotateCameraToFront();
        }

        shotCount = 1;

        level1Pos = new Vector2(3.5f, 1f);
        level2Pos = new Vector2(3.5f, -3.4f);

        level1 = levels[numberLevel1];
        level2 = levels[numberLevel2];

        Instantiate(level1, level1Pos, Quaternion.identity);
        Instantiate(level2, level2Pos, Quaternion.identity);

        SetBlocksCount(min, max);
    }

    void SetBlocksCount(int min, int max)
    {
        block = GameObject.FindGameObjectsWithTag("Block");

        for (int i = 0; i < block.Length; i++)
        {
            int count = Random.Range(min, max);
            block[i].GetComponent<Block>().SetStartingCount(count);
        }
    }

    void SpawnLevel()
    {
        if (PlayerPrefs.GetInt("Level", 0) == 0)
            SpawnNewLevel(0, 17, 3, 5);

        if (PlayerPrefs.GetInt("Level") == 1)
            SpawnNewLevel(12, 21, 4, 6);

        if (PlayerPrefs.GetInt("Level") == 2)
            SpawnNewLevel(8, 18, 6, 8);

        if (PlayerPrefs.GetInt("Level") == 3)
            SpawnNewLevel(4, 14, 3, 5);

        if (PlayerPrefs.GetInt("Level") == 4)
            SpawnNewLevel(5, 13, 4, 6);

        if (PlayerPrefs.GetInt("Level") == 5)
            SpawnNewLevel(26, 30, 6, 8);

        if (PlayerPrefs.GetInt("Level") == 6)
            SpawnNewLevel(40, 44, 3, 5);

        if (PlayerPrefs.GetInt("Level") == 7)
            SpawnNewLevel(42, 22, 3, 5);

        if (PlayerPrefs.GetInt("Level") == 8)
            SpawnNewLevel(38, 28, 6, 8);

        if (PlayerPrefs.GetInt("Level") == 9)
            SpawnNewLevel(41, 16, 3, 5);

        if (PlayerPrefs.GetInt("Level") == 10)
            SpawnNewLevel(36, 26, 4, 6);
    }

    void CheckBlocks()
    {
        block = GameObject.FindGameObjectsWithTag("Block");

        if (block.Length < 1)
        {
            RemoveBalls();
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
            SpawnLevel();
        }
    }

    void RemoveBalls()
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        for (int i = 0; i < balls.Length; i++)
        {
            Destroy(balls[i]);
        }
    }

    public void CheckShotCount()
    {
        if (shotCount == 1)
        {
            shotCountText.SetTopText("SHOT");
            shotCountText.SetBottomText("1/3");
            shotCountText.Flash();
        }
        if (shotCount == 2)
        {
            shotCountText.SetTopText("SHOT");
            shotCountText.SetBottomText("2/3");
            shotCountText.Flash();
        }
        if (shotCount == 3)
        {
            shotCountText.SetTopText("FINAL");
            shotCountText.SetBottomText("SHOT");
            shotCountText.Flash();
        }
    }
}
