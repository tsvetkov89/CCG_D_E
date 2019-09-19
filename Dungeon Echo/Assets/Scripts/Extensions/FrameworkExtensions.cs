using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///  Методы расширения
/// </summary>
public static partial class FrameworkExtensions
{
    //----------------усnтанавливаю RectTransform (Left,Right,Top,Bottom)
    public static void SetOffset(this RectTransform rect, float left, float bottom, float right, float top)
    {
        rect.offsetMin = new Vector2 (left,bottom);
        rect.offsetMax = new Vector2 (right,top);
    }
    //----------------устанавливаю MinMaxRect(float , float , float , float );
    public static void SetRect(this RectTransform rect, float xmin, float ymin, float xmax, float ymax)
    {
        rect.anchorMax = new Vector2 (xmax,ymax);
        rect.anchorMin = new Vector2 (xmin,ymin);
    }
    public static void OffsetAnchorY(this RectTransform rect, float offset)
    {
        rect.anchorMax = new Vector2 (rect.anchorMax.x,rect.anchorMax.y+offset);
        rect.anchorMin = new Vector2 (rect.anchorMin.x,rect.anchorMin.y +offset);
    }
    //-----------------установить альфу картинки
    public static void SetAlpha(this Image r, float alpha)
    {
        var c = r.color;
        r.color = new Color(c.r,c.g,c.b, alpha);
    }
    //----------------поиск CHild в иерархии обьекта по имени  //linq
    public static Transform SearchChild(this IEnumerable<Transform> allChildren, string nameChild)
    {
        return allChildren.FirstOrDefault(child => child.name == nameChild);
    }
    //----------------опрелеляем есть ли у обьекта компонет
    public static bool HasComponent<T> (this GameObject obj) 
    { 
        return obj.GetComponent(typeof(T)) != null; 
    }
    public static void SetSizeSprite(this SpriteRenderer sprite, RectTransform rect)
    {
        sprite.size = new Vector2(rect.rect.width,rect.rect.height);
    }
    public static void SetSizeBox2D(this BoxCollider2D box, RectTransform rect) 
    { 
        box.size = new Vector2(rect.rect.width,rect.rect.height);
    }
    public static void SetSizeBox2D(this BoxCollider2D box, RectTransform rect, float reduceX,float reduceY) 
    { 
        box.size = new Vector2(rect.rect.width-reduceX,rect.rect.height-reduceY);
    }
    /*public static void SetSize(this RectTransform trans, Vector2 newSize) {
        var oldSize = trans.rect.size;
        var deltaSize = newSize - oldSize;
        trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
        trans.offsetMax = trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
    }
    public static void SetWidth(this RectTransform trans, float newSize) {
        SetSize(trans, new Vector2(newSize, trans.rect.size.y));
    }
    public static void SetHeight(this RectTransform trans, float newSize) {
        SetSize(trans, new Vector2(trans.rect.size.x, newSize));
    }*/
}  
