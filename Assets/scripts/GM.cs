using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GM : MonoBehaviour {

    public Transform BallItem;
    public Transform Camera;
    public Transform Map;
    public Transform BrickItem;
    public Transform BrickEnvItem;

    public Canvas CanvasMenuItem;
    public Canvas CanvasOptionsItem;
    public Canvas CanvasGameItem;
    public Canvas CanvasPauseItem;
    public Canvas CanvasLoseItem;
    public Canvas CanvasWinItem;

    public Text TextHi;
    public Text TextLevel;
    public Text TextBalls;
    public Text TextScore;
    public Text TextCombo;
    public InputField InputFieldName;
    public Text TextMaxScore;

    public GameObject ExplosionBrickItem;

    public GameObject BallParticleSystemItem;
    private GameObject BallParticleSystemObject;

    public Slider SliderMusicItem;
    public Slider SliderSoundItem;
    
    [HideInInspector]
    public float ballInitialVelocity = 5f;    
    
    public static bool ballInPlay = false;
    public static Rigidbody rbBall;
    
    public static float k = 0.5f;
    public static int NumberLevel = 0;
    public static int NumberBalls = 0;
    public static int Score = 0;

    private float nMap = 2;
    private float mMap = 2;
    private bool isPlay = false;
    private bool isMenu = true;
    private bool isInitinigMap = false;
    private Vector3 BallVelocity;

    private int maxScore = 0;
    public int CountCombo = 0;

    public static GM instance = null;

    void Start()
    {
        StartMenu();
        InitEnv();
        InitMap();
    }

    void StartMenu()
    {
        nMap = 6;
        mMap = 6;
        CountCombo = 0;
        isMenu = true;
        isPlay = false;
        Ball.Hide();
        Player.Hide();
        Ball.instance.enabled = false;
        Player.instance.enabled = false;

        CanvasMenuItem.enabled = true;
        CanvasOptionsItem.enabled = false;
        CanvasGameItem.enabled = false;
        CanvasPauseItem.enabled = false;
        CanvasLoseItem.enabled = false;
        CanvasWinItem.enabled = false;

        ParticleSystem ps = BallParticleSystemObject.GetComponent<ParticleSystem>();
        ps.Stop();

        AudioManager.instance.PlayMenu();
    }

    void InitEnv()
    {
        for (float i = 0; i < 20; i++)
        {
            for (float j = 0; j < 20; j++)
            {
                float x = -10 + i * k * 2;
                float z = -10 + j * k * 2;
                float d = Mathf.Pow(x, 2) + Mathf.Pow(z, 2);
                if (d > 30 && d < 80)
                {
                    float r = Random.Range(1, 4);
                    BrickEnvItem.transform.localScale = new Vector3(k, r * k, k);
                    Instantiate(BrickEnvItem, new Vector3(x, r * k / 2, z), Quaternion.identity);
                }
            }
        }

    }

    void InitMap()
    {
        for (float x = -nMap / 2; x < nMap / 2; x += 1.0f)
        {
            for (float z = -mMap / 2; z < mMap / 2; z += 1.0f)
            {
                float r = Random.Range(1, 4);
                BrickItem.transform.localScale = new Vector3(k, r * k, k);
                Instantiate(BrickItem, new Vector3(x + k, r * k / 2, z + k), Quaternion.identity);
            }
        }
    }

	void Awake () {
	    rbBall = BallItem.GetComponent<Rigidbody>();

        BallParticleSystemObject = (GameObject)Instantiate(BallParticleSystemItem, new Vector3(0, 0, 0), Quaternion.identity);
        BallParticleSystemObject.transform.parent = transform;
        ParticleSystem ps = BallParticleSystemObject.GetComponent<ParticleSystem>();
        ps.Play();
        
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
	
	void Update () {
        if (isPlay)
        {
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            if (Cursor.visible)
            {
                Cursor.visible = false;
            }
            if (Input.GetButtonDown("Fire1") && !ballInPlay)
            {
                BallItem.parent = null;
                ballInPlay = true;
                rbBall.isKinematic = false;
                rbBall.AddForce(-Camera.position.normalized * ballInitialVelocity);
            }
            else
            {
                rbBall.velocity = rbBall.velocity.normalized * ballInitialVelocity;
            }
            if (Input.GetKey(KeyCode.Escape))
            {
                //TODO
                //Application.Quit();
                PauseGame();
            }
            SetBallPSPos();
        }
        if (isMenu)
        {
            Player.instance.MoveCamera();
        }
	}

    public void StartGame()
    {
        Ball.Show();
        Player.Show();
        Ball.instance.enabled = true;
        Ball.instance.initBall();
        Player.instance.enabled = true;
        CanvasMenuItem.enabled = false;
        CanvasGameItem.enabled = true;
        isPlay = true;
        isMenu = false;
        HideMouse();
        nMap = 2;
        mMap = 2;
        NumberLevel = 1;
        NumberBalls = 10; //10
        Score = 0;
        SetTextGame();
        if (!isInitinigMap)
        {
            StartCoroutine(StartInitMap());
        }
        Popup("LEVEL 1");

        ParticleSystem ps = BallParticleSystemObject.GetComponent<ParticleSystem>();
        ps.Play();

        AudioManager.instance.PlayGame();
    }

    IEnumerator StartInitMap()
    {
        isInitinigMap = true;
        Brick.AllDown();
        yield return new WaitForSeconds(1);
        isInitinigMap = false;
        Brick.DestroyBricks();
        InitMap(); 
    }

    public void SetTextGame()
    {
        TextLevel.text = "Level: " + NumberLevel;
        TextBalls.text = "Balls: " + NumberBalls;
        TextScore.text = "Score: " + Score;
    }

    public void GameOver()
    {
        isPlay = false;
        ShowMouse();
        Ball.instance.enabled = false;
        Player.instance.enabled = false;
        BallVelocity = rbBall.velocity;
        rbBall.Sleep();
        CanvasLoseItem.enabled = true;
    }

    public void LevelWin()
    {
        NumberLevel++;
        NumberBalls = 10;
        Score = 0;

        if (NumberLevel < 10)
        {
            Popup("LEVEL " + NumberLevel.ToString());
            if (NumberLevel % 2 == 0)
            {
                nMap++;
            }
            else
            {
                mMap++;
            }
            SetTextGame();
            InitMap();
            Ball.instance.initBall();
        }
        else
        {
            isPlay = false;
            ShowMouse();
            Ball.instance.enabled = false;
            Player.instance.enabled = false;
            BallVelocity = rbBall.velocity;
            rbBall.Sleep();
            CanvasWinItem.enabled = true;
        }        
    }

    void DestroyMap()
    {
        Brick.DestroyBricks();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HideMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void PauseGame()
    {
        isPlay = false;
        CanvasPauseItem.enabled = true;
        ShowMouse();
        Ball.instance.enabled = false;
        Player.instance.enabled = false;
        BallVelocity = rbBall.velocity;
        rbBall.Sleep();
        AudioManager.instance.Pause();
    }

    public void UnpauseGame()
    {
        isPlay = true;
        CanvasPauseItem.enabled = false;
        HideMouse();
        Ball.instance.enabled = true;
        Player.instance.enabled = true;
        rbBall.WakeUp();
        rbBall.velocity = BallVelocity;
        AudioManager.instance.Play();
    }

    public void ExitPause()
    {
        StartMenu();   
        StartCoroutine(StartInitMap());
        ParticleSystem ps = BallParticleSystemObject.GetComponent<ParticleSystem>();
        ps.Stop();
    }

    public void InitExplosion(Vector3 pos)
    {
        GameObject expl = (GameObject)Instantiate(ExplosionBrickItem, new Vector3(pos.x, 0.1f, pos.z), Quaternion.identity);
        ParticleSystem heal = expl.GetComponent<ParticleSystem>();
        heal.Play();
        Destroy(expl, 2f);
        AudioManager.instance.PlayExplosion();
    }

    public void SetBallPSPos()
    {
        BallParticleSystemObject.transform.position = BallItem.position;        
    }

    public void Popup(string str)
    {
        Text tempTextBox = Instantiate(TextCombo, new Vector3(400f, 0, 0), Quaternion.identity) as Text;
        tempTextBox.transform.SetParent(CanvasGameItem.transform, false);
        tempTextBox.text = str;
        Destroy(tempTextBox, 2f);
    }

    public void ShowOptions() 
    {
        CanvasMenuItem.enabled = false;
        CanvasOptionsItem.enabled = true;
    }

    public void ExitLose()
    {
        StartMenu();
        StartCoroutine(StartInitMap());
        SetMaxScore();
        ParticleSystem ps = BallParticleSystemObject.GetComponent<ParticleSystem>();
        ps.Stop();
    }

    public void ExitWin()
    {
        StartMenu();
        StartCoroutine(StartInitMap());        
        SetMaxScore();
        ParticleSystem ps = BallParticleSystemObject.GetComponent<ParticleSystem>();
        ps.Stop();
    }

    public void ExitOptions()
    {
        CanvasMenuItem.enabled = true;
        CanvasOptionsItem.enabled = false;
    }

    public void InputNameChanging()
    {
        string upperText = InputFieldName.text.ToUpper();
        if (upperText != InputFieldName.text)
        {
            InputFieldName.text = upperText;
        }
    }

    public void InputNameChanged()
    {
        TextHi.text = "HI, " + InputFieldName.text;
    }

    void SetMaxScore()
    {
        if (Score > maxScore)
        {
            maxScore = Score;
            TextMaxScore.text = "MAX SCORE: " + maxScore;
        }
    }

    public void SetVolumeMusic()
    {
        AudioManager.instance.SetVolumeMusic(SliderMusicItem.value);
    }

    public void SetVolumeSound()
    {
        AudioManager.instance.SetVolumeSound(SliderSoundItem.value);
        AudioManager.instance.PlayExplosion();
    }
}
