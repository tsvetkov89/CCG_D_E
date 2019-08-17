using System.Collections.Generic;
using EnumNamespace;
using InterfaceNamespace;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "New Item", menuName = "TokenReward")]

public class TokenReward : ScriptableObject, IToken
{
    [Tooltip("Название токена")] public TokenRewardEnum token;
    [Tooltip("Использование")] public TokenUse useToken;
    [Tooltip("Арт токена")] public Sprite artToken;
    [TextArea(1, 5)][Tooltip("Описание токена")] public string description;
  
    public void DisplayToken<T>(T obj)where T : Object
    {
        var cardObject = obj as GameObject;
        if (cardObject != null)
        {
            var art = cardObject.GetComponentsInChildren<Transform>().SearchChild("SpriteArt");           
            var des = cardObject.GetComponentsInChildren<Transform>().SearchChild("Description");
            art.GetComponent<SpriteRenderer>().sprite = artToken;
            var descriptionText = des.GetComponent<TMP_Text>();
            descriptionText.text = description;
        }
        else
        {
            throw new UnityException("Null object (DisplayCardInGame)");
        }
    }

    public DataToken GetDataToken()
    {
        var data = new DataToken {Token =token, Use = useToken};
        return data;
    }
}

