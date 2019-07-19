using System.Collections.Generic;
using EnumNamespace;
using InterfaceNamespace;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "New Item", menuName = "CardUnit")]
public class CardUnit : ScriptableObject, ICard
{
    [Header("Главные атрибуты класса")]  [Space]
    [Tooltip("Главные атрибуты класса")]public List<StatsUnit> attribute;
    [Header("Описание класса")] [Space]
	
    [Tooltip("Подвид карты")] public CardType status;
    [Tooltip("Подтип карты")]public SubTypeCard subType;
    [Tooltip("Название карты (код)")] public string cardName;
    [Tooltip("Оконтовка карты")]  public Sprite shirtMain;//лицевая сторона (оконтовка) 
    [Tooltip("Арт карты")] public Sprite artCard;
    [System.Serializable] public class StatsUnit
    {
        public MainAttribute stat;
        public int value;
    }
    public void DisplayCardInGame<T>(T obj) where T : Object
    {
        var cardObject = obj as GameObject;
        if (cardObject != null)
        {
            var art = cardObject.GetComponentsInChildren<Transform>().SearchChild("ArtCard");
            var shirtM = cardObject.GetComponentsInChildren<Transform>().SearchChild("ShirtMain");
            art.GetComponent<Image>().sprite = artCard;
            shirtM.GetComponent<Image>().sprite = shirtMain;
        }
        else
        {
            throw new UnityException("Null object (DisplayCardInGame)");
        }
    }

    public DataCard GetDataCard()
    {
        var typeCard = new DataCard {TypeSubCard = subType, NameCard = cardName, Art = artCard, AttributeUnit = attribute};
        return typeCard;
    }
    
    
    
}

