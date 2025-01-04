using UnityEngine;
public class PlayerAim : MonoBehaviour
{
    [SerializeField]private float minAngle = -45;
    [SerializeField]private float maxAngle = 45;
    [SerializeField]private float sensitivity = 100f;
    private Transform playerBody;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        playerBody = transform.parent;
    }

    void Update(){
      float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
      float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
      transform.Rotate(Vector3.left * mouseY);
      // Clamp the rotation of the camera
        Vector3 currentRotation = transform.localEulerAngles;
        if (currentRotation.x > 180) currentRotation.x -= 360;
        currentRotation.x = Mathf.Clamp(currentRotation.x, minAngle, maxAngle);
        transform.localRotation = Quaternion.Euler(currentRotation);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
