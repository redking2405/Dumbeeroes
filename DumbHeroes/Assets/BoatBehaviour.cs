using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatBehaviour : MonoBehaviour
{
    public float boatProgress = 0f;
    public float dampProgress = 1f;
    public float coefRowing = 1f;
    public float sinkingCoef = 1f;

    Vector2 startingPos;

    public int burdenItem = 0;
    float progressValue = 0f;



    // Start is called before the first frame update
    void Start()
    {
        startingPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ManageProgress();
        ManageSinking();
    }

    public void RowTheBoat(float rowingValue)
    {
        progressValue += rowingValue * dampProgress / (1 + burdenItem);
    }

    public void AddItem()
    {
        burdenItem += 1;
    }

    public void RemoveItem()
    {
        if(burdenItem>=0)
        {
            burdenItem -= 1;
        }
    }

    void ManageProgress()
    {
        if (boatProgress < 100f)
        {
            boatProgress = Mathf.Lerp(boatProgress, progressValue, Time.deltaTime * dampProgress);
        }
        else
        {
            boatProgress = 100f;
        }
    }

    void ManageSinking()
    {
        float sinkingY = Mathf.Lerp(this.transform.position.y, startingPos.y - sinkingCoef * burdenItem, dampProgress * Time.deltaTime);

        Debug.Log(startingPos.y - sinkingCoef * burdenItem);
        this.transform.position = new Vector2(this.transform.position.x, sinkingY);
    }
}
