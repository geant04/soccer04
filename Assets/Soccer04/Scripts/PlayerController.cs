using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private Player player;
    private float playerSpeed = 6.0f;

    public GameObject SoccerBallPrefab;

    void Start()
    {
        player = new Player(gameObject, SoccerBallPrefab);
    }

    void Update()
    {
        float speed = playerSpeed;

        // idk someone good with inputs help me out here
        int v = 0;
        int h = 0;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            v = 1;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            h = 1;
        }

        if (v + h == 0)
        {
            speed = 0.0f;
        }

        Vector3 dir = new Vector3(-Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));


        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed *= 1.6f;
        }

        player.MovePlayer(dir, speed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // blast that shit
            player.Shoot(speed * dir.magnitude);
        }
    }
}

public class Player
{
    private GameObject root;
    private GameObject ball;
    private Humanoid human;

    private float shotPower = 6.0f;
    private float shotAlt = 0.5f;
    private float flip = 0.0f;

    private Vector3 ballOrigin;
    private bool canShoot = true;

    private GameObject soccerBallPrefab;

    public Player(GameObject root, GameObject soccerBall)
    {
        this.root = root;
        human = new Humanoid(root.transform.Find("Human").gameObject);
        ball = root.transform.Find("Ball").gameObject;
        ballOrigin = ball.transform.localPosition;
        soccerBallPrefab = soccerBall;
    }
    public void MovePlayer(Vector3 dir, float playerSpeed)
    {
        if (dir.magnitude < 0.01f || playerSpeed == 0.0f)
        {
            human.Walk(0.0f);
            flip = 0.0f;
            ball.transform.localPosition = ballOrigin;
            return;
        }

        root.transform.position += dir.normalized * playerSpeed * Time.deltaTime;
        RotatePlayer(dir, 0.0f);

        flip += Time.deltaTime * 0.5f * playerSpeed;
        if (flip > 0.5f) flip = 0.0f;

        float walkAngle = flip < 0.25f ? 1.0f : -1.0f;
        human.Walk(45.0f * walkAngle);

        float ballForward = flip < 0.15f ? 1.0f : (flip > 0.35f) ? -1.0f : 0.0f;

        ball.transform.localPosition = ballOrigin + new Vector3(0, 0, ballForward * 0.35f);

    }
    public void RotatePlayer(Vector3 dir, float angle)
    {
        root.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }
    public void Shoot(float speed)
    {
        if (!canShoot) return;

        Vector3 shootDir = root.transform.forward;

        //canShoot = false; // duh you can only shoot one ball
        // can probably move this into its own file
        GameObject newBall = Object.Instantiate(soccerBallPrefab);
        newBall.name = "Bawl";
        newBall.transform.position = ball.transform.position;
        Object.Destroy(newBall, 5.0f);

        Rigidbody ballRigidBody = newBall.GetComponent<Rigidbody>();
        Vector3 blamDir = Vector3.Normalize(shootDir + new Vector3(0, 0.75f * shotAlt, 0));
        ballRigidBody.AddForce(blamDir * shotPower * (speed / 6.0f + 1.0f));

        human.Kick();
    }
}

public class Humanoid
{
    private GameObject body;
    private GameObject LeftShoulderPivot;
    private GameObject RightShoulderPivot;
    private GameObject LeftLegPivot;
    private GameObject RightLegPivot;

    public Humanoid(GameObject Body)
    {
        body = Body;
        LeftShoulderPivot = body.transform.Find("LeftShoulderPivot").gameObject;
        RightShoulderPivot = body.transform.Find("RightShoulderPivot").gameObject;
        LeftLegPivot = body.transform.Find("LeftLegPivot").gameObject;
        RightLegPivot = body.transform.Find("RightLegPivot").gameObject;
    }

    public void Walk(float angle)
    {
        LeftShoulderPivot.transform.localRotation = Quaternion.Euler(angle, 0, 0);
        RightShoulderPivot.transform.localRotation = Quaternion.Euler(-angle, 0, 0);
        LeftLegPivot.transform.localRotation = Quaternion.Euler(angle, 0, 0);
        RightLegPivot.transform.localRotation = Quaternion.Euler(-angle, 0, 0);
    }

    public void Kick()
    {
        RightLegPivot.transform.localRotation = Quaternion.Euler(90.0f, 0, 0);
    }
}
