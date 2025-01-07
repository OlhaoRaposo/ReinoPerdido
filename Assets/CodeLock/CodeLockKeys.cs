using UnityEngine;
using DG.Tweening;
public class CodeLockKeys : MonoBehaviour,IClickable {
   [SerializeField] private string key;

   public void Click() {
      transform.DOMove(transform.position + Vector3.forward * 0.01f, 0.1f).OnComplete(() => transform.DOMove(transform.position - Vector3.forward * 0.01f, 0.1f));
      
      CodeLock codeLock = transform.GetComponentInParent<CodeLock>();
      codeLock.AddKey(key);
      if (key == "confirm" || key == "clear") {
         codeLock.ClearKeys();
      }
   }
}
