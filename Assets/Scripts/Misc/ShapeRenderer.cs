using UnityEngine;

[ RequireComponent ( typeof ( LineRenderer ) ) ]
public class ShapeRenderer : MonoBehaviour
{
    private LineRenderer lineRenderer;

    private void Awake ( ) => lineRenderer = GetComponent<LineRenderer> ( );

    private void SetLineProperties ( Color startColor, Color endColor, float startLineWidth, float endLineWidth ) {
        if ( lineRenderer == null )
            lineRenderer = GetComponent<LineRenderer> ( );

        // lineRenderer.startColor = startColor;
        // lineRenderer.endColor = endColor;

        Gradient gradient = new ( );
        GradientColorKey[ ] colorKey = new GradientColorKey[ 1 ];
        colorKey[ 0 ].color = startColor;
        colorKey[ 0 ].time = 0.0f;
        GradientAlphaKey[ ] alphaKey = new GradientAlphaKey[ 1 ];
        alphaKey[ 0 ].alpha = 1.0f;
        alphaKey[ 0 ].time = 0.0f;
        gradient.SetKeys( colorKey, alphaKey );
        lineRenderer.colorGradient = gradient;

        lineRenderer.startWidth = startLineWidth;
        lineRenderer.endWidth = endLineWidth;
    }

    public void DrawQuad ( Vector3 center, float width, float height, Color color, float lineWidth ) {
        Vector3[] vertexPositions = new Vector3[] {
            new Vector2 ( center.x - width/2, center.y + height/2 ),
            new Vector2 ( center.x + width/2, center.y + height/2 ),
            new Vector2 ( center.x + width/2, center.y - height/2 ),
            new Vector2 ( center.x - width/2, center.y - height/2 )
        };

        DrawPolygon( vertexPositions, color, lineWidth );
    }

    public void DrawPolygon ( Vector3[] vertexPositions, Color color, float lineWidth ) {
        SetLineProperties ( startColor: color, endColor: color, startLineWidth: lineWidth, endLineWidth: lineWidth );

        lineRenderer.positionCount = vertexPositions.Length + 1;
        lineRenderer.SetPositions ( vertexPositions );
        lineRenderer.SetPosition ( vertexPositions.Length, vertexPositions[ 0 ] );
    }

    public void DrawCircle ( Vector3 center, float radius, Color color, float lineWidth ) {
        SetLineProperties ( startColor: color, endColor: color, startLineWidth: lineWidth, endLineWidth: lineWidth );

        var segments = 360;

        lineRenderer.positionCount = segments + 1;

        var pointCount = segments + 1; 
        
        var points = new Vector3[ pointCount ];
        for ( int i = 0; i < pointCount; i++ ) {
            var rad = Mathf.Deg2Rad * ( i * 360f / segments );
            points[ i ] = new Vector3 ( center.x + Mathf.Sin ( rad ) * radius, center.y + Mathf.Cos ( rad ) * radius, center.z );
        }

        lineRenderer.SetPositions ( points );
    }

    public void DrawEllipse ( Vector3 center, float xAxis, float yAxis, Color color, float lineWidth ) {
        SetLineProperties ( startColor: color, endColor: color, startLineWidth: lineWidth, endLineWidth: lineWidth );

        var segments = 360;

        lineRenderer.positionCount = segments + 1;

        var pointCount = segments + 1; 
        
        var points = new Vector3[ pointCount ];
        for ( int i = 0; i < pointCount; i++ ) {
            var rad = Mathf.Deg2Rad * ( i * 360f / segments );
            points[ i ] = new Vector3 ( center.x + Mathf.Sin ( rad ) * xAxis, center.y + Mathf.Cos ( rad ) * yAxis, center.z );
        }

        lineRenderer.SetPositions ( points );
    }
}
