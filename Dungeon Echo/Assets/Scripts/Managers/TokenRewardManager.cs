using System.Collections.Generic;
using EnumNamespace;
using InterfaceNamespace;
using UnityEngine;

public class TokenRewardManager: ITokenRewardManager, ISubscriber
{
    private IPublisher _publisher;
    private IAnimaManager _animaManager;
    private readonly IObjectStorage _objectStorage;
    private readonly IConfigurateManager _configurateManager;
    private IDictionary<GameClass, List<GameObject>> _dictionaryIconToken;
    private IDictionary<GameClass, List<TokenRewardEnum>> _dictionaryToken;
    private List<GameObject> _poolIconToken;
    private GameObject _panelPlayers;
    private IToken _currentToken;
    private GameClass _currentPlayer;
    public TokenRewardManager(IPublisher publisher, IAnimaManager animaManager, IObjectStorage objectStorage, IConfigurateManager configurateManager)
    {
        _publisher = publisher;
        _animaManager = animaManager;
        _objectStorage = objectStorage;
        _configurateManager = configurateManager;
        _dictionaryIconToken = new Dictionary<GameClass, List<GameObject>>();
        _dictionaryToken = new Dictionary<GameClass, List<TokenRewardEnum>>();
    }
    public void OnEvent(CustomEventArgs messageData)
    {
        var message = messageData.Message;
        switch (message)
        {
            case GameEventName.GoUseAutoReward:
                var tokensReward = messageData.Value as List<TokenRewardEnum>;
                ProcessAutoUseTokens(tokensReward);
                break;
            case GameEventName.GoSelectTokenReward:
                _currentToken = messageData.Value as IToken;
                Debug.Log(_currentToken.GetDataToken().Token);
                FindEmptyCell();
                break;
            case GameEventName.GoAddTokenReward:
                _currentPlayer = (GameClass) messageData.Value;
                var token = _currentToken.GetDataToken().Token;
                var index = _dictionaryToken[_currentPlayer].IndexOf(TokenRewardEnum.Undefined);
                _dictionaryToken[_currentPlayer][index] = token;
                var art = _dictionaryIconToken[_currentPlayer][index].GetComponent<SpriteRenderer>();
                art.sprite = _currentToken.GetDataToken().Art;
                _publisher.Publish(null,new CustomEventArgs(GameEventName.GoEndSelectTokenReward,_currentToken));
                _currentToken = null;
                break;
            case GameEventName.GoRemoveTokenReward:
                break;
            case GameEventName.GoDeActivateTargetsPlayer:
                _currentToken = null;
                break;
            
        }
    }

    private void FindEmptyCell()
    {
        foreach (var dict in _dictionaryToken)
        {
            var index = dict.Value.IndexOf(TokenRewardEnum.Undefined);
            if (index != -1)
            {
                _publisher.Publish(null,new CustomEventArgs(GameEventName.GoActivateTargetPlayer, dict.Key));
            }
        }   
    }
    private void ProcessAutoUseTokens(IEnumerable<TokenRewardEnum> tokensReward)
    {
        foreach (var token in tokensReward)
        {
            switch (token)
            {
                case TokenRewardEnum.Pass:
                    Debug.Log("token: "+ token);
                    break;
                case TokenRewardEnum.RestoreHealth:
                    Debug.Log("token: "+ token);
                    break;
            }
        }
    }

    public void CreateIconTokenByGameClass(GameClass gameClass)
    {
        _dictionaryIconToken[gameClass] = new List<GameObject>();
        _dictionaryToken[gameClass] = new List<TokenRewardEnum>(){TokenRewardEnum.Undefined,TokenRewardEnum.Undefined,TokenRewardEnum.Undefined};
        var cout = 0;
        foreach (var icon in _poolIconToken)
        {
            if(icon.activeSelf) continue;
            if(cout == 3) return;
            _dictionaryIconToken[gameClass].Add(icon);
            icon.SetActive(true);
            ++cout;
        }
    }

    public void SetDependecies(GameObject panelPlayes)
    {
        _panelPlayers = panelPlayes;
        //----------------------------
        _poolIconToken = _objectStorage.GetPollObjects(ObjectTypeEnum.PrefabIconToken, 12);
        var art = _poolIconToken[0].GetComponent<Transform>(); 
        var refRect = art.GetComponent<RectTransform>();
        var count = 0;
        foreach (var icon in _poolIconToken)
        {
            _configurateManager.ConfigurateByParent(icon, _panelPlayers, false);
            var rectIcon = icon.GetComponent<RectTransform>();
            rectIcon.SetOffset(0,0,0,0);
            rectIcon.OffsetAnchorY(-0.0825f*count);
            var artSprite = icon.GetComponent<SpriteRenderer>(); 
            _configurateManager.ConfigurateSprite(artSprite.gameObject,refRect);
            ++count;
        }
    }
}
