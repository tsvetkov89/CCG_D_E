

namespace InterfaceNamespace {

    public interface ISaveManager
    {
         void SaveClass(CardsGameClass cardGame);
         void SaveInventory(string[] inv);
         bool CheckLoad();
         void DeleteSave();
         void SaveGame();
    }
}