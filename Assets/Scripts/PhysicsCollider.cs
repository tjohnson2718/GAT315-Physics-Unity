using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCollider : MonoBehaviour
{
    string status;
    Vector3 contact;
    Vector3 normal;

    [SerializeField] GameObject collisionPrefab;
    private void OnCollisionEnter(Collision collision)
    {
        status = "collision enter: " + collision.gameObject.name;
        contact = collision.GetContact(0).point;
        normal = collision.GetContact(0).normal;

        Instantiate(collisionPrefab, contact, Quaternion.LookRotation(normal));

        OnGUI();
        OnDrawGizmos();
    }

    private void OnCollisionStay(Collision collision)
    {

    }

    private void OnCollisionExit(Collision collision)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        status = "collision enter: " + other.gameObject.name;
        OnGUI();
        OnDrawGizmos();
    }

    private void OnTriggerStay(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {

    }

    private void OnGUI()
    {
        GUI.skin.label.fontSize = 16;
        Vector2 screen = Camera.main.WorldToScreenPoint(transform.position);
        GUI.Label(new Rect(screen.x, Screen.height - screen.y, 250, 70), status);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(contact, 0.5f);
        Gizmos.DrawLine(contact, contact + normal);
    }
}
