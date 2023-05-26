using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(LineRenderer))]
public class TentacleTypeTwo : MonoBehaviour
{
    // BODY PARTS;
    [SerializeField] private Transform headTransform;
    [SerializeField] private Transform bodyParentTransform;
    [SerializeField] private GameObject bodyObject;
    [SerializeField] private GameObject tailObject;
    private Transform[] bodyTransforms;
    private Transform tailTransform;

    // COMPONENTS
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
    [SerializeField] [Range( 1.0f, 10.0f )] private float smoothSpeed = 3f;

    // WIGGLE
    [Header("Wiggle")]
    protected Transform wiggleDirection;
    [SerializeField] bool shouldWiggle = false;
    [SerializeField] [Range( 0.0f, 40.0f )] private float wiggleSpeed = 10;
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

        if( bodyObject != null ) {
            bodyTransforms = new Transform[ segmentCount ];
            for( int i = 0; i < segmentCount; i++ ) {
                GameObject bodyPart = Instantiate( original: bodyObject, position: Vector3.zero, rotation: Quaternion.identity, parent: bodyParentTransform ?? transform );
                bodyTransforms[ i ] = bodyPart.transform;
                
            }

            SpriteRenderer bodySpriteRenderer = bodyObject.GetComponent<SpriteRenderer>();
            if( bodySpriteRenderer != null ) {
                Sprite bodySprite = bodySpriteRenderer.sprite;
                if( bodySprite != null ) {
                    segmentDistance = bodySprite.bounds.size.x / 2;
                }
            }
        }

        if( tailObject != null ) {
            GameObject tailPart = Instantiate( original: tailObject, position: Vector3.zero, rotation: Quaternion.identity, parent: bodyParentTransform ?? transform );
            tailTransform = tailPart.transform;
        }

        setInitialOrientation();
    }

    void Update() {
        // SET WIGGLING
        if(shouldWiggle)
            wiggleDirection.localRotation = Quaternion.Euler( 0, 0, Mathf.Sin( ( Time.time + randomWiggleOffset ) * wiggleSpeed ) * wiggleMagnitude );

        // SET SEGMENT POSITIONS
        segmentPositions[ 0 ] = mainDirection.position;

        for( int i = 1; i < segmentPositions.Length; i++ ) {
            Vector3 targetPosition = segmentPositions[ i - 1 ] + ( segmentPositions[ i ] - segmentPositions[ i - 1 ] ).normalized * segmentDistance;
            segmentPositions[ i ] = Vector3.SmoothDamp( segmentPositions[ i ], targetPosition, ref segmentVelocities[i], smoothSpeed * Time.deltaTime );
        }
        setBody();

        // SET SEGMENTS AS LINE-RENDERER POSITIONS
        lineRenderer.SetPositions( segmentPositions );
    }

    void setBodyTransform( Transform bodyTransform, Vector3 position, Vector3 lookPosition ) {
        // SET POSITION
        bodyTransform.position = position;

        // SET ROTATION
        Vector3 dir = lookPosition - bodyTransform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        bodyTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void setBody() {
        if( bodyObject != null ) {
            setBodyTransform(
                bodyTransform: bodyTransforms[ 0 ],
                position: segmentPositions[ 0 ],
                lookPosition: headTransform != null ? headTransform.position : transform.position
            );
            for( int i = 1; i < segmentPositions.Length; i++ )
                setBodyTransform(
                    bodyTransform: bodyTransforms[ i ],
                    position: segmentPositions[ i ],
                    lookPosition: segmentPositions[ i - 1 ]
                );
        }
        if( tailObject != null ) {
            setBodyTransform(
                bodyTransform: tailTransform,
                position: segmentPositions[ segmentPositions.Length - 1 ],
                lookPosition: segmentPositions[ segmentPositions.Length - 2 ]
            );
        }
    }
    
    void setInitialOrientation() {
        segmentPositions[ 0 ] = mainDirection.position;

        for( int i = 1; i < segmentPositions.Length; i++ ) {
            segmentPositions[ i ] = segmentPositions[ i - 1 ] + mainDirection.right * segmentDistance;
        }

        setBody();
    }
}
