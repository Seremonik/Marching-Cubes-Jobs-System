using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;

[RequireComponent(typeof(MeshFilter))]
public class Chunk : MonoBehaviour
{
    private MeshFilter meshFilter;
    private NativeArray<byte> gridData;
    private NativeList<float3> vertices;
    private NativeList<int> triangles;
    private Mesh mesh;
    
    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;
    }
    
    public void Init(NativeArray<byte> gridData, NativeList<float3> vertices, NativeList<int> triangles)
    {
        this.gridData = gridData;
        this.vertices = vertices;
        this.triangles = triangles;
    }

    public void GenerateMesh()
    {
        mesh.Clear();

        mesh.SetVertices(vertices.AsArray());
        mesh.triangles = triangles.ToArray(); 
        mesh.RecalculateNormals();
        mesh.Optimize();
        mesh.OptimizeIndexBuffers();
        mesh.OptimizeReorderVertexBuffer();
    }

    private void OnDestroy()
    {
        Destroy(meshFilter.sharedMesh);
        gridData.Dispose();
        vertices.Dispose();
        triangles.Dispose();
    }
}
