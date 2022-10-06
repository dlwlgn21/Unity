using System.Collections;
using UnityEngine;
using Pathfinding;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(Seeker))]
public class EnemyAI : MonoBehaviour
{
    public Transform target;

    // How many time each second update it will update it's path
    public float updateRate;

    [HideInInspector]
    public bool isPathEnded = false;

    // The Calculated Path
    public Path path;

    // The AI's speed per second
    public float speed;
    public ForceMode2D forceMode;

    // The max distance from the AI to waypoint for it to continue to the next waypoint
    public float nextWayPointDistance;

    // Caching
    private Seeker seeker;
    private Rigidbody2D rb2D;

    // The waypoint it is currently moving towards
    private int currentWayPoint = 0;

    private bool isSearchingForPlayer = false;
    private float searchingDelay = 0.5f;

    // TODO : 겁나 찝찝한 코루틴 재귀호출.. 몬가..몬가 이상해. 이거 나중에 고쳐봐야할듯.
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb2D = GetComponent<Rigidbody2D>();
        
        if (target == null)
        {
            if (!isSearchingForPlayer)
            {
                isSearchingForPlayer = true;
                StartCoroutine(searchForPlayerRecursive());
            }
            return;
        }

        // Start a new path to the target position, return the result to the OnPathComplete method 
        seeker.StartPath(transform.position, target.position, OnPathComplete);

        StartCoroutine(UpdatePathRecursive());
    }

    private IEnumerator searchForPlayerRecursive()
    {
        GameObject searchResult = GameObject.FindGameObjectWithTag("Player");
        if (searchResult == null)
        {
            yield return new WaitForSeconds(searchingDelay);
            StartCoroutine(searchForPlayerRecursive());
        }
        else
        {
            target = searchResult.transform;
            StartCoroutine(UpdatePathRecursive());
            yield return false;
        }
        
    }

    private IEnumerator UpdatePathRecursive()
    {
        if (target == null)
        {
            if (!isSearchingForPlayer)
            {
                isSearchingForPlayer = true;
                StartCoroutine(searchForPlayerRecursive());
            }
            yield return false;
        }

        if (target != null)
        {
            seeker.StartPath(transform.position, target.position, OnPathComplete);
        }

        yield return new WaitForSeconds(1f / updateRate);

        StartCoroutine(UpdatePathRecursive());
    }

    public void OnPathComplete(Path paramPath)
    {
        Debug.Log($"Got a Path, Did it have an error? {paramPath.error}");
        if (!paramPath.error)
        {
            path = paramPath;
            currentWayPoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            if (!isSearchingForPlayer)
            {
                isSearchingForPlayer = true;
                StartCoroutine(searchForPlayerRecursive());
            }
            return;
        }

        //TODO: Always look at player?
        if (path == null)
        {
            return;
        }

        if (currentWayPoint >= path.vectorPath.Count)
        {
            if (isPathEnded)
            {
                return;
            }
            Debug.Log("End of path reached.");
            isPathEnded = true;
            return;
        }

        isPathEnded = false;

        // Direction to the next waypoint
        Vector3 dir = (path.vectorPath[currentWayPoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;

        // Move Enemy.
        rb2D.AddForce(dir, forceMode);

        float distance = Vector3.Distance(transform.position, path.vectorPath[currentWayPoint]);

        if (distance < nextWayPointDistance)
        {
            ++currentWayPoint;
            return;
        }

    }


}
