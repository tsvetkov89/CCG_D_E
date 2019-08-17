using UnityEngine;

namespace InterfaceNamespace
{
    public interface IConfigurateManager
    {
        void ConfigurateByParent(GameObject child, GameObject parent, float xmin, float ymin, float xmax, float ymax);
        void ConfigurateByParent(GameObject child, GameObject parent, bool active);
        void Configurate(GameObject child, GameObject parent, bool active, float xmin, float xmax, float ymin, float ymax);
        void ConfigurateCardByBattle(GameObject card);
        void ConfigurateSprite(GameObject obj, RectTransform rectTransform);
        void ConfigurateZero(GameObject obj);
        Transform GetPoolParent();
        void SetDependecies(GameObject obj, GameObject defaultCard);
    }
}