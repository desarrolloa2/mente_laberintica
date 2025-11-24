using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [Header("Movimiento")]
    public float chaseSpeed = 5f;

    [Header("Detección del jugador")]
    public float detectionRadius = 10f;
    public float loseSightRadius = 14f;
    public float fieldOfView = 90f;
    public Transform player;

    [Header("Rotación de torreta")]
    public float rotationStep = 45f;    // paso de giro
    public float rotationSpeed = 5f;    // velocidad de rotación entre pasos

    [Header("Daño")]
    public int damage = 10;

    private bool isChasing = false;

    void Update()
    {
        if (player == null)
            return;

        if (CanSeePlayer())
            isChasing = true;
        else if (Vector3.Distance(transform.position, player.position) > loseSightRadius)
            isChasing = false;

        if (isChasing)
            ChasePlayer();
        else
            RotateInSteps();
    }

    // ---------------------------
    //  TORRETA ROTANDO A 45°
    // ---------------------------
    void RotateInSteps()
    {
        Quaternion stepRotation = Quaternion.Euler(0f, rotationStep, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, transform.rotation * stepRotation, Time.deltaTime * rotationSpeed);
    }

    // ---------------------------
    //     PERSEGUIR JUGADOR
    // ---------------------------
    void ChasePlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0;

        Quaternion targetRot = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);

        transform.position = Vector3.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
    }

    // ---------------------------
    //        DETECCIÓN
    // ---------------------------
    bool CanSeePlayer()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist > detectionRadius) return false;

        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, dirToPlayer);

        if (angle > fieldOfView / 2f) return false;

        if (Physics.Raycast(transform.position + Vector3.up, dirToPlayer, out RaycastHit hit, detectionRadius))
        {
            if (hit.collider.CompareTag("Player"))
                return true;
        }

        return false;
    }

    // ---------------------------
    //      DAÑO AL JUGADOR
    // ---------------------------
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth hp = collision.gameObject.GetComponent<PlayerHealth>();
            if (hp != null) hp.TakeDamage(damage);
        }
    }
}
