using System.Collections.Generic;
using EnumNamespace;
using InterfaceNamespace;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "New Item", menuName = "CardEnemy")]

public class CardEnemy : ScriptableObject, ICard
{
    [Header("Главные атрибуты карты")]  [Space]
    [Tooltip("Подвид карты")] public CardType status;
    [Tooltip("Подтип карты")] public SubTypeCard subType;
    [Tooltip("Редкость карты")] public CardRarity rarity;
    [Tooltip("Спавнит юнитов")]public List<string> spawnUnits;
    [Header("Показатели карты")][Space]
    [TextArea(3, 5)][Tooltip("Описание карты")] public string description;
    [Tooltip("Название карты (код)")] public string cardName;
    [Tooltip("Название карты (игровое)")]public string displayCardName;
    [Tooltip("Арт карты")] public Sprite artCard;
    [Tooltip("Оконтовка карты")]  public Sprite shirtMain;//лицевая сторона (оконтовка)  
    [Tooltip("Рубашка карты")] public Sprite shirtCard;//лицевая сторона (оконтовка)

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
            nameCard.GetComponent<TMP_Text>().text = displayCardName;
            if (shirtC != null)
                shirtC.GetComponent<Image>().sprite = shirtCard;
            art = cardObject.GetComponentsInChildren<Transform>().SearchChild("SpriteArtCard");
            shirtM = cardObject.GetComponentsInChildren<Transform>().SearchChild("SpriteShirtMain");
            SpriteRenderer spriterender;
            spriterender = art.GetComponent<SpriteRenderer>();
            spriterender.sprite = artCard;
            art.gameObject.SetActive(false);
            spriterender = shirtM.GetComponent<SpriteRenderer>();
            spriterender.sprite = shirtMain;
            shirtM.gameObject.SetActive(false);
        }
        else
        {
            throw new UnityException("Null object (DisplayCardInGame)");
        }
    }
    public DataCard GetDataCard()
    {
        var typeCard = new DataCard {TypeCard = status, TypeSubCard = subType, 
            NameCard = cardName, DisplayNameCard = displayCardName, Rarity = rarity, SpawnUnits = spawnUnits};
        return typeCard;
    }
}

