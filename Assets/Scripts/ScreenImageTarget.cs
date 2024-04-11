using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;

public class ScreenImageTarget : MonoBehaviour
{
    [SerializeField] float distance = 5;
    [SerializeField] Image image;
    [SerializeField] Camera view;

    private void LateUpdate()
    {
        Vector3 screen = image.transform.position;
        screen.z = 10;

        Vector3 world = view.ScreenToWorldPoint(screen);
        transform.position = world;
    }

}
