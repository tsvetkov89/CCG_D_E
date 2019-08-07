using System.Collections.Generic;
using EnumNamespace;
using InterfaceNamespace;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableCard : Card, IBeginDragHandler, IDragHandler
{
    private ITargetManager _targetManager;
    public Transform ParentToReturnTo { private get; set; }
    private float _width;
    private float _height;
    private float _widthCard;
    private GameObject _pointStopDrag;
    private GameObject _pointReturnCard;
    private StatusCard _statusCard;

    private void Awake()
    {
        _width = Screen.width;
        _height = Screen.height;
        var rect = GetComponent<RectTransform>();
        _widthCard = rect.rect.width + 10f;
    }

    public void SetDependecies(GameObject objStopDrag, GameObject objReturnCard, ITargetManager targetManager)
    {
        _pointStopDrag = objStopDrag;
        _pointReturnCard = objReturnCard;
        _targetManager = targetManager;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _statusCard = StatusCard.InHand;
        var parent = transform.parent;
        ParentToReturnTo = parent;
        transform.SetParent(parent.parent);
        transform.position = eventData.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        switch (_statusCard)
        {
            case StatusCard.InHand:
            {
                var x = eventData.position.x - _width / 2;
                var y = eventData.position.y - _height / 2;
                transform.position = new Vector2(x, y);
                break;
            }
            case StatusCard.OnBoard:
            {
                if (!_targetManager.PointerActive())
                {
                    _targetManager.PointerInit(this);
                }

                break;
            }
        }
        if (transform.position.y > _pointStopDrag.transform.position.y && _statusCard == StatusCard.InHand)
        {
            _statusCard = StatusCard.OnBoard;
        }
        // if (!(Pointer.instance.transform.position.y < _pointReturnCard.transform.position.y)) return;
        // _statusCard = StatusCard.InHand;
        //  Pointer.instance.Reset();


        //var vector3 = new Vector3(Input.mousePosition.x,Input.mousePosition.y, -Camera.main.transform.position.z);
        //Debug.Log(vector3);
        //transform.position = Camera.main.ScreenToWorldPoint(vector3); 
    }

    public void OnEndDrag()
    {
        _statusCard = StatusCard.InHand;
        transform.SetParent(ParentToReturnTo);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
