using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
   static public Pointer instance;
   private DraggableCard _myCard;
   private Vector3 _startPosition;
   private Vector3[] _points;
   private LineRenderer _line;
   private Material _lineMaterial;
   private float _marerialLength;
   private float _materialSpeed;
   private void Awake()
   {
      gameObject.SetActive(false);
      instance = this;
      _line = GetComponent<LineRenderer>();
     
      _lineMaterial = _line.materials[0];
      _points = new Vector3[2];

      _materialSpeed = 0.015f;
      _marerialLength = 0.025f;
   }

   public void Init(DraggableCard myCard)
   {
      _myCard = myCard;
      _startPosition = new Vector3(myCard.transform.position.x,myCard.transform.position.y + 50f,myCard.transform.position.z);
      transform.position = _startPosition;
      _points[0] = _startPosition;
      gameObject.SetActive(true);
   }
   public void Reset()
   {    
      _startPosition = transform.position;
      transform.position = new Vector3(0,0,0);;
      _lineMaterial.mainTextureOffset = new Vector2(0, 0);
      gameObject.SetActive(false);
   }

   private void Update()
   {
      if (Input.GetMouseButtonUp(0))
         Reset();

      var vector3 = new Vector3(Input.mousePosition.x,Input.mousePosition.y, -Camera.main.transform.position.z);
      transform.position = Camera.main.ScreenToWorldPoint(vector3);
      
      transform.LookAt(_startPosition);
      
      _points[1] = transform.position;
      

      _line.SetPositions(_points);

      _lineMaterial.mainTextureScale = new Vector2(Vector2.Distance(_startPosition, transform.position)*_marerialLength,1);
      _lineMaterial.mainTextureOffset = new Vector2(_lineMaterial.mainTextureOffset.x - _materialSpeed, 0);
   }
}
