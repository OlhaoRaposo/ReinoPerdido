using System;
using TMPro;
using UnityEngine;
using DG.Tweening;
public class CodeLock : MonoBehaviour, IInteractable, ICameraLockable
{
    private Vector3 cameraView => transform.position + new Vector3(0,0,-.8f);
    
    [SerializeField] private string code;
    [SerializeField] private TextMeshProUGUI panelText;
    private float interval = 0.5f;
    private float timer = 0;
    private bool on;
    public void Interact() {
        Debug.Log("Code lock pressed");
    }
    public void LockCamera() {
        Debug.Log("Locking camera");
        if(!PlayerAim.instance.isLocked) {
            PlayerAim.instance.isLocked = true;
            PlayerMovement.instance.movementLocked = true;
            Cursor.lockState = CursorLockMode.Confined;
            Camera.main.transform.DOMove(cameraView, 1);
            Camera.main.transform.DOLookAt(transform.position + new Vector3(0,0,30), 1);
        }else {
            Cursor.lockState = CursorLockMode.Locked;
            PlayerMovement.instance.movementLocked = false;
            Camera.main.transform.DOMove(Camera.main.transform.parent.localPosition + new Vector3(0,1.5f,0), 1).OnComplete(() => PlayerAim.instance.isLocked = false);
            Camera.main.transform.DORotate(Camera.main.transform.parent.localEulerAngles, 1); 
        }
    }
    private void Update() {
        timer += Time.deltaTime;
        if (timer > interval) {
            on = !on;
            if(on)
                code += "I";
            else {
                if(code[^1] == 'I') 
                    code = code.Remove(code.Length - 1);
            }
            timer = 0;
        }
        panelText.text = code;
    }

}
