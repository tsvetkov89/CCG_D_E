 using InterfaceNamespace;
 using UnityEngine;

 public class ConfigurateManager : IConfigurateManager
 {
     private GameObject _poolParent;
     private GameObject _defaultCard;
     private RectTransform _rectArt;
     private RectTransform _rectShirtMain;

     public void SetDependecies(GameObject obj, GameObject defaultCard)
     {
         _poolParent = obj;
         _defaultCard = defaultCard;
         var art = _defaultCard.GetComponentsInChildren<Transform>().SearchChild("ArtCard");
         var shirtM = _defaultCard.GetComponentsInChildren<Transform>().SearchChild("ShirtMain");
         _rectArt = art.GetComponent<RectTransform>();
         _rectShirtMain = shirtM.GetComponent<RectTransform>();
         _defaultCard.SetActive(false);
     }
     
      public void ConfigurateByParent( GameObject child,GameObject parent,float xmin,float ymin,float xmax,float ymax)
    {
        child.transform.SetParent(parent.transform);
        child.GetComponent<RectTransform>().SetRect(xmin, ymin, xmax, ymax);
        child.GetComponent<RectTransform>().SetOffset(0,0,0,0);  
        child.SetActive(true);
    }
    public void ConfigurateByParent( GameObject child,GameObject parent, bool active)
    {
        child.transform.SetParent(parent.transform);
        child.SetActive(active);
    }
    public void ConfigurateZero( GameObject obj)
    {
        obj.transform.SetParent(_poolParent.transform);
        obj.GetComponent<RectTransform>().SetRect(0, 0, 1, 1);
        obj.GetComponent<RectTransform>().SetOffset(0,0,0,0);
        obj.SetActive(false);
    }

    public Transform GetPoolParent()
    {
        return _poolParent.transform;
    }

    public void Configurate( GameObject child,GameObject parent, bool active, 
        float xmin,float xmax,float ymin,float ymax)
    {
        child.transform.SetParent(parent.transform);
        child.GetComponent<RectTransform>().SetRect(xmin,ymin,xmax,ymax);
        child.GetComponent<RectTransform>().SetOffset(0,0,0,0);
        child.SetActive(active);
    }
    public void ConfigurateCardByBattle(GameObject card)
    {
        var art = card.GetComponentsInChildren<Transform>().SearchChild("SpriteArtCard");
        var shirtM = card.GetComponentsInChildren<Transform>().SearchChild("SpriteShirtMain");
        var spriterender = art.GetComponent<SpriteRenderer>();
        spriterender.SetSizeSprite(_rectArt);
        spriterender = shirtM.GetComponent<SpriteRenderer>();
        spriterender.SetSizeSprite(_rectShirtMain);
    }

    public void ConfigurateSprite(GameObject obj, RectTransform rectTransform)
    {
        var spriterender = obj.GetComponent<SpriteRenderer>();
        spriterender.SetSizeSprite(rectTransform);
    }
 }
