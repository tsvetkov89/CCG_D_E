using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;
using EnumNamespace;
using InterfaceNamespace;

//добавляем функцию создания в CreateAssetMenu 
[CreateAssetMenu(fileName = "New Card", menuName = "CardsGameClass")]
public class CardsGameClass : ScriptableObject, ICard
{
	
	[Header("Главные атрибуты класса")]  [Space]
	[Tooltip("Главные атрибуты класса")]public List<StatsGameClass> attribute;
	[Header("Описание класса")] [Space]
	
	[Tooltip("Подвид карты")] public CardType status;
	[Tooltip("Подтип карты")]public SubTypeCard subType;
	public GameClass cardName;
	public Sprite artCard;
	
	[System.Serializable] public class StatsGameClass
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
			art.GetComponent<Image>().sprite = artCard;
		}
		else
		{
			throw new UnityException("Null object (DisplayCardInGame)");
		}
	}

	public DataCard GetDataCard()
	{
		var typeCard = new DataCard {TypeSubCard = subType, GameClass = cardName, Art = artCard, Attribute = attribute};
		return typeCard;
	}
}

