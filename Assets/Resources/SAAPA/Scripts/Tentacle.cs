using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(LineRenderer))]
public class Tentacle : MonoBehaviour
{
    // COMPONENTS
    [SerializeField] private GameObject bodyObject;
    [SerializeField] private Transform tailObject;
    private LineRenderer lineRenderer;

    // VARIABLES
    // SEGMENT
    [Header("Segment")]
    [SerializeField] protected Transform mainDirection;
    [SerializeField] [Range( 15, 50 )] private int segmentCount = 40;
    [SerializeField] [Range( 0.05f, 0.6f )] private float segmentDistance = 0.1f;
    private Vector3[] segmentPositions;
    private Vector3[] segmentVelocities;
    
    // SPEED
    [Header("Speed")]
    [SerializeField] [Range( 1.0f, 6.0f )] private float smoothSpeed = 2;
    // [SerializeField] [Range( 0, 5000 )] private float trailSpeed = 0;

    // WIGGLE
    [Header("Wiggle")]
    protected Transform wiggleDirection;
    [SerializeField] [Range( 5.0f, 40.0f )] private float wiggleSpeed = 10;
    [SerializeField] [Range( 0.0f, 120.0f )] private float wiggleMagnitude = 60;
    private float randomWiggleOffset;


    void Awake() {
        randomWiggleOffset = Random.Range( 0, 90 );

        lineRenderer = GetComponent<LineRenderer>();
        wiggleDirection = transform.parent;
    }

    void Start() {
        lineRenderer.positionCount = segmentCount;
        segmentPositions = new Vector3[ segmentCount ];
        segmentVelocities = new Vector3[ segmentCount ];
    }

    void Update() {
        // SET WIGGLING
        wiggleDirection.localRotation = Quaternion.Euler( 0, 0, Mathf.Sin( ( Time.time + randomWiggleOffset ) * wiggleSpeed ) * wiggleMagnitude );

        // SET SEGMENT POSITIONS
        segmentPositions[ 0 ] = mainDirection.position;
        for( int i = 1; i < segmentPositions.Length; i++) {
            segmentPositions[i] = Vector3.SmoothDamp(
                segmentPositions[ i ],
                segmentPositions[ i-1 ] + mainDirection.right * segmentDistance,
                ref segmentVelocities[ i ],
                smoothSpeed * Time.deltaTime// + ( trailSpeed == 0 ? 0 : ( i/trailSpeed ) )
            );
        }

        // SET SEGMENTS AS LINE-RENDERER POSITIONS
        lineRenderer.SetPositions( segmentPositions );
    }
}
