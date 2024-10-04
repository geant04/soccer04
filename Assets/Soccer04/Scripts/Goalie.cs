using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goalie : MonoBehaviour
{
    private bool isLeap;
    private bool inSave;
    private GameObject ball;
    private Rigidbody rb;

    // ADJUST FOLLOWING VALUES IN EDITOR
    public float torqueSpeed = 0.6f;
    public float jumpForce = 0.26f;
    public float rbMass = 0.6f;

    public void EnterSaveState(Collider obj)
    {
        ball = obj.gameObject;
        inSave = true;

        SavePatrol();
    }

    public void SavePatrol()
    {
        if (ball == null) return;

        Rigidbody ballRB = ball.GetComponent<Rigidbody>();

        Vector3 deltaPos = ball.transform.position + 0.25f * ballRB.velocity;

        Debug.DrawRay(ball.transform.position, ballRB.velocity, Color.red);
        Debug.DrawRay(transform.position, (deltaPos - transform.position).normalized, Color.cyan);

        if (isLeap) return;

        if (Vector3.Distance(ball.transform.position, transform.position) < 6.0)
        {
            Debug.Log("Jump");
            isLeap = true;

            Vector3 jumpDir = deltaPos - transform.position;
            jumpDir.Normalize();

            Jump(jumpDir, ballRB);
        }
    }

    public void Jump(Vector3 dir, Rigidbody ballRB)
    {
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.mass = rbMass;
        }

        float ballVelocity = ballRB.velocity.magnitude;

        rb.AddForce(dir * jumpForce * ballVelocity, ForceMode.Impulse);

        Vector3 randomTorque = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        );

        rb.AddTorque(randomTorque * torqueSpeed, ForceMode.Impulse);
    }

    public void NormalPatrol()
    {
        Player player = GameManager.Instance.GetPlayer();
        Vector3 goalieToPlayer = Vector3.Normalize(player.GetTransform().position - transform.position);
        transform.rotation = Quaternion.LookRotation(goalieToPlayer, Vector3.up);
    }

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetGoalie(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inSave)
        {
            SavePatrol();
            return;
        }

        NormalPatrol();
    }
}
