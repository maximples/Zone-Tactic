using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI different;
    public TextMeshProUGUI winText;
    public static GameManager Instance;
    public Material player1Material;
    public Material player2Material;
    public GameObject winPanel;
    public GameObject load;
    public GameObject baseEnemy;
    void Awake()
    {
        Instance = this;

    }
        void Start()
    {
        TechnologyOff();
        GameStat.player1money = 1600;
       // InvokeRepeating("SpawnEnemy", startDelay, spawnInterval);
    }
    private void TechnologyOff()
    {
        GameStat.player1Technology.heavyFactory = false;
        GameStat.player1Technology.heavyTank= false;
        GameStat.player1Technology.fireTank= false;
        GameStat.player1Technology.towerMulti = false;
        GameStat.player1Technology.RSZO = false;
        GameStat.player1Technology.repireTover = false;
    }
    public void Win(string text)
    {
        GameStat.activ = false;
        winText.text = text;
        winPanel.SetActive(true);
    }
    public void StartNew()
    {
        load.SetActive(true);
        SceneManager.LoadScene(1);
    }
    public void Restart()
    {
        SceneManager.LoadScene(1);
    }
    public void Lose()
    {
        GameStat.activ = false;
    }
    public void Difficulty()
    {
        SaveData.Instance.DifficultyCorrect(1);
        float diff = SaveData.Instance.difficultyLevel;
        if (diff == 1) { different.text = "Уровень сложности: лёгко"; }
        if (diff == 2) { different.text = "Уровень сложности: нормально"; }
        if (diff == 3) { different.text = "Уровень сложности: сложно"; }
    }
    public void Exit()
    {

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif

    }
}
