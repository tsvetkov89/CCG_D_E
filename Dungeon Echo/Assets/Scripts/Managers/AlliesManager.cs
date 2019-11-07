using System.Collections.Generic;
using EnumNamespace;
using InterfaceNamespace;
using UnityEngine;
using System.Collections;
using TMPro;

public class AlliesManager : IAlliesManager, ISubscriber
{
    private readonly ICoroutiner _coroutiner;
    private readonly IConfigurateManager _configurateManager;
    
    private GameObject _panelAllies;
    private List<GameObject> _listAllies;
    private IDictionary<GameObject, TextMeshProUGUI> _hpAlliesText;
    private IDictionary<GameObject, TextMeshProUGUI> _damageAlliesText;
    private IDictionary<GameObject, List<int>> _curAndMaxHpAllies;
    private IDictionary<GameObject, List<int>> _curAndMaxDamageAllies;
    public AlliesManager (ICoroutiner coroutiner, IConfigurateManager configurateManager)
    {
        _coroutiner = coroutiner;
        _configurateManager = configurateManager;
        _listAllies = new List<GameObject>();
        _hpAlliesText = new Dictionary<GameObject, TextMeshProUGUI>();
        _damageAlliesText = new Dictionary<GameObject, TextMeshProUGUI>();
        _curAndMaxHpAllies = new Dictionary<GameObject, List<int>>();
        _curAndMaxDamageAllies = new Dictionary<GameObject, List<int>>();
    }
    public void SetDependecies(GameObject panel)
    {
        _panelAllies = panel;
    }
    public void OnEvent(CustomEventArgs messageData)
    {
        var message = messageData.Message;
        switch (message)
        {
            case GameEventName.SpawnAllies:
            {
                var ally = messageData.Value as GameObject;
                SpawnAlly(ally);
                break;
            }
        }
    }
    private IEnumerator SwitchParent(GameObject obj, float pause)
    {
        yield return new WaitForSeconds(pause);
        _configurateManager.ConfigurateByParent(obj,_panelAllies,1,1,1,1);   
    }

    private void SpawnAlly(GameObject ally)
    {
        
        _listAllies.Add(ally);
        
        var componentEnemy =  ally.GetComponent<ActionsWithCards>();
        var child = ally.GetComponentsInChildren<Transform>().SearchChild("Hp");  
        var componentText = child.GetComponent<TextMeshProUGUI>();
        _hpAlliesText[ally] = componentText;
        child = ally.GetComponentsInChildren<Transform>().SearchChild("Damage");  
        componentText = child.GetComponent<TextMeshProUGUI>();
        _damageAlliesText[ally] = componentText;
        var attribute = componentEnemy.CardGame.GetDataCard().AttributeUnit;
        var hp = attribute[0];
        var damage = attribute[1];
        _curAndMaxHpAllies[ally] = new List<int>() {hp.value, hp.value};
        _curAndMaxDamageAllies[ally] = new List<int>() {damage.value, damage.value};
        InitBarsAlly(ally);
        _coroutiner.StartCoroutine(SwitchParent(ally, 0.6f));
    }
    private void InitBarsAlly(GameObject ally)
    {
        _hpAlliesText[ally].text = _curAndMaxHpAllies[ally][1].ToString();
        _damageAlliesText[ally].text = _curAndMaxDamageAllies[ally][1].ToString();
    }
}