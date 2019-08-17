using System.Collections.Generic;
using EnumNamespace;
using InterfaceNamespace;
using UnityEngine;
public class PopupInventory:  IPopupInventory, ISubscriber
{
    private IInventoryManager _inventoryManager;
    private IAnimaManager _animaManager;
    
    private GameObject _popupInventory;       //вспылывающее окно инвентаря
    
    private GameObject _holderInventory;           //панель под инвентарь
    private GameObject _holderCards;           //панель под слоты экиировки  

    private  bool _flagPopup;
    public PopupInventory(IInventoryManager inventoryManager, IAnimaManager animaManager)
    {
        _inventoryManager = inventoryManager;
        _animaManager = animaManager;
    }

    public void SetDependecies(GameObject popupInventory)
    {
        
        _popupInventory = popupInventory;
        _flagPopup = false;
    }
    //---------------действия на ивенты
    public void OnEvent(CustomEventArgs messageData)
    {
        var message = messageData.Message;
        switch (message)
        {
            case GameEventName.GoOpenInventory:
                if (!_flagPopup)
                {
                    _popupInventory.transform.SetAsLastSibling();
                    _popupInventory.SetActive(true);
                    _flagPopup = true;            
                }
                break;
            case GameEventName.GoCloseInventory:
                if(_flagPopup)
                {
                    _popupInventory.SetActive(false);
                    _flagPopup = false;
                }
                break;
        }
    }
    //--------------Ищем в инвентаре панели
    public void SearchAndSetHolders(List<GameObject> gameObjects)
    {   
        var holder = _popupInventory.GetComponentsInChildren<Transform>().SearchChild("PanelCards");     
        _holderCards = holder.gameObject;//найдем панель под cards
        holder = _popupInventory.GetComponentsInChildren<Transform>().SearchChild("PanelInventory");
        _holderInventory = holder.gameObject; //найдем панель под сумку*/
        _inventoryManager.SetHolders(_holderCards,_holderInventory, gameObjects);
        _inventoryManager.CreateSlotsInHolders();
    }
    
}