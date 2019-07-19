using System.Collections;
using UnityEngine;
namespace InterfaceNamespace
{
   public interface ICoroutiner
   {
      Coroutine StartCoroutine(IEnumerator routine);
   }
}