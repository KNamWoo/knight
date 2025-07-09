using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject PauseWindow;
    public GameObject PlayerPrefab;
    public GameObject quickSlot;
    public bool currentPause;

    public Vector3 playerPosition;
    public Vector3 playerScale;

    Player_Move player;
    //save와 여러 전반적인 시스템 담당

    private void Awake() {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        currentPause = false;
    }
    private void Start() {
        PauseWindow.SetActive(false);

        //Instantiate(PlayerPrefab, playerPosition, Quaternion.identity);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Move>();
        //Instantiate(quickSlot, playerPosition, default);
        //currentPause = false;

        //PlayerPrefab.SetActive(true);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (currentPause) {
                GameContinue();
            }
            else {
                GamePause();
            }
        }
    }
    
    public void GamePause() {
        PauseWindow.SetActive(true);
        currentPause = true;
        Time.timeScale = 0f;
    }
    public void GameContinue() {
        PauseWindow.SetActive(false);
        currentPause = false;
        Time.timeScale = 1f;
        player.canPlay = false;    
    }
    public void GameSettings() {
        //SettingWindow.SetActive(true);
    }
    public void LoadInvenSave() {
        Inventory inventory = Inventory.instance;
        ItemManager itemMan = ItemManager.instance;
        SaveData saveData = LoadSystem.LoadGameData();

        if (saveData != null)
        {
            Debug.Log("파일을 찾음");
            
            for (int i = 0; i<inventory.maxSlot; i++) {
                if (saveData.invenData.ItemNames[i] == "")
                {
                    continue;
                }
                itemMan.ItemAdd(saveData.invenData.ItemNames[i], i, saveData.invenData.ItemCounts[i]);
            }
        }
    }
    
    public void PlayerPosSet() {
        SaveData saveData = LoadSystem.LoadGameData();
        
        if (saveData != null) {
            Debug.Log("파일을 찾음");
            playerPosition = saveData.playerData.positions[0];
            playerScale = saveData.playerData.scales[0];
        }
    }

    public void GameSave() {
        SaveSystem.SaveGameData();
    }

    public void Quit() {
        GameSave();
        Application.Quit();
    }
    
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            GameSave();
        }
    }

    private void OnApplicationQuit()
    {
        GameSave();
    }
    
    public void Load() {
        SaveData saveData = LoadSystem.LoadGameData();

        GameContinue();
        SceneManager.LoadScene(saveData.sceneName.sceneName);    
    }
}


public static class SaveSystem
{
    public const string FileName_SaveData = "/savedata.json";

    public static void SaveGameData()
    {
        string filePathSaveData = Application.persistentDataPath + FileName_SaveData;

        PlayerData playerData = new PlayerData(Player_Move.instance);
        InvenData invenData = new InvenData(Inventory.instance);
        SceneData sceneName = new SceneData();
        SaveData saveData = new SaveData(playerData, invenData, sceneName);

        string txt = JsonUtility.ToJson(saveData, true);// true로 설정하면 자동 줄 바꿈이 적용됨
        File.WriteAllText(filePathSaveData, txt);

        Debug.Log("저장완료 : " + filePathSaveData);
    }
}


[Serializable]
public class SaveData
{
    [SerializeField] public PlayerData playerData;
    [SerializeField] public InvenData invenData;
    [SerializeField] public SceneData sceneName;

    public SaveData(PlayerData playerData, InvenData invenData, SceneData sceneName)
    {
        this.playerData = playerData;
        this.invenData = invenData;
        this.sceneName = sceneName;
    }
}

[Serializable]
public class PlayerData
{
    //public Vector3 position; // 한 오브젝트만 저장해도 될 경우
    [SerializeField] public List<Vector3> positions; // 여러 오브젝트를 저장해야 할 경우
    [SerializeField] public List<Vector3> scales;

    public PlayerData(Player_Move playerMove)
    {
        //position = playerMove.transform.position; // 한 오브젝트만 저장해도 될 경우
        positions = playerMove.Player.Select(Object => Object.transform.position).ToList(); // 여러 오브젝트를 저장해야 할 경우
        scales = playerMove.Player.Select(Object => Object.transform.localScale).ToList();
    }
}

[Serializable]
public class InvenData
{
    [SerializeField] public List<String> ItemNames;
    [SerializeField] public List<int> ItemCounts;

    public InvenData(Inventory inven)
    {
        ItemNames = inven.slots.Select(Object => Object.itemName).ToList();
        ItemCounts = inven.slots.Select(Object => Object.itemCount).ToList();
    }
}

[Serializable]
public class SceneData {
    [SerializeField] public string sceneName;
    
    public SceneData() {
        sceneName = SceneManager.GetActiveScene().name;
    }
}

public static class LoadSystem
{
    public static SaveData LoadGameData()
    {
        Debug.Log("로드 중");
        try
        {
            string filePath = Application.persistentDataPath + SaveSystem.FileName_SaveData;
            string fileContent = File.ReadAllText(filePath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(fileContent);
            Debug.Log("로드 파일 경로 : " + filePath);
            Debug.Log("로드완료");
            return saveData;
        }
        catch
        {
            return null;
        }
    }
}