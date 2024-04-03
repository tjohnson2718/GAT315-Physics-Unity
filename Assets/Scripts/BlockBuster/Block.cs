using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Block : MonoBehaviour
{
    [SerializeField] int points = 100;
    [SerializeField] AudioSource audioSource;

    Rigidbody rb;
    bool destroyed = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (rb.velocity.magnitude > 2 || rb.angularVelocity.magnitude > 2)
        {
            audioSource.Play();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        

        if (!destroyed && other.CompareTag("Kill") 
            && rb.velocity.magnitude == 0 
            && rb.angularVelocity.magnitude == 0)
        {
            destroyed = true;
            print(points);
            Destroy(gameObject, 0.5f);
        }
    }
}
