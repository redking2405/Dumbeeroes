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
    float edgeSize = 100f;
    float moveSpeed = 7f;
    float zoomSpeed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        cameraPoint = transform.position;
        zoom = cam.orthographicSize;
    }
    // Update is called once per frame
    void Update()
    {

        float vertExtent = cam.orthographicSize;
        float horzExtent = vertExtent * Screen.width / Screen.height;
        /*
        cameraPoint.x = Mathf.Clamp(cameraPoint.x, -Bounds.size.x / 2 + horzExtent, Bounds.size.x / 2 - horzExtent);
        cameraPoint.y = Mathf.Clamp(cameraPoint.y, -Bounds.size.y / 2 + vertExtent, Bounds.size.y / 2 - vertExtent);
        */
        cameraPoint = GetMidPointPlayers();
        zoom = Mathf.Clamp(zoom, 5, 15);

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
}