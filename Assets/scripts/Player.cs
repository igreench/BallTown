using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    [HideInInspector]
    public float distanceMax = 1.0f;
    public float paddleSpeed = 0.1f;
    public float cameraSpeed = 3.0f;

    private Vector3 playerPos;
    private float currentCameraSpeed = 0.0f;
    private bool isRotation = false;

    private static GameObject PaddleItem;

    public static Player instance = null;

    void Awake()
    {
        PaddleItem = GameObject.Find("Paddle");
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
        PaddleItem.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        PaddleItem.gameObject.SetActive(false);
    }

    public static int Sign(float value)
    {
        if (value > 0)
        {
            return 1;
        }
        if (value < 0)
        {
            return -1;
        }
        return 0;
    }

    public void MoveCamera()
    {
        transform.Rotate(0, cameraSpeed / 4, 0);
    }
    
	void Update () {
        float xPos = transform.Find("DummyPaddle").localPosition.x + (Input.GetAxis("Mouse X") * paddleSpeed);
        playerPos = new Vector3(Mathf.Clamp(xPos, -distanceMax, distanceMax), playerPos.y, playerPos.z);
        transform.Find("DummyPaddle").localPosition = playerPos;

        if (!isRotation)
        {
            if (Mathf.Abs(xPos) >= distanceMax)
            {
                currentCameraSpeed = -Sign(Input.GetAxis("Mouse X")) * cameraSpeed;
                isRotation = true;
            }
        } else {
            if (Mathf.Abs(xPos) < distanceMax)
            {
                currentCameraSpeed = 0.0f;
                isRotation = false;
            }
        }
        transform.Rotate(0, currentCameraSpeed, 0);        
	}
}
