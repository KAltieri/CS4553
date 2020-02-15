using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject cam;
    public GameObject heart;
    public GameObject leftHeart;
    public GameObject rightHeart;
    public GameObject otherCanvas;
    bool click;
    bool win;
    public float lerpTime;

    public Vector3 rPos, lPos;
    Vector3 rCurrent, lCurrent;
    Quaternion rRot, lRot;

    public void StartClick()
    {
        click = false;
        cam.GetComponent<Game>().enabled = true;
        gameObject.SetActive(false);
    }
    public void EndClick()
    {
        click = true;
        win = false;
        cam.GetComponent<Game>().enabled = false;
        Vector3 heartPos = heart.transform.position;
        lRot = leftHeart.transform.rotation;
        lCurrent = leftHeart.transform.position;
        rCurrent = rightHeart.transform.position;
        rRot = rightHeart.transform.rotation;
        if(rightHeart.transform.localScale != new Vector3(0,0,0))
        {
            leftHeart.transform.position = new Vector3(heartPos.x + .025f, heartPos.y + .01f, heartPos.z);
            rightHeart.transform.position = new Vector3(heartPos.x - .035f, heartPos.y - .02f, heartPos.z);
            win = true;
        }
        heart.transform.position = new Vector3(10, 10, 0);
    }

    float time;

    private void Update()
    {
        if(click)
        {
            time += Time.deltaTime / lerpTime;
            if((win && leftHeart.transform.position == lPos && rightHeart.transform.position == rPos) || (!win && leftHeart.transform.localScale == new Vector3(1,1,1)))
            {
                gameObject.SetActive(false);
                otherCanvas.SetActive(true);
            }
            leftHeart.transform.rotation = Quaternion.Lerp(lRot, Quaternion.Euler(0, 0, 0), time);
            rightHeart.transform.rotation = Quaternion.Lerp(rRot, Quaternion.Euler(0, 0, 0), time);
            if(win)
            {
                leftHeart.transform.position = Vector3.Lerp(lCurrent, lPos, time);
                rightHeart.transform.position = Vector3.Lerp(rCurrent, rPos, time);
            }
            else
            {
                leftHeart.transform.localScale = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(1, 1, 1), time);
                rightHeart.transform.localScale = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(1, 1, 1), time);
            }
        }
    }
}
