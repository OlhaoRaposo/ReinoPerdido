using System.Collections;
using UnityEngine;

public class AimController : MonoBehaviour
{
    [Header("Bobbing")]
    [SerializeField]float frequency = 1.0f;
    [SerializeField]float amplitude = 2;
    [Header("Camera Settings")]
    [SerializeField]private float minAngle = -45;
    [SerializeField]private float maxAngle = 45;
    [Range(0.01f,10)]
    [SerializeField]private float sensitivity = 12;
    
    public static AimController instance => FindFirstObjectByType<AimController>();
    
    private Transform playerBody;
    private float mouseX, mouseY;
    void Start() { 
        Cursor.lockState = CursorLockMode.Locked;
        playerBody = transform.parent;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        playerBody.Rotate(Vector3.up * transform.localEulerAngles.y);
    }
    void FixedUpdate(){
        float bobbingStrenght = (Mathf.Sin(Time.time * frequency) * amplitude) * HasInput();
        
        mouseX = Mathf.Lerp(mouseX,Mathf.Clamp(Input.GetAxisRaw("Mouse X"),-5,5) * (sensitivity * 100) * Time.deltaTime,0.35f);
        mouseY = Mathf.Lerp(mouseY,Mathf.Clamp(Input.GetAxisRaw("Mouse Y"),-5,5) * (sensitivity * 100) * Time.deltaTime,0.35f);      
        
        transform.Rotate(Vector3.left * (mouseY + bobbingStrenght));
        if(transform.localEulerAngles.x > 180 && transform.localEulerAngles.x < 360 + minAngle){
            transform.localEulerAngles = new Vector3(360 + minAngle,0,0);
        }else if(transform.localEulerAngles.x < 180 && transform.localEulerAngles.x > maxAngle){
            transform.localEulerAngles = new Vector3(maxAngle,0,0);
        }
        playerBody.Rotate(Vector3.up * mouseX);
    }
    public IEnumerator PlayRunBobbing(){
        float bobbingStrenght = (Mathf.Sin(Time.time * frequency) * amplitude * .5f) * HasInput();
        transform.Rotate(Vector3.left * (mouseY + bobbingStrenght));
        yield return null;
    }
    float HasInput(){
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        if (x != 0 || y != 0){
            return 1;
        }
        //Lerps to 0
        return Mathf.Lerp(0,1,0.1f);
    }
}
