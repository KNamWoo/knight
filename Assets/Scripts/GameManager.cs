using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Overlays;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //save와 여러 전반적인 시스템 담당

    void GameStart() { }
    void GameSave()
    {
        //플레이어의 아이템의 개수와 여러가지 사항들을 저장
    }
    void GameLoad()
    {
        //
    }
    
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveSystem.SaveGameData();
        }
    }

    private void OnApplicationQuit()
    {
        SaveSystem.SaveGameData();
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
        SaveData saveData = new SaveData(playerData, invenData);

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

    public SaveData(PlayerData playerData, InvenData invenData)
    {
        this.playerData = playerData;
        this.invenData = invenData;
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