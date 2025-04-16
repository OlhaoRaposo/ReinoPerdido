using System;
using TMPro;
using UnityEngine;
using DG.Tweening;
public class CodeLock : MonoBehaviour, IInteractable, ICameraLockable
{
    private Vector3 cameraView => transform.position + new Vector3(0,0,-.8f);
    
    [SerializeField] private string text;
    [SerializeField] private string code;
    [SerializeField] private string targetCode;
    [SerializeField] private int maxCodeDigits = 4;
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
                text += "I";
            else {
                if(text.Length > 0) {
                    if(text[text.Length - 1] == 'I') 
                        text = text.Remove(text.Length - 1);
                }
            }
            timer = 0;
        }
        panelText.text = text;
    }
    
    public void AddKey(string key) {
        if(code.Length >= maxCodeDigits) return;
        
        foreach (var c in text) {
            if(c == 'I') {
               int index = text.IndexOf("I");
               text = text.Remove(index,1);
            }
        }
        text += key;
        code += key;
    }
    public void ClearKeys() {
        text = "";
        code = "";
    }
    public void CheckPassword() {
        if(code == targetCode) {
            Debug.Log("Code unlocked");
        }else {
            AddError();
        }
    }
    private void AddError() {
    }
}
