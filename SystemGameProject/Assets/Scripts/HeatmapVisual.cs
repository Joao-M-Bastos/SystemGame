using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatmapVisual : MonoBehaviour
{
    private Grid _grid;
    private Mesh _mesh;
    private static Quaternion[] cachedQuaternionEulerArr;

    private void Awake()
    {
        _mesh = new Mesh();
        
        GetComponent<MeshFilter>().mesh = _mesh;
    }

    public void SetGrid(Grid grid)
    {
        _grid = grid;
        UpdateHeatMapVisual();
    }

    public void UpdateHeatMapVisual()
    {
        int quadCount = _grid.Width * _grid.Hight;
        Vector3[] vertices = new Vector3[4 * quadCount];
        Vector2[] uvs = new Vector2[4 * quadCount];
        int[] triangles = new int[6 * quadCount];

        for (int i = 0; i < _grid.Width; i++)
        {
            for (int j = 0; j < _grid.Hight; j++)
            {
                int index = i * _grid.Hight + j;
                Vector3 quadSize = new Vector3(1, 1) * _grid.CellSize;


                AddQuad(vertices, uvs, triangles, index, _grid.GetWorldPosition(i,j) * (1/ (float)_grid.CellSize) + new Vector3(0.5f,0.5f,0.5f), quadSize,Vector2.zero);
               
            }
        }

        _mesh.vertices = vertices;
        _mesh.uv = uvs;
        _mesh.triangles = triangles;
    }
    private void AddQuad(Vector3[] vertices, Vector2[] uvs, int[] triangles, int index, Vector3 GridPos, Vector3 QuadSize, Vector2 Uv)
    {
        vertices[index * 4] = new Vector3((-0.5f + GridPos.x) * QuadSize.x, (-0.5f + GridPos.y) * QuadSize.y);
        vertices[(index * 4) + 1] = new Vector3((-0.5f + GridPos.x) * QuadSize.x, (+0.5f + GridPos.y) * QuadSize.y);
        vertices[(index * 4) + 2] = new Vector3((+0.5f + GridPos.x) * QuadSize.x, (+0.5f + GridPos.y) * QuadSize.y);
        vertices[(index * 4) + 3] = new Vector3((+0.5f + GridPos.x) * QuadSize.x, (-0.5f + GridPos.y) * QuadSize.y);

        uvs[(index * 4)] = Uv;
        uvs[(index * 4) + 1] = Uv;
        uvs[(index * 4) + 2] = Uv;
        uvs[(index * 4) + 3] = Uv;

        triangles[(index * 6) + 0] = (index * 4) + 0;
        triangles[(index * 6) + 1] = (index * 4) + 1;
        triangles[(index * 6) + 2] = (index * 4) + 2;
        triangles[(index * 6) + 3] = (index * 4) + 2;
        triangles[(index * 6) + 4] = (index * 4) + 3;
        triangles[(index * 6) + 5] = (index * 4) + 0;
    }
}
