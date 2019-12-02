using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector2 cameraPoint;
    public float zoom;
    public bool cameraLock = false;
    public float speedDamp;
    public float zoomDamp;

    public Camera cam;
    public BoxCollider2D Bounds;
    Vector2 origin;

    float edgeSize = 100f;
    float moveSpeed = 7f;
    float zoomSpeed = 0.5f;

    float vertExtent;
    float horzExtent;


    // Start is called before the first frame update
    void Start()
    {
        cameraPoint = transform.position;
        zoom = cam.orthographicSize;

        origin = Bounds.transform.position;
         
        vertExtent = cam.orthographicSize;
        horzExtent = vertExtent * Screen.width / Screen.height;
    }
    // Update is called once per frame
    void Update()
    {
        vertExtent = zoom;
        horzExtent = vertExtent * Screen.width / Screen.height;

        cameraPoint = GetMidPointPlayers();

        cameraPoint.x = Mathf.Clamp(cameraPoint.x, origin.x-Bounds.size.x / 2 + horzExtent, origin.x+Bounds.size.x / 2 - horzExtent);
        cameraPoint.y = Mathf.Clamp(cameraPoint.y, origin.y-Bounds.size.y / 2 + vertExtent, origin.y+Bounds.size.y / 2 - vertExtent);
        

        zoom = GetFurthestDistanceBetweenPlayers();
        zoom = Mathf.Clamp(zoom, 10, Mathf.Min(20,Bounds.size.y/2,Bounds.size.x/2));

        float targetzoom = Mathf.Lerp(cam.orthographicSize, zoom, Time.deltaTime * zoomDamp);
        cam.orthographicSize = targetzoom;

        Vector2 targetpos = Vector2.Lerp(transform.position, cameraPoint, Time.deltaTime * speedDamp);
        transform.position = new Vector3(targetpos.x, targetpos.y, transform.position.z);
    }

    Vector2 GetMidPointPlayers()
    {
        Vector2 midpoint= Vector2.zero;

        foreach(PlayerData pd in CharacterSpawn.PlayerSpawnedList)
        {
            int sizeList=CharacterSpawn.PlayerSpawnedList.Count;
            Vector2 playerPos = pd.playerObject.transform.position;

            midpoint.x += playerPos.x / sizeList;
            midpoint.y += playerPos.y / sizeList;
        }
        
        return midpoint;
    }

    float GetFurthestDistanceBetweenPlayers()
    {
        float furthestDistance = 5;

        foreach (PlayerData pd1 in CharacterSpawn.PlayerSpawnedList)
        {
            foreach (PlayerData pd2 in CharacterSpawn.PlayerSpawnedList)
            {
                if(pd1!=pd2)
                {
                    Vector2 player1Pos = pd1.playerObject.transform.position;
                    Vector2 player2Pos = pd2.playerObject.transform.position;

                    if(Vector2.Distance(player1Pos,player2Pos)>furthestDistance)
                    {
                        furthestDistance = Vector2.Distance(player1Pos, player2Pos);
                    }
                }
            }
        }

        return furthestDistance;
    }
}