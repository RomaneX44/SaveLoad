using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text bestScoreText;
    public GameObject GameOverText;
    public GameObject bestScorePanel;
    public InputField _bestNameInput;
    
    private bool m_Started = false;
    private int m_Points;
    public int _finalPoints;
    private string _saveBestName;


    private bool m_GameOver = false;


    // Start is called before the first frame update
    private void Awake()
    {
        LoadGame();
        
    }
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        if (_finalPoints != 0)
        {
            bestScoreText.text = "Champion Name: " + _saveBestName + ": "  + "Best Score: " + _finalPoints;
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        if (m_Points > _finalPoints)
        {
            _finalPoints = m_Points;
            bestScorePanel.SetActive(true);
        }
        m_GameOver = true;
        GameOverText.SetActive(true);
        
    }

    public void SetBestName()
    {
        _saveBestName = _bestNameInput.text;
        SaveGame();
    }

    public class SaveFileNeed
    {
        public int pointsSave;
        public string saveName;
    }

    private void SaveGame()
    {
        SaveFileNeed save = new SaveFileNeed();
        save.pointsSave = _finalPoints;
        save.saveName = _saveBestName;

        string json = JsonUtility.ToJson(save);
        File.WriteAllText(Application.persistentDataPath + "/save.json", json);
    }

    void LoadGame()
    {
        string path = Application.persistentDataPath + "/save.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveFileNeed saveFileNeed = JsonUtility.FromJson<SaveFileNeed>(json);
            _finalPoints = saveFileNeed.pointsSave;
            _saveBestName = saveFileNeed.saveName;
        }
    }
}
