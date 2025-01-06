using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class PlayerAim : MonoBehaviour
{
    [SerializeField]private float minAngle = -45;
    [SerializeField]private float maxAngle = 45;
    [Range(0.01f,10)]
    [SerializeField]private float sensitivity = 12;
    [SerializeField]GameObject interactText;
    private Transform playerBody;
    [SerializeField]private float mouseX,mouseY;
    public static PlayerAim instance => FindObjectOfType<PlayerAim>();
    public bool isLocked { get; set; }
    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        playerBody = transform.parent;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        playerBody.Rotate(Vector3.up * transform.localEulerAngles.y);
    }

    private void Update() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);
        IInteractable firstInteractable = null;
        foreach (var collider in colliders) {
            var interactable = collider.GetComponent<IInteractable>();
            if (interactable != null) {
                firstInteractable = interactable;
                break; 
            }
        }
        interactText.SetActive(firstInteractable != null && !isLocked);
        
        if (Input.GetKeyDown(KeyCode.E) && firstInteractable != null) {
            firstInteractable.Interact();
            if(firstInteractable is ICameraLockable cameraLockable) {
                cameraLockable.LockCamera();
            }
        }
    }

    private void FixedUpdate(){
        if(isLocked) return;
        mouseX = Mathf.Lerp(mouseX,Mathf.Clamp(Input.GetAxisRaw("Mouse X"),-5,5) * (sensitivity * 100) * Time.deltaTime,0.35f);
        mouseY = Mathf.Lerp(mouseY,Mathf.Clamp(Input.GetAxisRaw("Mouse Y"),-5,5) * (sensitivity * 100) * Time.deltaTime,0.35f);       
        transform.Rotate(Vector3.left * mouseY);
        if(transform.localEulerAngles.x > 180 && transform.localEulerAngles.x < 360 + minAngle){
            transform.localEulerAngles = new Vector3(360 + minAngle,0,0);
        }else if(transform.localEulerAngles.x < 180 && transform.localEulerAngles.x > maxAngle){
            transform.localEulerAngles = new Vector3(maxAngle,0,0);
        }
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
