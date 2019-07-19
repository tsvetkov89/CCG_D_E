using UnityEngine;
using System;
using InterfaceNamespace;

public class SaveManager : ISaveManager //
{
    private Save _sv = new Save();

    public bool CheckLoad()                 //проверим сохранение
    {
        if (!PlayerPrefs.HasKey("Save"))
        { 
             return false; 
        }
        else
        {
            _sv = JsonUtility.FromJson<Save>(PlayerPrefs.GetString("Save"));
            if (_sv.NameClass != "")
            {
                return true;
            }
            else
                return false;
        }
    }
    public void SaveClass(CardsGameClass cardGame)   //сохраняем игровой класс
    {
        
        _sv.NameClass = cardGame.cardName.ToString();
    }
    public void SaveInventory(string[] inv)        //сохраняем инвентарь игрока
    {
        _sv.Inventory = inv;
    }

    public void SaveGame()
    {
        PlayerPrefs.SetString("Save", JsonUtility.ToJson(_sv));
    }
    public void DeleteSave()
    {
        PlayerPrefs.DeleteAll();
    }
    [Serializable]
    public class Save
    {
        public string NameClass;    //название класса
        public int Id;              //ид класса
        public string[] Inventory;  //инвентарь игрока
    }
}
