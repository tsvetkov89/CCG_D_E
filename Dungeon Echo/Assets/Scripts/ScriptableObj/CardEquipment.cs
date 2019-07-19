using System.Collections.Generic;
using EnumNamespace;
using InterfaceNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "CardEquipment")]

public class CardEquipment : ScriptableObject, ICard
{
    [Header("Главные атрибуты карты")]  [Space]
    [Tooltip("Подвид карты")]public CardType status;
    [Tooltip("Подтип карты")]public SubTypeCard subType;
    [Tooltip("Редкость карты")] public CardRarity rarity;
    [Tooltip("Принадлежность карты к классу")]public GameClass gameClass;
   
    [Header("Показатели карты")][Space]
    [Tooltip("Статистика")]public List<StatsEquipment>  statsAttribute; 
    
    [Header("Затраты на использование карты")][Space]
    [Tooltip("Атрибут Стамина/Мана")] public CostEquipment costAttribute; 
    [Header("Ценность карты")][Space] 
    [Tooltip("Ценовые  показатели")]public List<PriceEquipment> priceAttribute; 
    [Header("Эффекты  карты")][Space]
    [Tooltip("Эффект карты")]public List<EffectEquipment> effectAttribute; 
    [Header("Описание карты")] [Space]
    [TextArea(3, 5)][Tooltip("Описание карты")]public string description;
    [Tooltip("Название карты (код)")]public string cardName;
    [Tooltip("Название карты (игровое)")] public string displayCardName;
    [Tooltip("Арт карты")]public Sprite artCard;
    [Tooltip("Оконтовка карты")] public Sprite shirtMain;//лицевая сторона (оконтовка)  
    [Tooltip("Рубашка карты")] public Sprite shirtCard;//лицевая сторона (оконтовка)    

    [System.Serializable] public class PriceEquipment
    {
        public PriceEqupment stat;
        public int value;
    }
    [System.Serializable]public class StatsEquipment
    {
        public StatsEqupment stat;
        public float value;   
    }
    [System.Serializable] public class CostEquipment
    {
        public MainAttribute stat;
        public int value;
    }
    [System.Serializable] public class EffectEquipment
    {
        public EffectCard stat;
        public float value;
    }
    public void DisplayCardInGame<T>(T obj)where T : Object
    {
        var cardObject = obj as GameObject;
        if (cardObject != null)
        {
            var art = cardObject.GetComponentsInChildren<Transform>().SearchChild("ArtCard");
            var shirtM = cardObject.GetComponentsInChildren<Transform>().SearchChild("ShirtMain");
            var shirtC = cardObject.GetComponentsInChildren<Transform>().SearchChild("ShirtCard"); 
            var nameCard = cardObject.GetComponentsInChildren<Transform>().SearchChild("NameCard");
            art.GetComponent<Image>().sprite = artCard;
            shirtM.GetComponent<Image>().sprite = shirtMain;
            nameCard.GetComponent<TextMeshProUGUI>().text = displayCardName;
            if (shirtC != null)
                shirtC.GetComponent<Image>().sprite = shirtCard;
        }
        else
        {
            throw new UnityException("Null object (DisplayCardInGame)");
        }
    }
    public DataCard GetDataCard()
    {
        var typeCard = new DataCard {TypeCard = status, TypeSubCard = subType, 
            NameCard = cardName, DisplayNameCard = displayCardName,  Rarity = rarity};
        return typeCard;
    }
}
