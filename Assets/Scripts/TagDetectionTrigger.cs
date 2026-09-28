using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

public class TagDetectionTrigger : MonoBehaviour
{
    [SerializeField] private bool _requiresVisible;
    [SerializeField,Tag] private string _tag;
    [SerializeField,ReadOnly] private List<Collider> _detectedColliders;
    [SerializeField,ReadOnly] private List<Collider> _detectedHiddenColliders;

    private void Update()
    {
        if(_requiresVisible) UpdateVisibleColliders();
    }
    
    private void UpdateVisibleColliders()
    {
        foreach (Collider other in _detectedHiddenColliders)
        {
            Physics.Raycast(transform.position, 
                   other.transform.position - transform.position, 
                   out RaycastHit hit, Mathf.Infinity);
            if (hit.collider == other)
            {
                if(!_detectedColliders.Contains(other)) _detectedColliders.Add(other);
            }
            else
            {
                _detectedColliders.Remove(other);
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        print("Entered " + other.name);
        if (other.CompareTag(_tag))
        {
            if (_requiresVisible) _detectedHiddenColliders.Add(other);
            else _detectedColliders.Add(other);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_tag))
        {
            _detectedHiddenColliders.Add(other);
            _detectedColliders.Remove(other);
        }
    }

    public bool IsAnyoneDetected()
    {
        return _detectedColliders.Count > 0;
    }
    
    public Collider GetNeerestCollider(Vector3 position)
    {
        if (_detectedColliders.Count > 0)
        {
            Collider collider = null;
            float distance = Mathf.Infinity;
            foreach (Collider col in _detectedColliders)
            {
                float dist = Vector3.Distance(position, col.transform.position);
                if (dist < distance)
                {
                    collider = col;
                    distance = dist;
                }
            }
            return collider;
        }
        return null;
    }
}
