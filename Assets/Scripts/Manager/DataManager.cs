using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance { get; private set; }

    [SerializeField] private string characterPath;
    [SerializeField] private string itemPath;
    [SerializeField] private List<Character_SO> CSO = new();
    [SerializeField] private List<Item_SO> ISO = new();

    void Awake()
    {
        if (instance == null) 
        { 
            instance = this; 
            DontDestroyOnLoad(gameObject); 
        }
        else if (instance != this) 
        { 
            Destroy(gameObject); 
            return; 
        }

        LoadAll<Character_SO>(characterPath, CSO);
        LoadAll<Item_SO>(itemPath, ISO);
    }

    private void LoadAll<T>(string path, List<T> list) where T : ScriptableObject
    {
        list.Clear();
        var loaded = Resources.LoadAll<T>(path);
        list.AddRange(loaded);
        Debug.Log($"[DataManager] {typeof(T).Name} {loaded.Length}°³ ·ÎµåµÊ ({path})");
    }

    public Character_SO GetCharacterById(int wantedId)
    {
        return CSO.Find(c => c.id == wantedId);
    }

    public Character_SO GetRandomCharacterByStar(int wantedStar)
    {
        var pool = CSO.FindAll(c => c.star == wantedStar);
        if (pool.Count == 0) return null;
        return pool[Random.Range(0, pool.Count)];
    }

    public Item_SO GetItemById(int wantedId)
    {
        return ISO.Find(i => i.id == wantedId);
    }

    public Item_SO GetRandomItemByStar(int wantedStar)
    {
        var pool = ISO.FindAll(i => i.star == wantedStar);
        if (pool.Count == 0) return null;
        return pool[Random.Range(0, pool.Count)];
    }
}
