using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouette : MonoBehaviour
{
    [SerializeField]GameObject v_PrefabObjectSpawn;
    float v_Speed;
    float v_TimeBeforeObjectSpawn;
    float v_TimeBeforeActivate;
    bool isActive;
    bool objectLaunched;
    bool flag;
    Rigidbody2D rbd;

    private void Awake()
    {
        rbd = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
       
       

    }

    public void Initialise(float p_Speed, float p_TimeBeforeActivate, float p_TimeBeforeObjectSpawn)
    {
        v_Speed = p_Speed;
        v_TimeBeforeActivate = p_TimeBeforeActivate;
        v_TimeBeforeObjectSpawn = p_TimeBeforeObjectSpawn;
        StartCoroutine(WaitForActivation());
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            Move();
            if (!flag && !objectLaunched)
            {
                StartCoroutine(WaitForObjectLaunch());
            }
        }
    }
    private void Move()
    {
        rbd.velocity = v_Speed * Vector2.left;
    }
    IEnumerator WaitForActivation()
    {
        yield return new WaitForSeconds(v_TimeBeforeActivate);
        isActive = true;
    }

    IEnumerator WaitForObjectLaunch()
    {
        flag = true;
        yield return new WaitForSeconds(v_TimeBeforeObjectSpawn);
        LaunchObject();
        
    }

    void LaunchObject()
    {
        Instantiate(v_PrefabObjectSpawn, transform.position, Quaternion.identity);
        objectLaunched = true;
    }

    void Death()
    {
        //Play animation éventuelle puis destroy
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != 12)
        {
            if (collision.gameObject.tag == "CarryAble" || collision.gameObject.tag == "Player")
            {

                Death();
            }
        }
       
        
    }
}
