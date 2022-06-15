using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SaveData : MonoBehaviour
{
    public static SaveData Instance;
    public float difficultyLevel = 2;
    // Start is called before the first frame update
    private void Awake()
    {

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void DifficultyCorrect(float correct)
    {
        difficultyLevel += correct;
        if (difficultyLevel > 3) { difficultyLevel = 1; }

    }

}