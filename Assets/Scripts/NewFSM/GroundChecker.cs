using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private float groundDistance = 0.08f;
    [SerializeField] private float radius = 0.08f;
    [SerializeField] private LayerMask groundLayer;

    private RaycastHit hit;

    public bool IsGrounded { get; private set; }

    private void Update()
    {
        IsGrounded = Physics.SphereCast(transform.position, radius, Vector3.down * groundDistance, out hit,
            groundDistance,
            groundLayer);
    }

    private void OnDrawGizmos()
    {
        if (IsGrounded)
        {
            Gizmos.color = Color.green;
            Vector3 sphereCastMidpoint = transform.position + (Vector3.down * hit.distance);
            Gizmos.DrawWireSphere(sphereCastMidpoint, radius);
            Gizmos.DrawSphere(hit.point, 0.1f);
            Debug.DrawLine(transform.position, sphereCastMidpoint, Color.green);
        }
        else
        {
            Gizmos.color = Color.red;
            Vector3 sphereCastMidpoint = transform.position + (Vector3.down * (groundDistance - radius));
            Gizmos.DrawWireSphere(sphereCastMidpoint, radius);
            Debug.DrawLine(transform.position, sphereCastMidpoint, Color.red);
        }
    }
}