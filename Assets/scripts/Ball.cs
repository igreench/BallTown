using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    public static Ball instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public static void Show()
    {
        instance.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        instance.gameObject.SetActive(false);
    }

    public void initBall()
    {
        GM.instance.CountCombo = 0;
        transform.parent = GameObject.Find("DummyBall").transform;
        transform.position = GameObject.Find("DummyBall").transform.position;
        GM.ballInPlay = false;
        GM.rbBall.isKinematic = true;
    }
    
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Area")
        {            
            initBall();
            GM.NumberBalls--;
            if (GM.NumberBalls == 0)
            {
                GM.instance.GameOver();
            }
            else
            {
                GM.instance.SetTextGame();
            }
        }
    }
}
