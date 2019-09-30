using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : MonoBehaviour
{
    [Header("Controller")]
    public int controllerNum = 0;

    private IWormController control;

    [Header("Settings")]
    [SerializeField] private int numSegments = 4;
    [SerializeField] private float segmentLength = 1.5f;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float gravity = 1f;

    [Header("Prefabs")]
    [SerializeField] private GameObject head;
    private Cutter wormEater;
    [SerializeField] private GameObject segment;
    private List<GameObject> segments = new List<GameObject>();
    [SerializeField] private GameObject tail;
    private List<Vector3> trail = new List<Vector3>();
    
    void Start()
    {
        // temp
        control = new PlayerWormController(controllerNum);

        wormEater = head.GetComponent<Cutter>();
        
        segments.Add(segment);
        for(var i = 1; i < numSegments; i++)
        {
            segments.Add(Instantiate(segment, segment.transform.parent));
        }

        trail = new List<Vector3> {head.transform.position};
    }
    
    void Update()
    {
        var moveDir = control.GetMoveDirection();
        head.transform.position += new Vector3(moveDir.x, moveDir.y, 0) * Time.deltaTime * moveSpeed;
        
        if ((head.transform.position - trail[trail.Count - 1]).magnitude > 0.1)
        {
            trail.Add(head.transform.position);

            var totalLength = 0f;
            var lastPos = trail[trail.Count - 1];
            for (var i = trail.Count - 2; i <= 0; i++)
            {
                if (totalLength > segmentLength * (numSegments + 2))
                {
                    trail.RemoveAt(i);
                }
                else
                {
                    var pos = trail[i];
                    totalLength += (lastPos - pos).magnitude;
                }
            }
        }
    }
}
