using System.Collections.Generic;
using EnumNamespace;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableCard : Card, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    
     public Transform ParentToReturnTo { private get; set; }
     private float _width;
     private float _height;
     private float _widthCard;
     private Vector2 _startPosition;
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
     public void SetDependecies(GameObject objStopDrag, GameObject objReturnCard)
     {
         _pointStopDrag = objStopDrag;
         _pointReturnCard = objReturnCard;
     }
     public void OnBeginDrag(PointerEventData eventData)
     {
         _statusCard = StatusCard.InHand;
         var parent = transform.parent;
         ParentToReturnTo = parent;
         transform.SetParent(parent.parent);
         transform.position = eventData.position;
         _startPosition = eventData.position;
         GetComponent<CanvasGroup>().blocksRaycasts = false;
     }
     public void OnDrag(PointerEventData eventData)
     {
         if (_statusCard == StatusCard.InHand)
         {
             var x = _startPosition.x - _width / 2;
             var y = eventData.position.y - _height / 2;
             transform.position = new Vector2(x, y);
         }else if (_statusCard == StatusCard.OnBoard)
         {
             if (!Pointer.instance.gameObject.activeSelf)
             {
                 Pointer.instance.Init(this);
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
     public void OnEndDrag(PointerEventData evenData)
     {
         _statusCard = StatusCard.InHand;
         transform.SetParent(ParentToReturnTo);
         GetComponent<CanvasGroup>().blocksRaycasts = true;
         
     } 
     
}
