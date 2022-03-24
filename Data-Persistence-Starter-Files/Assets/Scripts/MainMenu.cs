using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.IO;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private int _finalPoints;
    private string _saveBestName;

    public Text textObj;

    private void Awake()
    {
        LoadGame();
        textObj.text = "Lider: " + _saveBestName + "; Best Scrore: " + _finalPoints;
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }

    void LoadGame()
    {
        string path = Application.persistentDataPath + "/save.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            MainManager.SaveFileNeed saveFileNeed = JsonUtility.FromJson<MainManager.SaveFileNeed>(json);
            _finalPoints = saveFileNeed.pointsSave;
            _saveBestName = saveFileNeed.saveName;
        }
    }
}
