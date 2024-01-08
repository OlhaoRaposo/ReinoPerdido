using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioObject : MonoBehaviour
{
    public AudioCaracteristics caracteristics;
    [SerializeField]
    private float atualPropagationDistance;
    [SerializeField]
    bool hasAchivedMaxDistance = false;
    [SerializeField]
    bool startPropagation = false;

    private void Start() {
        startPropagation = true;
    }

    void Update() { PropagateAudio(); }

    private void PropagateAudio()
    {
        if (!startPropagation)
            return;
        if (!hasAchivedMaxDistance)
        {
            float speed = caracteristics.propagationSpeed;
            speed = Mathf.Lerp(caracteristics.propagationSpeed, caracteristics.propagationSpeed / 2, (0.1f * Time.deltaTime));
            atualPropagationDistance += speed * Time.deltaTime;
            if (atualPropagationDistance >= caracteristics.propagationDistance) {
                hasAchivedMaxDistance = true;
                DestroyMyself();
            }
        }

        if (TryGetComponent(out SphereCollider col)) { col.radius = atualPropagationDistance; }
    }

    public void DestroyMyself()
    {
        Destroy(this.gameObject);
    }

    private void OnDrawGizmos() {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, atualPropagationDistance);
    }
}

[Serializable]
public class AudioCaracteristics {
    public float propagationDistance;
    [Range(2,100)]
    public float propagationSpeed;
}
