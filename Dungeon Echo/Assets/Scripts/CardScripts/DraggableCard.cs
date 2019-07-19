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
     public void Awake()
     {
         _width = Screen.width;
         _height = Screen.height;
     }
     public void OnBeginDrag(PointerEventData eventData)
     {
         var parent = transform.parent;
         ParentToReturnTo = parent;
         transform.SetParent(parent.parent);
         transform.position = eventData.position;
         GetComponent<CanvasGroup>().blocksRaycasts = false;
     }
     public void OnDrag(PointerEventData eventData)
     {
         var x = eventData.position.x  - _width/2;
         var y = eventData.position.y  - _height/2;
         transform.position = new Vector2 (x,y);
     }
     public void OnEndDrag(PointerEventData evenData)
     {
         transform.SetParent(ParentToReturnTo);
         GetComponent<CanvasGroup>().blocksRaycasts = true;
     } 
}
