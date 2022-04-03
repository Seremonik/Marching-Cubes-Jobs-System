using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
public struct GenerateChunkJob : IJob
{
    [ReadOnly] public NativeArray<byte> Voxels;
    public NativeList<float3> Vertices;
    public NativeList<int> Triangles;

    [ReadOnly] public NativeArray<float3> VerticesShiftTable;
    [ReadOnly] public NativeArray<int> TriTable;
    [ReadOnly] public NativeArray<int> EdgeTable;
    [DeallocateOnJobCompletionAttribute] public NativeArray<int> CurrentVertices;
    [DeallocateOnJobCompletionAttribute] public NativeArray<float3> VertArray;

    public float GridSize;

    private int cubeIndex;
    private int triangleCounter;

    public void Execute()
    {
        triangleCounter = 0;

        for (int x = 0; x < WorldManager.ChunkXSize - 1; x++)
        for (int y = 0; y < WorldManager.ChunkYSize - 1; y++)
        for (int z = 0; z < WorldManager.ChunkZSize - 1; z++)
        {
            Polygonise(x, y, z);
        }
    }

    private void Polygonise(int x, int y, int z)
    {
        cubeIndex = 0;

        CurrentVertices[0] = Voxels[WorldManager.GetIndex(x, y, z)];
        CurrentVertices[1] = Voxels[WorldManager.GetIndex(x + 1, y, z)];
        CurrentVertices[2] = Voxels[WorldManager.GetIndex(x + 1, y + 1, z)];
        CurrentVertices[3] = Voxels[WorldManager.GetIndex(x, y + 1, z)];
        CurrentVertices[4] = Voxels[WorldManager.GetIndex(x, y, z + 1)];
        CurrentVertices[5] = Voxels[WorldManager.GetIndex(x + 1, y, z + 1)];
        CurrentVertices[6] = Voxels[WorldManager.GetIndex(x + 1, y + 1, z + 1)];
        CurrentVertices[7] = Voxels[WorldManager.GetIndex(x, y + 1, z + 1)];

        cubeIndex += CurrentVertices[0];
        cubeIndex += CurrentVertices[1] << 1;
        cubeIndex += CurrentVertices[2] << 2;
        cubeIndex += CurrentVertices[3] << 3;
        cubeIndex += CurrentVertices[4] << 4;
        cubeIndex += CurrentVertices[5] << 5;
        cubeIndex += CurrentVertices[6] << 6;
        cubeIndex += CurrentVertices[7] << 7;

        if (EdgeTable[cubeIndex] == 0)
            return;

        if ((EdgeTable[cubeIndex] & 1) != 0)
            VertArray[0] = PointBetween(GetCubeVertex(x, y, z, 0), GetCubeVertex(x, y, z, 1));
        if ((EdgeTable[cubeIndex] & 2) != 0)
            VertArray[1] = PointBetween(GetCubeVertex(x, y, z, 1), GetCubeVertex(x, y, z, 2));
        if ((EdgeTable[cubeIndex] & 4) != 0)
            VertArray[2] = PointBetween(GetCubeVertex(x, y, z, 2), GetCubeVertex(x, y, z, 3));
        if ((EdgeTable[cubeIndex] & 8) != 0)
            VertArray[3] = PointBetween(GetCubeVertex(x, y, z, 3), GetCubeVertex(x, y, z, 0));
        if ((EdgeTable[cubeIndex] & 16) != 0)
            VertArray[4] = PointBetween(GetCubeVertex(x, y, z, 4), GetCubeVertex(x, y, z, 5));
        if ((EdgeTable[cubeIndex] & 32) != 0)
            VertArray[5] = PointBetween(GetCubeVertex(x, y, z, 5), GetCubeVertex(x, y, z, 6));
        if ((EdgeTable[cubeIndex] & 64) != 0)
            VertArray[6] = PointBetween(GetCubeVertex(x, y, z, 6), GetCubeVertex(x, y, z, 7));
        if ((EdgeTable[cubeIndex] & 128) != 0)
            VertArray[7] = PointBetween(GetCubeVertex(x, y, z, 7), GetCubeVertex(x, y, z, 4));
        if ((EdgeTable[cubeIndex] & 256) != 0)
            VertArray[8] = PointBetween(GetCubeVertex(x, y, z, 0), GetCubeVertex(x, y, z, 4));
        if ((EdgeTable[cubeIndex] & 512) != 0)
            VertArray[9] = PointBetween(GetCubeVertex(x, y, z, 1), GetCubeVertex(x, y, z, 5));
        if ((EdgeTable[cubeIndex] & 1024) != 0)
            VertArray[10] = PointBetween(GetCubeVertex(x, y, z, 2), GetCubeVertex(x, y, z, 6));
        if ((EdgeTable[cubeIndex] & 2048) != 0)
            VertArray[11] = PointBetween(GetCubeVertex(x, y, z, 3), GetCubeVertex(x, y, z, 7));


        for (var i = 0; TriTable[cubeIndex * 16 + i] != -1; i += 3)
        {
            Vertices.Add(VertArray[TriTable[cubeIndex * 16 + i]]);
            Vertices.Add(VertArray[TriTable[cubeIndex * 16 + i + 1]]);
            Vertices.Add(VertArray[TriTable[cubeIndex * 16 + i + 2]]);

            Triangles.Add(triangleCounter + 2);
            Triangles.Add(triangleCounter + 1);
            Triangles.Add(triangleCounter);
            triangleCounter += 3;
        }
    }

    private float3 PointBetween(float3 p1, float3 p2)
    {
        return new float3
        {
            x = p1.x + 0.5f * (p2.x - p1.x),
            y = p1.y + 0.5f * (p2.y - p1.y),
            z = p1.z + 0.5f * (p2.z - p1.z)
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private float3 GetCubeVertex(int x, int y, int z, int vertexIndex)
    {
        return new float3(x * GridSize, y * GridSize, z * GridSize) + VerticesShiftTable[vertexIndex];
    }
}