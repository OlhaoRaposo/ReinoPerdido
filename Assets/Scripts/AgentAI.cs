using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor;
public class AgentAI : MonoBehaviour
{
    public Caracteristics caracteristics;
    private NavMeshAgent agent;
    [SerializeField]
    public bool hasDettectedAudio = false; 
    Vector3 audioPosition;

    private void Start() {
        SetVariables();
    }
    public void SetVariables()
    {
        if (TryGetComponent(out NavMeshAgent agent))
            this.agent = agent;
    }
    void FixedUpdate()
    {
        Senses();
    }
    private void Senses() {
        Hearing();
        Sight();
    }
    #region Senses Region

    private void Hearing() {
        if(!caracteristics.senses.hearing.canHear)
                    return;
        Collider[] colliders = Physics.OverlapSphere(transform.position, caracteristics.senses.hearing.hearingDistance);
        if(hasDettectedAudio)
            return;
        foreach (Collider col in colliders) {
            Debug.Log("Colliders: " + col.gameObject.name);
            if (col.gameObject.CompareTag("Audio")) {
                    hasDettectedAudio = true;
                    audioPosition = col.gameObject.transform.position + (Random.insideUnitSphere * CalculateHearingPrecision()) * 5 ;
                    audioPosition = new Vector3(audioPosition.x, transform.position.y, audioPosition.z);
                    agent.SetDestination(audioPosition);
            }
        }
    }
    private float CalculateHearingPrecision() {
        float offset = 0;
        if(caracteristics.senses.hearing.hearingPrecision == 100)
            offset = 0;
        else if(caracteristics.senses.hearing.hearingPrecision == 0)
            offset = 0;
        else if (caracteristics.senses.hearing.hearingPrecision > 0 && caracteristics.senses.hearing.hearingPrecision < 1)
        {
           
            
        }
        return offset;
    }
    private void Sight()
    {
        if(!caracteristics.senses.sight.canSee)
            return;
        for (int i = 0; i < caracteristics.senses.sight.sightRays; i++)
        {
            float halfAngle = caracteristics.senses.sight.sightAngle / 2f;
            float angle = (i * caracteristics.senses.sight.sightAngle / (caracteristics.senses.sight.sightRays - 1)) - halfAngle; // Calcula o ângulo para cada raio
            RaycastHit hit;
            Vector3 direction = Quaternion.AngleAxis(angle, transform.up) * transform.forward;
            if (Physics.Raycast(transform.position, direction, out hit, caracteristics.senses.sight.sightDistance)) {
                
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (caracteristics.senses.sight.canSee) {
            for (int i = 0; i < caracteristics.senses.sight.sightRays; i++)
            {
                float halfAngle = caracteristics.senses.sight.sightAngle / 2f;
                float angle = (i * caracteristics.senses.sight.sightAngle / (caracteristics.senses.sight.sightRays - 1)) - halfAngle; // Calcula o ângulo para cada raio
               
                RaycastHit hit;
                Vector3 direction = Quaternion.AngleAxis(angle, transform.up) * transform.forward;

                if (Physics.Raycast(transform.position, direction, out hit, caracteristics.senses.sight.sightDistance)) {
                    Debug.DrawRay(transform.position, direction * hit.distance, Color.red);
                }else {
                    Debug.DrawRay(transform.position, direction * caracteristics.senses.sight.sightDistance, Color.green);
                }
            }
        }
        if (caracteristics.senses.hearing.canHear){
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, caracteristics.senses.hearing.hearingDistance);
        }
        if(hasDettectedAudio) {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, audioPosition);
            Gizmos.DrawWireSphere(audioPosition, 2f);
        }
        
    }
    #endregion

    private void Behaviours()
    {


    }
}

[Serializable]
public class Caracteristics 
{ 
    [Range(0, 100)] public float aggressiveness;
    [Range(0, 100)] public float curiosity;
    [Range(0, 100)] public float fear;
    public Senses senses;
}
[Serializable]
public class Senses {
    public Sight sight;
    public Hearing hearing;
}
[Serializable]
public class Hearing {
    [Header("Hearing")]
    public bool canHear;
    [Range(0, 100)] public float hearingDistance;
    [Range(0, 100)] public float hearingPrecision;
}
[Serializable]
public class Sight {
    [Header("Sight")]
    public bool canSee;
    [Range(0, 100)] public float sightDistance;
    [Range(0, 360)] public float sightAngle;
    [Range(0, 100)] public int sightRays;
}

[CustomEditor(typeof(AgentAI))]
class CustomEditorClass : Editor
{
    private void OnValidate()
    {
        Repaint();
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        AgentAI target = (AgentAI)base.target;
        EditorGUILayout.LabelField("Individual Characteristics Settings", EditorStyles.boldLabel);
        
        target.caracteristics.aggressiveness = EditorGUILayout.Slider("Aggressiveness", target.caracteristics.aggressiveness, 0, 100);
        target.caracteristics.curiosity = EditorGUILayout.Slider("Curiosity", target.caracteristics.curiosity, 0, 100);
        target.caracteristics.fear = EditorGUILayout.Slider("Fear", target.caracteristics.fear, 0, 100);
        GUILayout.Space(20);
        
        if (GUILayout.Button("Enable/Disable Vision")) {
            target.caracteristics.senses.sight.canSee = !target.caracteristics.senses.sight.canSee;
        }
        
        if(target.caracteristics.senses.sight.canSee) {
            target.caracteristics.senses.sight.sightDistance = EditorGUILayout.Slider("Sight Distance", target.caracteristics.senses.sight.sightDistance, 0, 100);
            target.caracteristics.senses.sight.sightAngle = EditorGUILayout.Slider("Sight FOV", target.caracteristics.senses.sight.sightAngle, 0, 360);
            target.caracteristics.senses.sight.sightRays = EditorGUILayout.IntSlider("Sight Rays Count", target.caracteristics.senses.sight.sightRays, 0, 100);
        }
        if (GUILayout.Button("Enable/Disable Hearing")) {
            target.caracteristics.senses.hearing.canHear = !target.caracteristics.senses.hearing.canHear;
        }
        if(target.caracteristics.senses.hearing.canHear) {
            target.hasDettectedAudio = EditorGUILayout.Toggle("Has Dettected Audio", target.hasDettectedAudio);
            target.caracteristics.senses.hearing.hearingDistance = EditorGUILayout.Slider("Hearing Distance", target.caracteristics.senses.hearing.hearingDistance, 0, 100);
            target.caracteristics.senses.hearing.hearingPrecision = EditorGUILayout.Slider("Hearing Precision", target.caracteristics.senses.hearing.hearingPrecision, 0, 100);
        }
    }
}

#endif
