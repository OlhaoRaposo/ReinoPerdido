using UnityEngine;
public class PlayerController : MonoBehaviour
{
    private Rigidbody controller => GetComponent<Rigidbody>();
    public static PlayerMovement instance => FindFirstObjectByType<PlayerMovement>();
    [SerializeField] private bool debug;
    [Header("Movement")]
    [SerializeField]private KeyCode runKey = KeyCode.LeftShift;
    [SerializeField]private float baseSpeed = 2;
    [SerializeField]private float runSpeed = 6;
    [SerializeField]private float crounchSpeed = .5f;
    [Space]
    
    [Header("Jump")]
    [SerializeField]private KeyCode jumpKey = KeyCode.Space;
    [SerializeField]private float jumpForce = 650f;
    [SerializeField]private float groundDistance = .2f;
    [SerializeField]private GameObject groundCheck;
    [Space]
    
    [Header("Crounch")]
    [SerializeField] private KeyCode crounchKey = KeyCode.LeftControl;
    
    void FixedUpdate() {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float playerSpeed = baseSpeed;
        
        if(CanRun())
            playerSpeed = Mathf.Lerp(runSpeed, baseSpeed, 0.1f * Time.deltaTime);
        else if(CanCrounch())
            playerSpeed = Mathf.Lerp(crounchSpeed, baseSpeed, 0.1f * Time.deltaTime);
        else if(HasInput())
            playerSpeed = Mathf.Lerp(baseSpeed, runSpeed , 0.5f * Time.deltaTime);
            
        if (CanJump()) {
            Jump();
        }
        if (CanCrounch()) {
            Crounch();
        }else {
            StandUp();
        }
        if (CanRun()){
            StartCoroutine(AimController.instance.PlayRunBobbing());
        }
        Vector3 move = transform.right * x + transform.forward * z;
        controller.MovePosition(controller.position + move * playerSpeed * Time.deltaTime);
    }
    private void StandUp()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * 10);
    }
    private void Crounch()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, 0.5f, 1), Time.deltaTime * 10);
    }
    private void Jump()
    {
        controller.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    private bool CanRun(){
        if (Input.GetKey(runKey) && !Input.GetKey(crounchKey) && controller.linearVelocity.y == 0) {
            return true;
        }
        return false;
    }
    private bool CanCrounch(){
        if (Input.GetKey(crounchKey) && !Input.GetKey(runKey)) {
            return true;
        }
        return false;
    }
    private bool CanJump(){
        if (Input.GetKey(jumpKey) && IsGrounded() && !Input.GetKey(crounchKey)) {
            return true;
        }
        return false;
    }
    private bool IsGrounded(){
        RaycastHit hit;
        if (Physics.Raycast(groundCheck.transform.position, Vector3.down, out hit, groundDistance)) {
            return true;
        }
        return false;
    }
    private bool HasInput(){
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        if (x != 0 || y != 0){
            return true;
        }
        //Lerps to 0
        return false;
    }
    private void OnDrawGizmos() {
        if(!debug) return;
        Color rayColor  = IsGrounded() ? Color.green : Color.red; 
        Debug.DrawRay(groundCheck.transform.position, Vector3.down * groundDistance,rayColor);
        
        if(IsGrounded())
            Gizmos.DrawIcon(transform.position + new Vector3(0,2.1f,0), "isGrounded.png", true, Color.white);
        if(CanJump())
            Gizmos.DrawIcon(transform.position + new Vector3(0,2.3f,0), "isJumping.png", true, Color.white);
        if(CanCrounch())
            Gizmos.DrawIcon(transform.position + new Vector3(0,2.5f,0), "isCrounching.png", true, Color.white);
        if(CanRun())
            Gizmos.DrawIcon(transform.position + new Vector3(0,2.7f,0), "isRunning.png", true, Color.white);
        if(HasInput())
            Gizmos.DrawIcon(transform.position + new Vector3(0,2.9f,0), "isWalking.png", true, Color.white);
            
    }
}
