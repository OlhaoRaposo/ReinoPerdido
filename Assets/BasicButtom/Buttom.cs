using UnityEngine;
using DG.Tweening;

public class Buttom : MonoBehaviour, IInteractable
{
    //[SerializeField] private float rotation;
    public void Interact()
    {
        Debug.Log("Button pressed");
        transform.DOMove(transform.position + Vector3.forward * -0.1f, 0.1f).OnComplete(() => transform.DOMove(transform.position + Vector3.forward * 0.1f, 0.1f));
        
        //Screw
        //transform.DORotate(new Vector3(0, 0, rotation), 3, RotateMode.FastBeyond360).SetEase(Ease.Linear);
        //transform.DOMove(transform.position + Vector3.forward * -0.5f, 3);
    }
   
}
