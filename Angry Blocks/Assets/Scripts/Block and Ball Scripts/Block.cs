using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private int count;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -10)
        {
            Destroy(gameObject);
        }
    }

    public void SetStartingCount(int count)
    {
        this.count = count;
    }

    private void OnCollisionEnter2D(Collision2D target)
    {
        if (target.collider.name == "Ball" && count > 0)
        {
            count--;
            Camera.main.GetComponent<CameraTransition>().Shake();

            if (count == 0)
            {
                Destroy(gameObject);
                Camera.main.GetComponent<CameraTransition>().MediumShake();
            }
        }
    }
}
