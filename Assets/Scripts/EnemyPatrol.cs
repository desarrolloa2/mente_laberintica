using UnityEngine;
using System.Collections.Generic;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 3f;
    public float chaseSpeed = 5f; // velocidad al perseguir
    public bool loop = true;
    public bool lookAtTarget = true;

    [Header("Waypoints")]
    public List<Transform> waypoints = new List<Transform>();

    [Header("Detección del Jugador")]
    public float detectionRadius = 8f;       // distancia para detectar
    public float loseSightRadius = 12f;      // distancia para dejar de seguir
    public float fieldOfView = 90f;          // ángulo de visión
    public Transform player;                 // referencia al jugador

    public int damage = 10;

    private int currentWaypoint = 0;
    private bool forward = true;

    private bool isChasing = false;

    void Update()
    {
        if (player == null)
            return;

        // Si detecta al jugador → perseguir
        if (CanSeePlayer())
        {
            isChasing = true;
        }
        else
        {
            // Si está siguiendo pero perdió al jugador
            float dist = Vector3.Distance(transform.position, player.position);
            if (dist > loseSightRadius)
                isChasing = false;
        }

        if (isChasing)
            ChasePlayer();
        else
            Patrol();
    }

    // -----------------------
    //     PATRULLA NORMAL
    // -----------------------
    void Patrol()
    {
        if (waypoints.Count == 0) return;

        Transform target = waypoints[currentWaypoint];
        Vector3 direction = target.position - transform.position;
        direction.y = 0;

        if (lookAtTarget && direction.magnitude > 0.1f)
        {
            Quaternion rot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * 5f);
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            NextWaypoint();
        }
    }

    void NextWaypoint()
    {
        if (loop)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Count;
        }
        else
        {
            if (forward)
            {
                currentWaypoint++;
                if (currentWaypoint >= waypoints.Count - 1)
                    forward = false;
            }
            else
            {
                currentWaypoint--;
                if (currentWaypoint <= 0)
                    forward = true;
            }
        }
    }

    // -----------------------
    //     PERSEGUIR JUGADOR
    // -----------------------
    void ChasePlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0;

        if (lookAtTarget && direction.magnitude > 0.1f)
        {
            Quaternion rot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * 5f);
        }

        transform.position = Vector3.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
    }

    // -----------------------
    //   DETECCIÓN DEL JUGADOR
    // -----------------------
    bool CanSeePlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > detectionRadius)
            return false;

        Vector3 dirToPlayer = (player.position - transform.position).normalized;

        float angle = Vector3.Angle(transform.forward, dirToPlayer);

        if (angle > fieldOfView / 2f)
            return false;

        // *Raycast opcional para no ver a través de paredes*
        if (Physics.Raycast(transform.position + Vector3.up, dirToPlayer, out RaycastHit hit, detectionRadius))
        {
            if (hit.collider.CompareTag("Player"))
                return true;
        }

        return false;
    }

    // -----------------------
    //    DAÑO AL JUGADOR
    // -----------------------
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
                playerHealth.TakeDamage(damage);
        }
    }
}
