using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoatBehaviour : MonoBehaviour
{
    public float boatProgress;
    public float boatDistance;
    public float coefRowing;
    public float sinkingCoef;
    public float Slowfactor;
    public Slider progressBar;
    public GameObject DockStart;
    public GameObject DockFinish;
    public ParticleSystem speedPartObj;
    ParticleSystem.MainModule speedPart;
    ParticleSystem.EmissionModule speedPartEm;
    public Animator Scrolling;

    Vector2 startingPos;

    public List<GameObject> burdenItem =  new List<GameObject>();
    [SerializeField]
    float boatSpeed = 0f;



    // Start is called before the first frame update
    void Start()
    {
        startingPos = this.transform.position;
        speedPart = speedPartObj.main;
        speedPartEm = speedPartObj.emission;
    }

    // Update is called once per frame
    void Update()
    {
        boatSpeed -= Slowfactor * Time.deltaTime * Mathf.Max(burdenItem.Count/3,1);
        boatSpeed = Mathf.Clamp(boatSpeed, 0, 1);
        speedPart.simulationSpeed = Mathf.Max(speedPart.simulationSpeed, boatSpeed);
        speedPartEm.rateOverTime = Mathf.Lerp(0, 50, boatSpeed);
        Scrolling.speed = boatSpeed;

        boatProgress += boatSpeed;
        boatProgress = Mathf.Clamp(boatProgress, 0, boatDistance);
        progressBar.value = boatProgress / boatDistance;

        DockStart.transform.position = new Vector2(DockStart.transform.position.x - boatSpeed/3, DockStart.transform.position.y);
        if(boatProgress == boatDistance && DockFinish.transform.position.x > 0)
        {
            DockFinish.transform.position = new Vector2(DockFinish.transform.position.x - boatSpeed / 3, DockFinish.transform.position.y);
        }
    }

    public void RowTheBoat(float rowingValue)
    {
        boatSpeed += rowingValue*coefRowing/ Mathf.Max(burdenItem.Count / 3, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.gameObject.tag == "CarryAble")
        {
            if (!burdenItem.Contains(collision.gameObject))
            {
                burdenItem.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "CarryAble")
        {
            if (burdenItem.Contains(collision.gameObject))
            {
                burdenItem.Remove(collision.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "CarryAble")
        {
            //SFXManager.Instance.BoatLevel[4].Play();
        }
    }
}
