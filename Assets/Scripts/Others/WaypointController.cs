using System.Collections.Generic;
using UnityEngine;

public class WaypointController : MonoBehaviour
{
    [Header("--- Misc Settings ---")]
    [SerializeField] public GameObject waypointPrefab;
    [SerializeField][HideInInspector] private List<Transform> waypoints = new List<Transform>();

    [Header("--- Waypoint Settings ---")]
    [SerializeField] public int numberOfPoints = 0;
    [SerializeField] public float radius = 0F;
    [SerializeField] public float expandSpeed = 0F;
    [SerializeField] public float minRadius = 0F;
    [SerializeField] public float maxRadius = 0F;

    [SerializeField][HideInInspector] private bool expanding = true;

    void Start()
    {
        GenerateWaypoints();
    }

    void Update()
    {
        AnimateRadius();
        UpdateWaypointPositions();
    }

    public void GenerateWaypoints()
    {
        for (int i = 0; i < numberOfPoints; i++)
        {
            GameObject wp = Instantiate(waypointPrefab, transform.position, Quaternion.identity);
            wp.transform.parent = transform;
            waypoints.Add(wp.transform);
        }
    }

    void AnimateRadius()
    {
        if (expanding)
        {
            radius += expandSpeed * Time.deltaTime;
            if (radius >= maxRadius)
                expanding = false;
        }
        else
        {
            radius -= expandSpeed * Time.deltaTime;
            if (radius <= minRadius)
                expanding = true;
        }
    }

    void UpdateWaypointPositions()
    {
        for (int i = 0; i < numberOfPoints; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfPoints;
            Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            waypoints[i].position = (Vector2)transform.position + offset;
        }
    }

    public List<Transform> GetWaypoints() {  return waypoints; }
}