using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAI : MonoBehaviour
{
    public float speed;
    [HideInInspector] public Vector2 destination;
    [HideInInspector] public int damage;
    // Use this for initialization
    void Awake()
    {
        //rotate arrow
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, destination);
        if (distance.Equals(0)) { Destroy(gameObject); }
        transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            //Let me hit you.
            PlayerHealth.doDamage(damage, this.transform.position);
            Destroy(gameObject);
        }
    }

    public Vector2 computeDestination(Vector3 playerPosition)
    {
        return transform.position + (playerPosition - transform.position).normalized * 80.0f;
    }

    public Quaternion computeRotation(Vector3 playerPosition)
    {
        return Quaternion.LookRotation(Vector3.forward, playerPosition - transform.position) * Quaternion.Euler(0, 0, -180);
    }
}
