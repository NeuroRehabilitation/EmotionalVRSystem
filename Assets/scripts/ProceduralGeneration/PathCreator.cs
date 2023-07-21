using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class PathCreator : MonoBehaviour
{

    [Header("General fields")]
    public GameObject finalDoor;
    public GameObject Anchor;
    public Transform AnchorsParent;


    //public GameObject anchor;

    [Header("Agent and navigation fields")]
    private List<Vector3> lastPositions = new List<Vector3>();
    public Vector3 pointPosition;
    public Vector3 pointPositionLate;
    public NavMeshAgent myNavMeshAgent;
    //public GameObject destination;
    public List<Transform> waypoints = new List<Transform>();
    public int currentWaypoint = 0;

    [Header("Milestones and environment objects")]
    public GameObject grass;
    public GameObject rock;
    public GameObject bush;
    public GameObject grassParent;
    public GameObject rockParent;
    public GameObject bushParent;

    [Header("State and timing variables")]
    public int x = 0;
    public int milestoneIncrementCounter;
    public bool finished = false;
    public float TimeT;
    public float TimeTotal;
    private float AngleBetweenPathmakerPositions;

    public Countdown Countdown;
    private bool countdownStarted = false;


    // Destroy grass objects upon collision to keep the path clear

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Waypoint") && ShouldDrawPath())
        {

            if (currentWaypoint < waypoints.Count-1)
            {
                currentWaypoint++;
                myNavMeshAgent.SetDestination(waypoints[currentWaypoint].position);
                
            }
        }

        if (other.gameObject.CompareTag("grass"))
        {
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("FinishPrimary"))
        {
            OnFinishPrimary(other);
        }
    }

    private void OnFinishPrimary(Collider collision)
    {
        finished = true;
        Destroy(collision.gameObject);
        StartCoroutine(FinalPathPreparations(1));
        TimeTotal = 0;
    }

    public void DrawPath()
    {
        if (!myNavMeshAgent.hasPath)
        {
            myNavMeshAgent.SetDestination(waypoints[currentWaypoint].position);
        }

        if (myNavMeshAgent.path.corners.Length < 2)
        {
            return;
        }
    }

    private Transform FindChildRecursive(Transform parent, string targetName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == targetName)
            {
                return child;
            }

            Transform foundChild = FindChildRecursive(child, targetName);
            if (foundChild != null)
            {
                return foundChild;
            }
        }

        return null;
    }

    // Clean up the scene after completion and spawn the final door
    private IEnumerator FinalPathPreparations(int secs)
    {
        yield return new WaitForSeconds(secs);

        CleanUpScene();
        //SpawnFinalDoor();

        gameObject.SetActive(false);
    }

    private void CleanUpScene()
    {
        foreach (GameObject grass in GameObject.FindGameObjectsWithTag("grass"))
        {
            Destroy(grass.GetComponent<Collider>());
            Destroy(grass.GetComponent<Rigidbody>());
        }
        //GameObject.Find("BridgeRock").transform.position = new Vector3(GameObject.Find("BridgeRock").transform.position.x, GameObject.Find("BridgeRock").transform.position.y + 1.0f, GameObject.Find("BridgeRock").transform.position.z);
    }

    private void SpawnFinalDoor()
    {
        GameObject finalDoorTemp = Instantiate(finalDoor, waypoints[currentWaypoint].position, transform.rotation);
        finalDoorTemp.GetComponent<DoorColliderDetection>().SpawnPos = new Vector3(-50f, 15f, -35f);
    }


    private void FixedUpdate()
    {
        // Update the total time elapsed
        //UpdateTime();
        //Debug.Log("Has Path = " + myNavMeshAgent.hasPath);
       
     
        // Check if the agent should draw the path and update accordingly
        if (ShouldDrawPath())
        {
            if (!countdownStarted)
            {
                countdownStarted = true;
                Countdown.StartCountdown();
            }
            UpdatePath();
        }
    }

    private void Start()
    {

        InvokeRepeating("PlaceAnchor", 0, 0.2f); //calls PlaceAnchor() every 0.2 sec

        GameObject[] taggedPoints = GameObject.FindGameObjectsWithTag("Waypoint");

        foreach(GameObject taggedPoint in taggedPoints)
        {
            waypoints.Add(taggedPoint.transform);
        }

    }
    

    void PlaceAnchor()
    {
        if (ShouldDrawPath())
        {
            Instantiate(Anchor, pointPosition, Quaternion.Euler(0, -AngleBetweenPathmakerPositions, 0), AnchorsParent);
        }

    }

    private void UpdateTime()
    {
        TimeTotal += Time.deltaTime;
    }

    // Determine if the agent should draw the path based on its navigation mesh and completion status
    private bool ShouldDrawPath()
    {
        return myNavMeshAgent.hasPath && !finished;
    }

    // Update the path and related objects based on the agent's current state

    public int counterTest;
    private void UpdatePath()
    {
        TimeT += Time.deltaTime;
        var randomRotation = GetRandomRotation();

        DrawPath();
        float AngleBetweenPathmakerPositions = UpdateAgentPosition();

        // Instantiate environment objects along the path
        //PlaceEnvironmentObjects(randomRotation, AngleBetweenPathmakerPositions);
    }

    // Generate a random rotation for instantiated objects
    private Quaternion GetRandomRotation()
    {
        return Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
    }


    // Update the agent's position and calculate the angle between its current and previous position
    private float UpdateAgentPosition()
    {
        pointPosition = transform.position;
        AngleBetweenPathmakerPositions = UpdateAngleBetweenPositions();
        pointPositionLate = pointPosition;

        // Add the current position to the list
        lastPositions.Add(pointPosition);

        // If the list has more than 10 positions, remove the oldest one
        if (lastPositions.Count > 10)
        {
            lastPositions.RemoveAt(0);
        }

        x++;
        milestoneIncrementCounter++;

        return AngleBetweenPathmakerPositions;
    }

    // Calculate the angle between the agent's current position and its previous position
    private float UpdateAngleBetweenPositions()
    {
        // If there are not enough positions in the list, return 0
        if (lastPositions.Count < 10)
        {
            return 0;
        }

        Vector3 positionFrom10IterationsBefore = lastPositions[0];

        return Mathf.Atan2(positionFrom10IterationsBefore.z - pointPosition.z, positionFrom10IterationsBefore.x - pointPosition.x) * 180 / Mathf.PI;
    }

    // Instantiate grass, rocks, and bushes along the path based on random probability
    private void PlaceEnvironmentObjects(Quaternion randomRotation, float AngleBetweenPathmakerPositions)
    {
        int randm = Random.Range(0, 41);

        if (randm <= 30)
        {
            bool isPositive = randm <= 15;
            Instantiate(grass, CalculateNewPosition(isPositive, AngleBetweenPathmakerPositions, 8.0f, 8.5f), randomRotation, grassParent.transform);

        }
        else if (randm >= 37)
        {
            GameObject objectToInstantiate = (randm == 38 || randm == 37) ? bush : rock;
            bool isPositive = randm == 38 || randm == 40;
            Instantiate(objectToInstantiate, CalculateNewPosition(isPositive, AngleBetweenPathmakerPositions, 8.0f, 8.5f), randomRotation, (randm == 38 || randm == 37) ? bushParent.transform : rockParent.transform);
        }
    }

    // Calculate the new position for an instantiated object based on the angle between the agent's positions
    private Vector3 CalculateNewPosition(bool isPositive, float AngleBetweenPathmakerPositions, float minRange, float maxRange)
    {
        float randomRange = Random.Range(minRange, maxRange) / Mathf.Cos((AngleBetweenPathmakerPositions * Mathf.PI) / 180);
        float zPosition = transform.position.z + (isPositive ? randomRange : -randomRange);
        return new Vector3(transform.position.x, transform.position.y + 10, zPosition);
    }
}
    