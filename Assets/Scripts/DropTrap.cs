using System.Collections;
using UnityEngine;

public class DropTrap : MonoBehaviour
{
    [SerializeField] private GameObject[] wayPoints;
    [SerializeField] private Animator anim;
    public float SpeedDrop = 2.5f;
    [SerializeField] private float timeDelay;
    private int currentWaypointIndex = 0;


    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        StartCoroutine(DropBlink());
    }

    public IEnumerator DropBlink()
    {
        anim.SetTrigger("OpenEye");
        if (Vector2.Distance(wayPoints[currentWaypointIndex].transform.position, transform.position) < .1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= wayPoints.Length)
            {
                currentWaypointIndex =0;
            }
            if(currentWaypointIndex == 0)
            {
                anim.SetTrigger("Drop");
            }

        }
        transform.position = Vector2.MoveTowards(transform.position, wayPoints[currentWaypointIndex].transform.position, SpeedDrop * Time.deltaTime);
       yield return null;
    }
}
