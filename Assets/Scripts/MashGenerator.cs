using UnityEngine;
using System.Collections;

public static class MashGenerator
{
    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve, int levelOfDetail)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        int simplificationIncrimentent = levelOfDetail == 0 ? 1 : levelOfDetail * 2;

        int verticesPerLine = ((width - 1) / simplificationIncrimentent) + 1;

        MeshData meshData = new MeshData(verticesPerLine, verticesPerLine);
        int vertexIndex = 0;

        for( int y = 0; y < height; y += simplificationIncrimentent)
        {
            for( int x = 0; x < width; x += simplificationIncrimentent)
            {
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, (heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier), topLeftZ - y);
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                if( x < width - 1 && y < height - 1)
                {
                    meshData.AddTriangule(vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
                    meshData.AddTriangule(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        return meshData;
    }
}

public class MeshData
{
    public Vector3[] vertices;
    public int[] triangules;
    public Vector2[] uvs;

    private int trianguleIndex;

    public MeshData(int meshWidth, int meshHeight)
    {
        vertices = new Vector3[meshWidth * meshHeight];
        uvs = new Vector2[meshWidth * meshHeight];
        triangules = new int[(meshWidth - 1) * (meshHeight  - 1) * 6];
    }

    public void AddTriangule(int a, int b, int c)
    {
        triangules[trianguleIndex] = a;
        triangules[trianguleIndex + 1] = b;
        triangules[trianguleIndex + 2] = c;

        trianguleIndex += 3;
    }

    public Mesh createMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangules;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        return mesh;
    }
}