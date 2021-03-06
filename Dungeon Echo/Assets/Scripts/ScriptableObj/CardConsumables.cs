﻿using System.Collections.Generic;
using EnumNamespace;
using InterfaceNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "New Item", menuName = "CardConsumables")]

public class CardConsumables : ScriptableObject, ICard
{
    [Header("Главные атрибуты карты")]  [Space]
    [Tooltip("Подвид карты")] public CardType status;
    [Tooltip("Подтип карты")]public SubTypeCard subType;
    [Tooltip("Редкость карты")] public CardRarity rarity;
    
    [Header("Показатели карты")][Space]
    [Tooltip("На кого применяется данная карта")] public Membership application;
    [Tooltip("Эффект карты")]public List<EffectEqupment> effectAttribute; 
    [Header("Затраты на использование карты")][Space]
   
    [Tooltip("Атрибут Стамина/Мана")] public CostEqupment costAttribute;
    [Header("Описание карты")] [Space]
    [TextArea(1, 5)][Tooltip("Описание карты")] public string description;
    [TextArea(3, 5)][Tooltip("Полное описание карты")] public string fulldescription;
    [Tooltip("Название карты (код)")] public string cardName;
    [Tooltip("Название карты (игровое)")]public string displayCardName;
    [Tooltip("Арт карты")] public Sprite artCard;
    [Tooltip("Оконтовка карты")]  public Sprite shirtMain;//лицевая сторона (оконтовка)  
    [Tooltip("Рубашка карты")] public Sprite shirtCard;//лицевая сторона (оконтовка)
    [System.Serializable] public class CostEqupment
    {
        public MainAttribute stat;
        public int value;
    }
    [System.Serializable] public class EffectEqupment
    {
        public EffectCard stat;
        public float value;
        public int duration;
        public int cooldown;
    }
    [System.Serializable] public class StatsSpell
    {
        public EnumNamespace.StatsSpell stat;
        public int value;
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
            var descriptionCard= cardObject.GetComponentsInChildren<Transform>().SearchChild("DescriptionCard");
            var mana= cardObject.GetComponentsInChildren<Transform>().SearchChild("Mana");
            art.GetComponent<Image>().sprite = artCard;
            shirtM.GetComponent<Image>().sprite = shirtMain;
            nameCard.GetComponent<TMP_Text>().text = displayCardName;
            
            if (shirtC != null)
                shirtC.GetComponent<Image>().sprite = shirtCard;

            if (descriptionCard != null)
            {
                var descriptionText = descriptionCard.GetComponent<TMP_Text>();
                descriptionText.text = description;
                if (descriptionText.color.a == 0)
                    descriptionText.color = new Color32(255,255,255,255);
            }
            
            art = cardObject.GetComponentsInChildren<Transform>().SearchChild("SpriteArtCard");
            shirtM = cardObject.GetComponentsInChildren<Transform>().SearchChild("SpriteShirtMain");
            if (art)
            {
                var spriterender = art.GetComponent<SpriteRenderer>();
                spriterender.sprite = artCard;
                spriterender.size = Vector2.zero;
                //art.gameObject.SetActive(false);
            }
            if (!shirtM) return;
            {
                var spriterender = shirtM.GetComponent<SpriteRenderer>();
                spriterender.sprite = shirtMain;
                spriterender.size = Vector2.zero;
                //shirtM.gameObject.SetActive(false);
            }
            //----------------Мана на использование
            var manaText = mana.GetComponent<TMP_Text>();
            manaText.text = costAttribute.value.ToString();
            if (manaText.color.a == 0)
                manaText.color = new Color32(255,255,255,255);
        }
        else
        {
            throw new UnityException("Null object (DisplayCardInGame)");
        }
    }
    public DataCard GetDataCard()
    {
        var typeCard = new DataCard {TypeCard = status, TypeSubCard = subType, FullDescription = fulldescription, 
            NameCard = cardName, DisplayNameCard = displayCardName, Rarity = rarity , Application = application, Effect = effectAttribute};
        return typeCard;
    }
}

