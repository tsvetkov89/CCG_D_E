namespace InterfaceNamespace
{
    public interface IPopupManagers
    {
        IPopupGameMenu PopupGameMenu { get; }
        IPopupInventory PopupInventory { get; }
        IPopupEvent PopupEvent { get; }
        IPopupPlayers PopupPlayers { get; }
        IPopupDescriptionCard PopupDescriptionCard { get; }
        IPopupPlaceInSlot PopupPlaceInSlot { get; }
        IPopupRewardEvent PopupRewardEvent { get; }
    }
}