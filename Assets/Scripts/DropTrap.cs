using System.Collections;
using UnityEngine;

public class DropTrap : MonoBehaviour
{
    [SerializeField] private GameObject[] wayPoints;
    [SerializeField] private Animator anim;
    public float SpeedDrop = 2.5f;
    private int currentWaypointIndex = 0;


    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetTrigger("OpenEye");
    }

    void Update()
    {
        StartCoroutine(DropBlink());
    }

    public IEnumerator DropBlink()
    {
        Vector3 target = wayPoints[currentWaypointIndex].transform.position;
        transform.position = Vector2.MoveTowards(transform.position,target, SpeedDrop * Time.deltaTime);

        if (transform.position == wayPoints[currentWaypointIndex].transform.position)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= wayPoints.Length)
            {
                currentWaypointIndex =0;
            }
        }
        if(transform.position == wayPoints[1].transform.position)
        {
            anim.SetTrigger("Drop");
            SpeedDrop = 2.5f;
        }
        if(transform.position == wayPoints[0].transform.position)
        {
            anim.SetTrigger("OpenEye");
            SpeedDrop = 8f;
        }
        yield return new WaitForSeconds(0.5f);
    }
}
