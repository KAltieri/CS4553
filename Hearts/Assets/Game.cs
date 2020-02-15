using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject leftHeart;
    public GameObject rightHeart;
    public GameObject moveHeart;

    AudioSource src;

    public GameObject canvas;
    public Text text;

    public float thrust;
    public float torque;
    bool win;
    bool move;
    bool heart;

    Quaternion heartRotation;

    Rigidbody2D rbLeft, rbRight;
    public float reachTime;
    public float gameLength;
    float dieTime;
    public float deathTime;
    public float loss; 
    float time;
    Vector3 leftStart, rightStart, heartScale;
    Quaternion rightRotate, leftRotate;
    public Vector3 leftTarget;
    public Vector3 rightTarget;
    
    void Start()
    {
        dieTime = gameLength;
        src = GetComponent<AudioSource>();
        text.canvasRenderer.SetAlpha(0f);
        rbLeft = leftHeart.GetComponent<Rigidbody2D>();
        rbLeft.WakeUp();
        rbRight = rightHeart.GetComponent<Rigidbody2D>();
        rbRight.WakeUp();
        move = false;
        time = 0;
        heartScale = new Vector3(1f, 1f, 0f);
        heartRotation = Quaternion.Euler(0, 0, 0);
        rightStart = rightHeart.transform.position;
        leftStart = leftHeart.transform.position;
        rightRotate = rightHeart.transform.rotation;
        leftRotate = leftHeart.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (heart)
        {
            if(Input.GetKey(KeyCode.F))
            {
                moveHeart.GetComponent<Rigidbody2D>().AddForce(-transform.right * thrust);
            }
            if (Input.GetKey(KeyCode.H))
            {
                moveHeart.GetComponent<Rigidbody2D>().AddForce(transform.right * thrust);
            }
            if (Input.GetKey(KeyCode.T))
            {
                moveHeart.GetComponent<Rigidbody2D>().AddForce(transform.up * thrust);
            }
            if (Input.GetKey(KeyCode.G))
            {
                moveHeart.GetComponent<Rigidbody2D>().AddForce(-transform.up * thrust);
            }
            if (Input.GetKey(KeyCode.Y))
            {
                moveHeart.GetComponent<Rigidbody2D>().AddTorque(torque);
            }
            if (Input.GetKey(KeyCode.R))
            {
                moveHeart.GetComponent<Rigidbody2D>().AddTorque(-torque);
            }
        }
        if (leftHeart.transform.position == leftTarget && rightHeart.transform.position == rightTarget)
        {
            canvas.SetActive(true);
            text.CrossFadeAlpha(1.0f, 1.5f, false);
            moveHeart.SetActive(true);
            leftHeart.SetActive(false);
            rightHeart.SetActive(false);
            heart = true;
            src.Stop();
            src.volume = 1.0f;
        }
        if (move && !heart)
        {
            time += Time.deltaTime / reachTime;
            leftHeart.transform.position = Vector3.Lerp(leftStart, leftTarget, time);
            leftHeart.transform.rotation = Quaternion.Lerp(rightRotate, heartRotation, time);
            leftHeart.transform.localScale = Vector3.Lerp(heartScale, new Vector3(1f, 1f, 1f), time);
            rightHeart.transform.position = Vector3.Lerp(rightStart, rightTarget, time);
            rightHeart.transform.rotation = Quaternion.Lerp(leftRotate, heartRotation, time);
            rightHeart.transform.localScale = Vector3.Lerp(heartScale, new Vector3(1f, 1f, 1f), time);
            src.volume = Mathf.Lerp(1.0f, 0f, time);
        }
        if (!move)
        {
            time += Time.deltaTime;
            if (dieTime <= 0)
            {
                time /= deathTime;
                canvas.SetActive(true);
                rbLeft.constraints = RigidbodyConstraints2D.FreezeAll;
                rbRight.constraints = RigidbodyConstraints2D.FreezeAll;
                leftHeart.transform.localScale = Vector3.Lerp(heartScale, new Vector3(0f, 0f, 1f), time);
                rightHeart.transform.localScale = Vector3.Lerp(heartScale, new Vector3(0f, 0f, 1f), time);
                leftHeart.transform.position = leftStart;
                rightHeart.transform.position = rightStart;
                leftHeart.SetActive(false);
                rightHeart.SetActive(false);
                text.text = "Broken Hearts\nTry Again?";
                text.CrossFadeAlpha(1.0f, 1.5f, false);
            }
            if (time >= 1.0f && dieTime > 0)
            {
                dieTime -= 1;
                heartScale -= new Vector3(loss, loss, 0f);
                leftHeart.transform.localScale = Vector3.Lerp(leftHeart.transform.localScale, heartScale, 1f);
                rightHeart.transform.localScale = Vector3.Lerp(rightHeart.transform.localScale, heartScale, 1f);
                time = 0;
            }
            Compare();
            if (Input.GetKey(KeyCode.A))
            {
                rbLeft.AddForce(-transform.right * thrust);
            }
            if (Input.GetKey(KeyCode.J))
            {
                rbRight.AddForce(-transform.right * thrust);
            }
            if (Input.GetKey(KeyCode.D))
            {
                rbLeft.AddForce(transform.right * thrust);
            }
            if (Input.GetKey(KeyCode.L))
            {
                rbRight.AddForce(transform.right * thrust);
            }
            if (Input.GetKey(KeyCode.W))
            {
                rbLeft.AddForce(transform.up * thrust);
            }
            if (Input.GetKey(KeyCode.I))
            {
                rbRight.AddForce(transform.up * thrust);
            }
            if (Input.GetKey(KeyCode.S))
            {
                rbLeft.AddForce(-transform.up * thrust);
            }
            if (Input.GetKey(KeyCode.K))
            {
                rbRight.AddForce(-transform.up * thrust);
            }
            if (Input.GetKey(KeyCode.Q))
            {
                rbLeft.AddTorque(torque);
            }
            if (Input.GetKey(KeyCode.U))
            {
                rbRight.AddTorque(torque);
            }
            if (Input.GetKey(KeyCode.E))
            {
                rbLeft.AddTorque(-torque);
            }
            if (Input.GetKey(KeyCode.O))
            {
                rbRight.AddTorque(-torque);
            }
        }
    }

    void Compare()
    {
        win = true;
        if (Mathf.Abs(leftHeart.transform.rotation.x - rightHeart.transform.rotation.x) >= 0.15f)
        {
            win = false;
        }
        if (Mathf.Abs(leftHeart.transform.rotation.y - rightHeart.transform.rotation.y) >= 0.15f)
        {
            win = false;
        }
        if(Mathf.Abs(leftHeart.transform.position.x - rightHeart.transform.position.x) >= 0.4f)
        {
            win = false;
        }
        if (Mathf.Abs(leftHeart.transform.position.y - rightHeart.transform.position.y) >= 0.25f)
        {
            win = false;
        }
        if(win)
        {
            src.Play();
            time = 0;
            move = true;
            rightStart = rightHeart.transform.position;
            leftStart = leftHeart.transform.position;
            rightRotate = rightHeart.transform.rotation;
            leftRotate = leftHeart.transform.rotation;
            heartScale = rightHeart.transform.localScale;
            rightHeart.GetComponent<PolygonCollider2D>().enabled = false;
            leftHeart.GetComponent<PolygonCollider2D>().enabled = false;
            rightHeart.GetComponent<Rigidbody2D>().Sleep();
            leftHeart.GetComponent<Rigidbody2D>().Sleep();
            text.text = "Love Wins!\nRestart?";
        }
    }
}
