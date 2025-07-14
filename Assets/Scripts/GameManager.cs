using System;
using System.Collections;
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
        Load(); 
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
            GamePause();
        }
    }

    private void OnApplicationQuit()
    {
        GameSave();
    }
    
    public void LoadBtn() {
        SaveData saveData = LoadSystem.LoadGameData();

        if (SceneManager.GetActiveScene().name != saveData.sceneName.sceneName) {
            //SaveFile에 저장된 씬 네임이 다를경우 그쪽 씬을 로드해줘야 함.
            SceneManager.LoadScene(saveData.sceneName.sceneName);
        }

        Load();
    }
    
    public void Load() {
        SaveData saveData = LoadSystem.LoadGameData();
        
        if (saveData == null) {
            Debug.LogWarning("저장된 데이터 없음");
            return;
        }

        GameContinue();
        player.PlayerPosLoad();
        
        InventorySys inven = InventorySys.instance;
        inven.ResetQuickSlots();
        StartCoroutine(LoadAfterSceneLoad(saveData));
    }
    
    private IEnumerator LoadAfterSceneLoad(SaveData saveData) {
        yield return new WaitForSeconds(0.1f);

        InventorySys inven = InventorySys.instance;
        inven.LoadQuickSlotData(saveData.quickSlotData);
    }
}


public static class SaveSystem
{
    public const string FileName_SaveData = "/savedata.json";

    public static void SaveGameData()
    {
        string filePathSaveData = Application.persistentDataPath + FileName_SaveData;

        PlayerData playerData = new PlayerData(Player_Move.instance);
        SceneData sceneName = new SceneData();
        QuickSlotData quickSlotData = new QuickSlotData(InventorySys.instance.slots);

        SaveData saveData = new SaveData(playerData, sceneName, quickSlotData);

        string txt = JsonUtility.ToJson(saveData, true);// true로 설정하면 자동 줄 바꿈이 적용됨
        File.WriteAllText(filePathSaveData, txt);

        Debug.Log("저장완료 : " + filePathSaveData);
    }
}


[Serializable]
public class SaveData
{
    [SerializeField] public PlayerData playerData;
    [SerializeField] public SceneData sceneName;
    [SerializeField] public QuickSlotData quickSlotData;

    public SaveData(PlayerData playerData, SceneData sceneName, QuickSlotData quickSlotData)
    {
        this.playerData = playerData;
        this.sceneName = sceneName;
        this.quickSlotData = quickSlotData;
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
public class SceneData {
    [SerializeField] public string sceneName;
    
    public SceneData() {
        sceneName = SceneManager.GetActiveScene().name;
    }
}

[Serializable]
public class QuickSlotData {
    public List<string> itemNames;
    public List<int> itemCounts;
    
    public QuickSlotData(QuickSlot[] slots) {
        itemNames = new List<string>();
        itemCounts = new List<int>();
        
        foreach (var slot in slots) {
            if (slot.currentItem != null) {
                itemNames.Add(slot.currentItem.ItemName);
                itemCounts.Add(slot.currentItem.itemCount);
            }
            else {
                itemNames.Add("");
                itemCounts.Add(0);
            }
        }
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