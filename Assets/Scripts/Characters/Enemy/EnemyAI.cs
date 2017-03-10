using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour, IDamageable
{
    NavMeshAgent thisNavAgent;
    public Transform player;
    public float health = 10;

    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        thisNavAgent = GetComponent<NavMeshAgent>();
        gameObject.tag = "Enemy";
    }

    void Update()
    {
        thisNavAgent.SetDestination(player.position);
        if (health < 0) die();
    }

    public void takeDamage(float damage)
    {
        health -= damage;
    }

    public void die()
    {
        Destroy(gameObject);
    }
}
