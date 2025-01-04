using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]private float speed = 12f;
    private CharacterController controller => GetComponent<CharacterController>();
    
    void Update(){
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
        //Gravity handler
        if (!controller.isGrounded) {
            controller.Move(Vector3.down * 9.81f * Time.deltaTime);
        }
    }
}
