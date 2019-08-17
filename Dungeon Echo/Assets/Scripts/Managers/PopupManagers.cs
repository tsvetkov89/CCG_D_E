using System.Collections;
using System.Collections.Generic;
using InterfaceNamespace;
using UnityEngine;

public class PopupManagers : IPopupManagers
{
    public PopupManagers(IPopupGameMenu popupGameMenu, IPopupInventory popupInventory, IPopupEvent popupEvent, IPopupPlayers popupPlayers,
        IPopupDescriptionCard popupDescriptionCard, IPopupPlaceInSlot popupPlaceInSlot, IPopupRewardEvent popupRewardEvent)
    {
        PopupGameMenu = popupGameMenu;
        PopupInventory =  popupInventory;
        PopupEvent = popupEvent;
        PopupPlayers = popupPlayers;
        PopupDescriptionCard = popupDescriptionCard;
        PopupPlaceInSlot = popupPlaceInSlot;
        PopupRewardEvent = popupRewardEvent;
    }
    public IPopupGameMenu PopupGameMenu { get; private set; }
    public IPopupInventory  PopupInventory{ get; private set; }
    public IPopupEvent PopupEvent{ get; private set; }
    public IPopupPlayers PopupPlayers{ get; private set; }
    public IPopupDescriptionCard PopupDescriptionCard{ get; private set; }
    public IPopupPlaceInSlot PopupPlaceInSlot{ get; private set; }
    public IPopupRewardEvent PopupRewardEvent { get; private set; }
}
