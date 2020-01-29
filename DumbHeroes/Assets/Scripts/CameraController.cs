using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector2 cameraPoint;
    float zoom;
    public bool cameraLock = false;
    public float speedDamp=1;
    public float zoomDamp=1;
    public float zoomMax=25;
    public float zoomMin = 15;
    float CameraHeight;
    public BoxCollider2D Bounds;
    Vector2 origin;

    

   


    // Start is called before the first frame update
    void Start()
    {
        cameraPoint = transform.position;
        zoom = Camera.main.orthographicSize;

        origin = Bounds.transform.position + (Vector3)Bounds.offset;
         
        
        
    }
    // Update is called once per frame
    void Update()
    {
       
        cameraPoint = GetMidPointPlayers();

       
        

        zoom = GetFurthestDistanceBetweenPlayers()/Camera.main.aspect;
        zoom = Mathf.Clamp(zoom, zoomMin, zoomMax);

        float targetzoom = Mathf.Lerp(Camera.main.orthographicSize, zoom, Time.deltaTime * zoomDamp);
        Camera.main.orthographicSize = targetzoom;
        float CameraWidth = Camera.main.orthographicSize * Camera.main.aspect;

        float Width = Mathf.Max(0, Bounds.size.x - CameraWidth*2);
        float Height = Mathf.Max(0, Bounds.size.y - Camera.main.orthographicSize*2);

        cameraPoint.x = Mathf.Clamp(cameraPoint.x, origin.x - Width/ 2, origin.x + Width / 2 );
        cameraPoint.y = Mathf.Clamp(cameraPoint.y, origin.y - Height / 2, origin.y + Height / 2);

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