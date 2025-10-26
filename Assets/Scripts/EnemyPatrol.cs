using UnityEngine;
using System.Collections.Generic;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 3f;
    public bool loop = true; // si quieres que vuelva al inicio cuando termina
    public bool lookAtTarget = true; // si quieres que mire hacia el waypoint

    [Header("Waypoints")]
    public List<Transform> waypoints = new List<Transform>();

    private int currentWaypoint = 0;
    private bool forward = true; // para patrulla ida y vuelta

    public int damage = 10; // cantidad de daño al tocar

    void Update()
    {
        if (waypoints.Count == 0) return;

        Transform target = waypoints[currentWaypoint];
        Vector3 direction = target.position - transform.position;
        direction.y = 0; // no inclinarse en vertical

        // Rotación opcional hacia el siguiente punto
        if (lookAtTarget && direction.magnitude > 0.1f)
        {
            Quaternion rot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * 5f);
        }

        // Movimiento hacia el waypoint
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Llegar al waypoint
        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            NextWaypoint();
        }
    }

    void NextWaypoint()
    {
        if (loop)
        {
            // bucle infinito (1 → 2 → 3 → 1 → 2 …)
            currentWaypoint = (currentWaypoint + 1) % waypoints.Count;
        }
        else
        {
            // ida y vuelta
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

    // Mostrar gizmos en el editor
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < waypoints.Count; i++)
        {
            if (waypoints[i] != null)
            {
                Gizmos.DrawSphere(waypoints[i].position, 0.2f);
                if (i < waypoints.Count - 1)
                    Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
                else if (loop && waypoints.Count > 1)
                    Gizmos.DrawLine(waypoints[i].position, waypoints[0].position);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

}
