using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShootScript : MonoBehaviour
{
    private GameController gameController;

    public float power = 2;
    //private int dot = 15;

    private Vector2 startPos;

    private bool shoot, aiming;

    private GameObject Dots;
    private List<GameObject> projectilesPath;

    private Rigidbody2D ballbody;

    public GameObject ballPrefab;
    public GameObject ballContainer;


    void Awake()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        Dots = GameObject.Find("Dots");

    }
    void Start()
    {
        projectilesPath = Dots.transform.Cast<Transform>().ToList().ConvertAll(t => t.gameObject);
        HideDots();
    }

    void Update()
    {
        ballbody = ballPrefab.GetComponent<Rigidbody2D>();
        if (gameController.shotCount <= 3)
        {
            Aim();
            Rotate();
        }
    }

    void Aim()
    {
        if (shoot)
            return;

        if (Input.GetMouseButton(0)) // or Input.GetAxis("Fire1")==1
        {
            if (!aiming)
            {
                //cal
                aiming = true;
                startPos = Input.mousePosition;
                gameController.CheckShotCount();
            }
            else
            {
                //aim call path
                PathCalculation();
            }
        }
        else if (aiming && !shoot)
        {
            aiming = false;
            HideDots();
            StartCoroutine(Shoot());
            if (gameController.shotCount == 1)
            {
                Camera.main.GetComponent<CameraTransition>().RotateCameraToSide();
            }
        }
    }

    Vector2 ShootForce(Vector3 force)
    {
        return (new Vector2(startPos.x, startPos.y) - new Vector2(force.x, force.y)) * power;
    }

    Vector2 DotPath(Vector2 startP, Vector2 startVel, float t)
    {
        return startP + startVel * t + .5f * Physics2D.gravity * t * t;
    }
    void PathCalculation()
    {
        Vector2 vel = ShootForce(Input.mousePosition) * Time.fixedDeltaTime / ballbody.mass;
        for (int i = 0; i < projectilesPath.Count; i++)
        {
            ShowDots();
            float t = i / 15f;
            Vector3 point = DotPath(transform.position, vel, t);
            point.z = 1;
            projectilesPath[i].transform.position = point;
        }
    }

    void ShowDots()
    {
        for (int i = 0; i < projectilesPath.Count; i++)
        {
            projectilesPath[i].GetComponent<Renderer>().enabled = true;
        }
    }
    void HideDots()
    {
        for (int i = 0; i < projectilesPath.Count; i++)
        {
            projectilesPath[i].GetComponent<Renderer>().enabled = false;
        }
    }
    void Rotate()
    {
        Vector2 dir = GameObject.Find("dot (1)").transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    IEnumerator Shoot()
    {
        for (int i = 0; i < gameController.ballsCount; i++)
        {

            yield return new WaitForSeconds(0.07f);
            GameObject ball = Instantiate(ballPrefab, transform.position, Quaternion.identity);
            ball.name = "Ball";
            ball.transform.SetParent(ballContainer.transform);
            ballbody = ball.GetComponent<Rigidbody2D>();
            ballbody.AddForce(ShootForce(Input.mousePosition));

            int balls = gameController.ballsCount - i;
            gameController.ballsCountText.text = (balls - 1).ToString();
        }

        yield return new WaitForSeconds(0.5f);
        gameController.shotCount++;
        gameController.ballsCountText.text = gameController.ballsCount.ToString();
    }
}
