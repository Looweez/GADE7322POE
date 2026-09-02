using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class MeshGenerator : MonoBehaviour
{
    private Mesh mesh;
    
    Vector3[] vertices;
    private int[] triangles;
    //private Vector2[] uvs;
    private Color[] colors;

    public int xSize = 20;
    public int zSize = 20;

    public float offsetX;
    public float offsetZ;
    
    public Gradient gradient;

    private float minTerrainHeight;
    private float maxTerrainHeight;
    
    public System.Action OnMeshGenerated;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        
        offsetX = Random.Range(0f, 99999f);
        offsetZ = Random.Range(0f, 99999f);

        CreateShape();
    }

    private void Update()
    {
        UpdateMesh();
    }

    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise((x * 0.3f) + offsetX, (z * 0.3f) + offsetZ) * 2f;
                vertices[i] = new Vector3(x, y, z);
                
                if (y > maxTerrainHeight)
                    maxTerrainHeight = y;
                if (y < minTerrainHeight)
                    minTerrainHeight = y;
                
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;

        for (int i = 0; i < zSize; i++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;

            }

            vert++;
        }

        colors = new Color[vertices.Length];
        
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y);
                colors[i] = gradient.Evaluate(height);
                i++;
            }
        }
         UpdateMesh();
         
         OnMeshGenerated?.Invoke();

    }

    void UpdateMesh()
    {
        mesh.Clear();
        
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        
        mesh.RecalculateNormals();
        
        // adds mesh collider so that raycasts can detect when mouse is hovering over mesh for placement of defenders
        if (!TryGetComponent<MeshCollider>(out MeshCollider meshCollider))
        {
            meshCollider = gameObject.AddComponent<MeshCollider>();
        }
        meshCollider.sharedMesh = mesh;
    }

    public float GetTerrainHeightAt(float worldX, float worldZ) //this will be used to find the center of the mesh so the tower can spawn there 
    {
        float height = Mathf.PerlinNoise((worldX * 0.3f) + offsetX, (worldZ * 0.3f) + offsetZ) * 2f;
        return height;
    }
    
    /*private void OnDrawGizmos()
    {
        if (vertices == null)
            return;
        
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }*/
}
