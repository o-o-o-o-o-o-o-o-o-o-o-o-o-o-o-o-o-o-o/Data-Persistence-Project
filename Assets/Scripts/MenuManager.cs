using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    public string playerName;

    [SerializeField] TextMeshProUGUI bestScore;
    [SerializeField] TMP_InputField inputField;

    [System.Serializable]
    class NameData
    {
        public string name;
    }
    
    private void Awake() {
        LoadData();
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        if (SceneManager.GetActiveScene().name == "menu")
        {
            playerName = inputField.text;
        }
    }

    public void StartGame() 
    {
        SaveName();
        SceneManager.LoadScene("main");
    }

    public void Quit() 
    {
        SaveName();
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif      
    }

    private void LoadData()
    {
        string scorePath = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(scorePath))
        {
            string json = File.ReadAllText(scorePath);
            MainManager.SaveData data = JsonUtility.FromJson<MainManager.SaveData>(json);

            bestScore.text = "Best Score : " + data.name + " : " + data.score;
        }

        string namePath = Application.persistentDataPath + "/name.json";
        if (File.Exists(namePath))
        {
            string json = File.ReadAllText(namePath);
            NameData data = JsonUtility.FromJson<NameData>(json);
            
            inputField.text = data.name;
        }
    }

    private void SaveName()
    {
        NameData data = new NameData();
        data.name = inputField.text;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/name.json", json);
    }

}
