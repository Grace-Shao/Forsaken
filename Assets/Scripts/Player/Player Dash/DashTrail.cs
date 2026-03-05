using UnityEngine;
using System.Collections.Generic;

public class DashTrail : MonoBehaviour
{

    [SerializeField] private GameObject trailMarker;
    [SerializeField] private int numMarkers = 5;
    [SerializeField] private float timeBetweenMarkers = 0.1f;
    [SerializeField] private int dashDamage = 5;

    private float timeSinceLastMarker;
    private int curMarkers = 0;
    private bool isDrawingTrail = false;

    private List<GameObject> markers;
    private Vector3 direction;

    public bool IsDrawingTrail {get { return isDrawingTrail; } set {isDrawingTrail = value;}}
    public Vector3 Direction {get {return direction;} set {direction = value;}}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        markers = new List<GameObject>();
        timeSinceLastMarker = 0f;
        curMarkers = 0;
    }

    public void Clear()
    {
        foreach (GameObject marker in markers) {
            Destroy(marker);
        }
        markers = new List<GameObject>();
        timeSinceLastMarker = 0f;
        curMarkers = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDrawingTrail)
        {
            if (curMarkers == 0 || timeSinceLastMarker > timeBetweenMarkers)
            {
                timeSinceLastMarker = 0;
                curMarkers ++;
                Vector3 position = new Vector3(transform.position.x, transform.position.y, 0f);
                GameObject marker = Instantiate(trailMarker, position, Quaternion.identity);
                //marker.transform.SetParent(transform);
                float facing = direction.x * 0.15f;
                marker.transform.localScale = new Vector3(facing, 0.15f, 0.15f);
                marker.SetActive(true);
                markers.Add(marker);
            }
            timeSinceLastMarker += Time.deltaTime;
        }
        if (curMarkers >= numMarkers)
        {
            GameObject markerToRemove = markers[0];
            markers.RemoveAt(0);
            Destroy(markerToRemove);
            curMarkers--;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        string layer = LayerMask.LayerToName(other.gameObject.layer);
        if (layer.Equals("Enemies"))
        {
            // other.gameObject.GetComponent<IDamageable>().ApplyDamage(dashDamage);
        }
    }
}
