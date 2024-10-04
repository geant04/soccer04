using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private Player player;
    private float playerSpeed = 6.0f;
    private bool popUpActive = true;
    public GameObject SoccerBallPrefab;

    public Player GetPlayer()
    {
        return player;
    }

    void ShutOffPopUp()
    {
        popUpActive = false;
        Transform popUp = transform.Find("PopUp");
        if (popUp != null)
        {
            popUp.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        player = new Player(gameObject, SoccerBallPrefab);
        GameManager.Instance.SetPlayer(player);
    }

    void Update()
    {
        float moveSpeed = playerSpeed;
        float turnSpeed = playerSpeed;

        // idk someone good with inputs help me out here
        int v = 0;
        int h = 0;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) v = 1;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) h = 1;

        if (v + h == 0) moveSpeed = 0.0f;
        if (v + h != 0 && popUpActive) ShutOffPopUp();
        if (h == 0) turnSpeed = 0.0f;

        Vector3 dir = new Vector3(-Input.GetAxis("Vertical") * v, 0, Input.GetAxis("Horizontal") * h);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed *= 1.6f;
        }

        player.MovePlayer(dir.normalized, moveSpeed, turnSpeed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // blast that shit
            player.Shoot(moveSpeed * dir.magnitude, turnSpeed * Input.GetAxis("Horizontal")); // maybe turnSpeed should come in handy for curve
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
    public void MovePlayer(Vector3 dir, float moveSpeed, float turnSpeed)
    {
        if (dir.magnitude < 0.01f || (moveSpeed == 0.0f && turnSpeed == 0.0f))
        {
            human.Walk(0.0f);
            flip = 0.0f;
            ball.transform.localPosition = ballOrigin;
            return;
        }

        root.transform.position += dir * moveSpeed * Time.deltaTime;

        RotatePlayer(dir, 0.0f);

        AnimatePlayerAndBall(moveSpeed);
    }

    public void AnimatePlayerAndBall(float speed)
    {
        flip += Time.deltaTime * 0.5f * speed;
        if (flip > 0.5f) flip = 0.0f;

        float walkAngle = flip < 0.25f ? 1.0f : -1.0f;
        human.Walk(45.0f * walkAngle);

        float ballForward = flip < 0.15f ? 1.0f : (flip > 0.35f) ? -1.0f : 0.0f;

        ball.transform.localPosition = ballOrigin + new Vector3(0, 0, ballForward * 0.35f);
    }
    public void RotatePlayer(Vector3 dir, float angle)
    {
        root.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        //root.transform.Rotate(new Vector3(0, angle * Time.deltaTime * 32.0f, 0));
    }
    public void Shoot(float speed, float curveVelocity)
    {
        if (!canShoot) return;

        ball.SetActive(false);

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
        ballRigidBody.AddTorque(new Vector3(curveVelocity * 0.01f, 0, 0));

        human.Kick();
    }
    
    ///<summary>
    /// Gets transform of the root
    ///</summary>
    public Transform GetTransform()
    {
        return root.transform;
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
