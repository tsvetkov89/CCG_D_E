using System.Collections.Generic;
using UnityEngine;
using EnumNamespace;
/// <summary>
/// Класс для хранения строк
/// </summary>
public static class Strings
{
    private static IDictionary<SceneTypeEnum, string> _sceneNameStorage;
    private static IDictionary<GameClass, string> _descriptionStorage;
    private static IDictionary<ObjectTypeEnum, string> _stringsPaths;
    static Strings()
    {
        _sceneNameStorage = new Dictionary<SceneTypeEnum, string>();
        _descriptionStorage = new Dictionary<GameClass, string>();
        _stringsPaths = new Dictionary<ObjectTypeEnum, string>();
        InitSceneStorage();
        InitDescriptionStorage();
        InitPaths();
    }
    //-----------------Инициализация словаря имен игровых сцен
    private static void InitSceneStorage()
    {
        _sceneNameStorage.Add(SceneTypeEnum.Bootstrapper, "Bootstrapper");
        _sceneNameStorage.Add(SceneTypeEnum.Game, "Game");
        _sceneNameStorage.Add(SceneTypeEnum.Menu, "Menu");
    }
    //-----------------Инициализация словаря описаний игровых классов     //resx
    private static void InitDescriptionStorage()
    {
        _descriptionStorage.Add(GameClass.Red,"Старый Медведь - именно так называют Мстигора боевые товарищи. Его волосы тронула седина, а лицо испещрено морщинами, однако сила еще не покинула тело закаленного ветерана. Уже пятнадцать лет Мстигор служит в рядах личной гвардии доминуса Гормагона и заработал репутацию могучего воителя с поистине железной волей и великанским телосложением. Крепкие доспехи хранят своего хозяина от самых сильных ударов, а тяжелый двуручный меч Мстигора способен разрубить лошадь вместе с всадником надвое.");
        _descriptionStorage.Add(GameClass.Green,"Истово верующий в Порядок и воспитывающий в себе добродетели, Рихард является непревзойденным душегубом. В мирской жизни братья Рихарда по Ордо Модерати знают его как скромного и доброго человека, всегда готового помочь ближнему своему словом и делом. Сложно поверить, что этот щуплый добряк может обидеть даже муху, но когда дело касается его ремесла, Рихард становится воплощением смерти. Его родной дом - самые непроглядные тени, его лучшие друзья - остро заточенные парные клинки, вкусившие крови не одного десятка жертв.");
        _descriptionStorage.Add(GameClass.Blue, "Иордан Фламель происходит родом из древней знатной семьи и его ждало бы безоблачное будущее, если бы он только не родился маледиктом. Родители хотели скрыть природу юноши, но Фламель добровольно пришел в Имперо Маледиктум и, пройдя многочисленные проверки, начал свою службу на благо государства. Хоть Иордан молод, но его таланты и сила уже привлекают к себе внимание более опытных членов Имперо Маледиктум. Магосу Фламелю пророчат великое будущее, но юноше еще предстоит набраться опыта и пройти боевое крещение на поле боя.");
        _descriptionStorage.Add(GameClass.Purple, "");
    }
    //-----------------Инициализация словаря путей к ресурсам и префабам   
    private static void InitPaths()
    {
        _stringsPaths.Add(ObjectTypeEnum.PrefabIconPlayer, "Prefabs/prefabIconPlayer");
        _stringsPaths.Add(ObjectTypeEnum.PrefabCard, "Prefabs/prefabCard");
        _stringsPaths.Add(ObjectTypeEnum.PrefabCardInInventory, "Prefabs/prefabCardInInventory");
        _stringsPaths.Add(ObjectTypeEnum.PrefabCardArea, "Prefabs/prefabCardArea");
        _stringsPaths.Add(ObjectTypeEnum.PrefabCardEvent, "Prefabs/prefabCardEvent");
        _stringsPaths.Add(ObjectTypeEnum.PrefabCardEnemy, "Prefabs/prefabCardEnemy");
        _stringsPaths.Add(ObjectTypeEnum.PrefabCardBattleEnemy, "Prefabs/prefabCardBattleEnemy");
        
        _stringsPaths.Add(ObjectTypeEnum.SlotCardInInventory, "Prefabs/prefabSlotCardInInventory");
        _stringsPaths.Add(ObjectTypeEnum.SlotInventory2, "Prefabs/prefabSlotInventory2");
        _stringsPaths.Add(ObjectTypeEnum.CardsGameClass, "CardGameClass/");
        _stringsPaths.Add(ObjectTypeEnum.CardsArea, "CardGameArea/");  
        _stringsPaths.Add(ObjectTypeEnum.CardsEvents, "CardGameEvent/"); 
        _stringsPaths.Add(ObjectTypeEnum.CardsEnemy, "CardEnemy");
        _stringsPaths.Add(ObjectTypeEnum.CardsUnits, "Cards/Units/");
        
        _stringsPaths.Add(ObjectTypeEnum.CardsEquipment, "Cards/Equipment/");
        _stringsPaths.Add(ObjectTypeEnum.CardsSpells, "Cards/Spells/");
        _stringsPaths.Add(ObjectTypeEnum.CardsConsumables, "Cards/Consumables/");
        
        _stringsPaths.Add(ObjectTypeEnum.Adventure, "Adventure");
        _stringsPaths.Add(ObjectTypeEnum.PrefabBtnEvent, "Prefabs/prefabBtnEvent");
        
    }
    public static string GetScenePath(SceneTypeEnum sceneTypeTo)
    {
        string sceneName;
        var isExists = _sceneNameStorage.TryGetValue(sceneTypeTo, out sceneName);
        if (isExists == false)
        {
            throw new UnityException("It is unknown scene");
        }
        return sceneName;
    }
    public static string GetResoursePath(ObjectTypeEnum stringPath)
    {
        string pathString;
        var isExists = _stringsPaths.TryGetValue(stringPath, out pathString);
        if (isExists == false)
        {
            throw new UnityException("It is unknown path");
        }
        return pathString;
    }
    public static string GetDescriptionGameClass(GameClass gameClassType)
    {
        string description;
        var isExists = _descriptionStorage.TryGetValue(gameClassType, out description);
        if (isExists == false)
        {
            throw new UnityException("It is unknown game class");
        }
        return description;
    }
}