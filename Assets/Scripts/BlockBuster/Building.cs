using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] float health = 100;
    [SerializeField] GameObject destroyPrefab;
    [SerializeField] AudioSource sound;

    private void Update()
    {
        if (health <= 0)
        {
            Vector3 spawnPos = transform.position;
            spawnPos.y += 5;
            Instantiate(destroyPrefab, spawnPos, Quaternion.identity);
            sound.Play();
            Destroy(gameObject);
            Destroy(destroyPrefab, 3);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ammo"))
        {
            health = health - 25;
        }
    }
}
