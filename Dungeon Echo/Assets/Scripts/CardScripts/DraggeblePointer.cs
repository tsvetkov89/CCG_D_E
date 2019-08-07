
using EnumNamespace;
using InterfaceNamespace;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggeblePointer : MonoBehaviour
{
   private IPublisher _publisher;
   //static public DraggeblePointer instance;
   private Vector3 _startPosition;
   private Vector3[] _points;
   private LineRenderer _line;
   private Material _lineMaterial;
   private float _materialLength;
   private float _materialSpeed;
   private Camera _camera;
   private RaycastHit2D _ray;
   private RaycastHit2D _mainRay;
   private RaycastHit2D _rayPlayer;
   private LayerMask _mainLayer;
   private bool _targeting;
   private Membership _membership;
   private void Awake()
   {
      _camera = Camera.main;
      _line = GetComponent<LineRenderer>();
      _lineMaterial = _line.materials[0];
      _points = new Vector3[2];
      _materialSpeed = 0.015f;
      _materialLength = 0.015f;
           
   }
   public void SetDependecies(IPublisher publisher)
   {
      _publisher = publisher;
   }
   public void Init(DraggableCard draggableCard, Membership membership)
   {
      _membership = membership;
      _mainLayer = membership == Membership.Enemy ? 1 << LayerMask.NameToLayer("EnemyCard") : 1 << LayerMask.NameToLayer("IconPlayer");
      _targeting = false;
      _startPosition = new Vector3(draggableCard.transform.position.x, draggableCard.transform.position.y + 50f,
         draggableCard.transform.position.z);
      transform.position = _startPosition;
      _points[0] = _startPosition;
      gameObject.SetActive(true);
   }

   public void Reset()
   {
      _ray = Physics2D.GetRayIntersection(_camera.ScreenPointToRay(Input.mousePosition), float.PositiveInfinity, _mainLayer);
      if (_ray)
      {
         var targetObject = _ray.collider.gameObject;
         _publisher.Publish(this,
            _membership == Membership.Player
               ? new CustomEventArgs(GameEventName.TargetPlayer, targetObject)
               : new CustomEventArgs(GameEventName.TargetEnemy, targetObject));
         _publisher.Publish(this, new CustomEventArgs(GameEventName.TargetСapture, targetObject));

      }
      else
         _publisher.Publish(this, new CustomEventArgs(GameEventName.NonTargetСapture));
      
      _targeting = false;
      _publisher.Publish(this,
         _membership == Membership.Player
            ? new CustomEventArgs(GameEventName.NonTargetingPlayer)
            : new CustomEventArgs(GameEventName.NonTargetingСapture));
      
      _startPosition = transform.position;
      transform.position = new Vector3(0, 0, 0);
      _lineMaterial.mainTextureOffset = new Vector2(0, 0);
      gameObject.SetActive(false);
   }

   private void Update()
   {
      if (Input.GetMouseButtonUp(0))
         Reset();
      else
      {
         _mainRay = Physics2D.GetRayIntersection(_camera.ScreenPointToRay(Input.mousePosition),float.PositiveInfinity, _mainLayer);
         if (_mainRay)
         {
            var targetObject = _mainRay.collider.gameObject;
            _targeting = true;
            _publisher.Publish(this,
               _membership == Membership.Player
                  ? new CustomEventArgs(GameEventName.TargetingPlayer, targetObject)
                  : new CustomEventArgs(GameEventName.TargetingСapture, targetObject));
         }
         else if (_targeting)
         {
            _targeting = false;
            _publisher.Publish(this,
               _membership == Membership.Player
                  ? new CustomEventArgs(GameEventName.NonTargetingPlayer)
                  : new CustomEventArgs(GameEventName.NonTargetingСapture));
         }
      }

      var vector3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -_camera.transform.position.z);
      transform.position = _camera.ScreenToWorldPoint(vector3);
      transform.LookAt(_startPosition);
      _points[1] = transform.position;
      _line.SetPositions(_points);
      _lineMaterial.mainTextureScale =
         new Vector2(Vector2.Distance(_startPosition, transform.position) * _materialLength, 1);
      _lineMaterial.mainTextureOffset = new Vector2(_lineMaterial.mainTextureOffset.x - _materialSpeed, 0);
   }
}