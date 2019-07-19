using System.Collections;
using EnumNamespace;
using InterfaceNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopupDescriptionCard:  ISubscriber
{       
   private GameObject _popupDescription;    //вспылывающее окно
   private TMP_Text _fullDescription;
   private ICoroutiner _coroutiner;
   private IAnimaManager _animaManager;

   public PopupDescriptionCard(GameObject popup)
   {
       _popupDescription = popup;
       var description = _popupDescription.GetComponentsInChildren<Transform>().SearchChild("FullDescription");
       _fullDescription = description.GetComponent<TMP_Text>();
   }
   public void OnEvent(CustomEventArgs messageData)
   {
       var message = messageData.Message;
       switch (message)
       {
           case GameEventName.GoOpenPopupWithDescriptionCard:
           {
               var card = messageData.Value as ICard;
               _fullDescription.text = card.GetDataCard().FullDescription;
               _popupDescription.transform.SetAsLastSibling();
               _popupDescription.SetActive(true);
               _coroutiner.StartCoroutine(ViewDescriptionCard(0.6f));
               break;
           }
           case GameEventName.GoOpenPopupNoDescription:
               _popupDescription.transform.SetAsLastSibling();
               _popupDescription.SetActive(true);
               break;
           case GameEventName.GoClosePopupDescriptionCard:
               _animaManager.SetStateAnimation(_popupDescription, "popupCard", false);
               _coroutiner.StartCoroutine(ClosePopup(0.5f));
               break;
       }
   }
   public void SetDependecies( ICoroutiner coroutiner, IAnimaManager animaManager)
   {
       _coroutiner = coroutiner;
       _animaManager = animaManager;
   }

   private IEnumerator ViewDescriptionCard(float seconds)
   {
       yield return new WaitForSeconds(seconds);
       _animaManager.SetStateAnimation(_popupDescription,"popupCard", true);
   }
   private IEnumerator ClosePopup(float seconds)
   {
       yield return new WaitForSeconds(seconds);
       _popupDescription.SetActive(false);
   }
}
