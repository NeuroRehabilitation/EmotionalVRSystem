using UnityEngine;

public class RayCastHitVertex : MonoBehaviour
{
    private int[] lastTriangle;

    public bool firstTime=true;
    public Vector3[] vertices;
    public int[] triangles;
    public Mesh mesh;
    public LayerMask mask;
    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit,1000f, mask))
        {
            
            MeshCollider meshCollider = hit.collider as MeshCollider;
            if (meshCollider == null || meshCollider.sharedMesh == null)
                return;

            
      
            if (firstTime == true && meshCollider.name=="Terrain(Clone)")
            {
                
                firstTime = false;
                mesh = meshCollider.sharedMesh;
                vertices = mesh.vertices;
                triangles = mesh.triangles;
            }
            
            
             
            

            Vector3 p0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
            Vector3 p1 = vertices[triangles[hit.triangleIndex * 3 + 1]];
            Vector3 p2 = vertices[triangles[hit.triangleIndex * 3 + 2]];
            Transform hitTransform = hit.collider.transform;
            p0 = hitTransform.TransformPoint(p0);
            p1 = hitTransform.TransformPoint(p1);
            p2 = hitTransform.TransformPoint(p2);

            // Color the hit triangle red
            Color[] colors = mesh.colors;

            if (colors.Length == 0 )
            {
                
                
                colors = new Color[vertices.Length];
                for (int i = 0; i < colors.Length; i++)
                {
                    colors[i] = Color.white;
                }
            }

            if (lastTriangle != null)
            {
                Color brown = new Color(0.64f, 0.16f, 0.16f);
                colors[lastTriangle[0]] = brown;
                colors[lastTriangle[1]] = brown;
                colors[lastTriangle[2]] = brown;
            }
            colors[triangles[hit.triangleIndex * 3 + 0]] = Color.red;
            colors[triangles[hit.triangleIndex * 3 + 1]] = Color.red;
            colors[triangles[hit.triangleIndex * 3 + 2]] = Color.red;
            mesh.colors = colors;
            lastTriangle = new int[] { triangles[hit.triangleIndex * 3 + 0], triangles[hit.triangleIndex * 3 + 1], triangles[hit.triangleIndex * 3 + 2] };

            // Draw the raycast
            Debug.DrawRay(transform.position, Vector3.down * hit.distance, Color.yellow);

            // Draw lines to visualize the triangle in the editor
            Debug.DrawLine(p0, p1, Color.red);
            Debug.DrawLine(p1, p2, Color.red);
            Debug.DrawLine(p2, p0, Color.red);
        }
    }
}
