using UnityEngine;

class FieldOfVision : MonoBehaviour
{

    [SerializeField] private float fov = 60f;
    [SerializeField] private float viewDistance = 5f;
    void Start() {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        Vector3 origin = Vector3.zero;
        Vector3 triangleMiddle = origin + GetVectorFromAngle(-fov / 2) * viewDistance;
        triangleMiddle.y = -0.01f;
        int rayCount = 20;
        float angle = 0f;
        float angleIncrease = fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 3]; // +3 for the origin the bootom, and the last vertex, 
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 6];

        vertices[0] = origin;
        
        int vertexIndex = 1;
        int triangleIndex = 0;

        for (int i = 0; i < rayCount; i++) {
            Vector3 vertex = origin + GetVectorFromAngle(angle) * viewDistance;
            vertices[vertexIndex] = vertex;

            if (i > 0) {
                triangles[triangleIndex] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }

        // Add the bottom vertex to make a pyramid
        vertices[vertexIndex] = triangleMiddle;

        for (int i = 0; i < rayCount; i++) {
            triangles[triangleIndex] = vertexIndex;
            triangles[triangleIndex + 1] = i;
            triangles[triangleIndex + 2] = i + 1;

            triangleIndex += 3;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    private Vector3 GetVectorFromAngle(float angle) {
        // angle = 0 -> 360
        float angleRad = angle * (Mathf.PI / 180);
        return new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad));
    }
}