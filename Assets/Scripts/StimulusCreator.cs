using UnityEngine;
public class StimulusCreator : MonoBehaviour
{
    public GameObject stimulusPrefab;
    
    void Start() {
        
    }
    void Update() {
        if(LeftClick()) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                Vector3 instantiatePosition = hit.point; 
                instantiatePosition += Vector3.up * 0.1f + new Vector3(0,0.5f,0) ;
                Instantiate(stimulusPrefab, instantiatePosition, Quaternion.identity);
            }
        }
    }
    private bool LeftClick()
    {
        if(Input.GetMouseButtonDown(0))
            return true;
        else
            return false;
    }
}
