using InterfaceNamespace;
 using UnityEngine;
 
 public class Coroutiner : MonoBehaviour , ICoroutiner
 {
     private void Awake()
     {
         DontDestroyOnLoad(this); 
     }
 }