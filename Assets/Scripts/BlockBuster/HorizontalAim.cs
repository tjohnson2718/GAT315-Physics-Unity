using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalAim : MonoBehaviour
{
    [SerializeField] float sensitivity = 3;

    Vector3 rotation = Vector3.zero;
    Vector2 prevAxis = Vector3.zero;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector3 axis = Vector3.zero;

        axis.x = -Input.GetAxis("Mouse Y") - prevAxis.x;

        rotation.x += axis.x * sensitivity;

        rotation.x = Mathf.Clamp(rotation.x, -50, 50);

        //Quaternion q_yaw = Quaternion.AngleAxis(rotation.y * sensitivity, Vector3.up);
        Quaternion q_pitch = Quaternion.AngleAxis(rotation.x * sensitivity, Vector3.right);

        transform.localRotation = (q_pitch);
    }
}
