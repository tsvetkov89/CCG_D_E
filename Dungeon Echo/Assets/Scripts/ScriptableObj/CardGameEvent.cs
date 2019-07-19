using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EnumNamespace;
using InterfaceNamespace;

//добавляем функцию создания в CreateAssetMenu 
[CreateAssetMenu(fileName = "New Card", menuName = "CardGameEvent")]
public class CardGameEvent : ScriptableObject, ICard
{
    [Tooltip("Подвид карты")] public CardType status;
    [Tooltip("Подтип карты")]public SubTypeCard subType;
    [Tooltip("Редкость карты")] public CardRarity rarity;
    
    [Tooltip("Название карты (код)")] public string cardName;
    [Tooltip("Название карты (в игре)")]public string displayCardName;
    [Tooltip("Арт карты")] public Sprite artCard;
    [Tooltip("Оконтовка карты")]  public Sprite shirtMain;//лицевая сторона
    [Tooltip("Рубашка карты")] public Sprite shirtCard;//лицевая сторона (оконтовка)
    
    public void DisplayCardInGame<T>(T obj) where T : Object
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
            nameCard.GetComponent<TMP_Text>().text = displayCardName;
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
        var typeCard = new DataCard {NameCard = cardName, DisplayNameCard = displayCardName, TypeCard = status,TypeSubCard = subType, Rarity = rarity};
        return typeCard;
    }
}