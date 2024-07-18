using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using static UnityEditor.Progress;
using System;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    public PlayerData playerData;
    public List<Items> allItemsInventory;
    public List<string> idItemsDestroyed;

    private void Awake()
    {
        instance = this;
    }
    public void SaveGame()
    {
        // Convert item references to paths
        PlayerMovement.Instance.SaveDataPlayer(playerData);
        playerData.items = allItemsInventory;
        playerData.itemsDestroyed = idItemsDestroyed;

        string json = JsonUtility.ToJson(playerData);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        Debug.Log("Game saved");
    }

    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            playerData = JsonUtility.FromJson<PlayerData>(json);

            // Convert paths back to item references
            PlayerMovement.Instance.LoadDataPlayer(playerData);
            foreach(Items item in playerData.items)
            {
                InventoryManager.Instance.AddItem(item);
            }

            foreach (string item in playerData.itemsDestroyed)
            {
                idItemsDestroyed.Add(item);
            }
            Debug.Log("Game loaded");
        }
        else
        {
            Debug.Log("Save file not found");
        }
    }
    public void ClearJSONData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            allItemsInventory.Clear();
            idItemsDestroyed.Clear();
            string json = File.ReadAllText(path);

            playerData = JsonUtility.FromJson<PlayerData>(json);

            playerData = new PlayerData {level = 1, levelShield = 1, health = 100, stamina = 90, coin = 100 };

            PlayerMovement.Instance.LoadDataPlayer(playerData);

            json = JsonUtility.ToJson(playerData);

            File.WriteAllText(path, json);
        }
    }
}
