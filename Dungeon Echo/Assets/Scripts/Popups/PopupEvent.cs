using System.Collections;
using System.Collections.Generic;
using EnumNamespace;
using InterfaceNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupEvent : ISubscriber
{
    private IObjectStorage _objectStorage;
    private IPublisher _publisher;
    private IAnimaManager _animaManager;
    private ICoroutiner _coroutiner;
    private IConfigurateManager _configurateManager;
    private readonly GameObject _popupEvent;
    private Adventure _adventure;
    private TMP_Text _title;
    private TMP_Text _description;
    private SpriteRenderer _firstArt;
    private SpriteRenderer _secondArt;
    private GameObject _panelButtons1;
    private GameObject _panelButtons2;
    private GameObject _btnSkip;
    private List<GameObject> _poolButtons;
    private bool _printDescription;
    private bool _skipPrintDescription;
    private float _firstPause;
    private float _secondPause;
    
    public PopupEvent(GameObject popupEvent)
    {
        _popupEvent = popupEvent;
        var child = _popupEvent.GetComponentsInChildren<Transform>().SearchChild("TitleAdventure");
        _title = child.GetComponent<TMP_Text>();
        child = _popupEvent.GetComponentsInChildren<Transform>().SearchChild("Description");
        _description = child.GetComponent<TMP_Text>();
        _panelButtons1 = _popupEvent.GetComponentsInChildren<Transform>().SearchChild("PanelButtons_1").gameObject;
        _panelButtons2 = _popupEvent.GetComponentsInChildren<Transform>().SearchChild("PanelButtons_2").gameObject;
        child = _popupEvent.GetComponentsInChildren<Transform>().SearchChild("SpriteArtFirst");
        _firstArt = child.GetComponent<SpriteRenderer>();
        child = _popupEvent.GetComponentsInChildren<Transform>().SearchChild("SpriteArtSecond");
        _secondArt = child.GetComponent<SpriteRenderer>();
        _btnSkip = _popupEvent.GetComponentsInChildren<Transform>().SearchChild("btnSkip").gameObject;
        _btnSkip.SetActive(false);
        _printDescription = true;
        _skipPrintDescription = false;
    }
    public void SetDependecies( IObjectStorage objectStorage, IPublisher publisher, IAnimaManager animaManager,
        ICoroutiner coroutiner, IConfigurateManager configurateManager)
    {
        _objectStorage = objectStorage;
        _publisher = publisher;
        _animaManager = animaManager;
        _coroutiner = coroutiner;
        _configurateManager = configurateManager;
        _poolButtons = _objectStorage.GetPollObjects(ObjectTypeEnum.PrefabBtnEvent, 4);
    }
    public void OnEvent(CustomEventArgs messageData)
    {
        var message = messageData.Message;
        switch (message)
        {
            case GameEventName.GoStartEvent:
                _adventure = messageData.Value as Adventure;
                _popupEvent.transform.SetAsLastSibling();
                DisplayEvent();
                _popupEvent.SetActive(true);
                _animaManager.SetStateAnimation(_popupEvent,"fading",true);
                break;
        }  
    }
    private void DisplayEvent()
    {
        _firstPause = 0.04f;
        _secondPause = 0.4f;
        var data = _adventure.GetDataEvent();
        _title.text = data.NameEvent;
        _coroutiner.StartCoroutine(DisplayDescription(data.Description, 0.5f));
        _firstArt.sprite = data.Art;
        var events = data.Events;
        var count = 1;
        foreach (var @event  in events)
        {
            foreach (var btn in _poolButtons)
            {
               
                if (btn.activeSelf) continue; 
                var child = btn.GetComponentsInChildren<Transform>().SearchChild("Text");
                var btnText = child.GetComponent<TMP_Text>();
                btnText.text = @event.label;
                var button = btn.GetComponent<Button>();
                Debug.Log("event = " + @event.label);
                button.onClick.AddListener(delegate {OnClickButtonEvent(@event); });
                _configurateManager.ConfigurateByParent(btn, count <= 2 ? _panelButtons1 : _panelButtons2, true);
                count++;
                break;
            }  
        }
        _skipPrintDescription = false;
        var component = _btnSkip.GetComponent<Button>();
        component.onClick.AddListener(OnClickButtonSkip);
    }
    private IEnumerator DisplayDescription(string text, float pause)
    {
        _firstPause = 0.04f;
        _secondPause = 0.4f;
        _description.text = "";
        yield return new WaitForSeconds(pause);
        _printDescription = true;
        foreach (var t in text)
        {
            _description.text = _description.text + t;
            if (t == '.')
                yield return new WaitForSeconds(_secondPause);
            else
                yield return new WaitForSeconds(_firstPause);
            if (_description.text.Length == 22)
            {
                var component = _btnSkip.GetComponent<Button>();
                _btnSkip.SetActive(true);
            }
            if(!_printDescription) break;
        }
    }

    private void OnClickButtonEvent(Adventure.Event @event)
    {
        _printDescription = false;
        var outcomes = @event.outcomes;
        //var arrayOutcome= new[] {@event.goodOutcome,@event.badOutcome};
        var outcome = outcomes[RandomExtensions.GetRandomElementDictionary(DropChance.ChanceOutcome)];

        _coroutiner.StartCoroutine(DisplayDescription(outcome.description, 0.1f));
        _secondArt.sprite = outcome.art;
        _animaManager.SetStateAnimation(_popupEvent,"result",true);
        //var data = _adventure.GetDataEvent();
        //var events = data.Events;
        foreach (var btn in _poolButtons)
        {
            if (!btn.activeSelf) continue;
            var button = btn.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            btn.SetActive(false);
        }
        foreach (var btn in _poolButtons)
        {
            if (btn.activeSelf) continue;
            var child = btn.GetComponentsInChildren<Transform>().SearchChild("Text");
            var btnText = child.GetComponent<TMP_Text>();
            btnText.text = "Продолжить";
            var button = btn.GetComponent<Button>();
            button.onClick.AddListener(OnClickButtonEndEvent);
            _configurateManager.ConfigurateByParent(btn, _panelButtons2,true);
            break;
        }
    }

    private void OnClickButtonEndEvent()
    {
        foreach (var btn in _poolButtons)
        {
            if (!btn.activeSelf) continue;
            var button = btn.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
        }
        _animaManager.SetStateAnimation(_popupEvent,"end",true);
        _coroutiner.StartCoroutine(SayEndEvent(1f));
    }
    private void OnClickButtonSkip()
    {
        _btnSkip.SetActive(false);
        _firstPause = 0f;
        _secondPause = 0f;
    }

    private IEnumerator SayEndEvent(float pause)
    {
        yield return new WaitForSeconds(pause);
        foreach (var btn in _poolButtons)
        {
            if (!btn.activeSelf) continue;
            var button = btn.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            btn.SetActive(false);
        }
        _publisher.Publish(null, new CustomEventArgs(GameEventName.GoEndRandomEvent));
        _popupEvent.SetActive(false);
    }
}