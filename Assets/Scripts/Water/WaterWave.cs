using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshCollider))]
public class WaterWave : MonoBehaviour
{
	[System.Serializable]
	struct Wave
	{
		[Range(0, 10)] public float amplitude;
		[Range(0, 10)] public float roll;
		[Range(0, 10)] public float rate;

		[Range(0, 100)] public float length;
		[Range(0, 360)] public float direction;
	}

	[SerializeField] Wave[] waves;

	[Header("Mesh Generator")]
	[SerializeField][Range(1, 500)] float xSize = 40;
	[SerializeField][Range(1, 500)] float zSize = 40;
	[SerializeField][Range(2, 500)] int xVertexNum = 40;
	[SerializeField][Range(2, 500)] int zVertexNum = 40;

	MeshFilter meshFilter;
	MeshCollider meshCollider;

	Mesh mesh;
	Vector3[] vertices;
	Vector3[,] buffer;

	void Start()
	{
		meshFilter = GetComponent<MeshFilter>();
		meshCollider = GetComponent<MeshCollider>();

		MeshGenerator.Plane(meshFilter, xSize, zSize, xVertexNum, zVertexNum);

		mesh = meshFilter.mesh;
		vertices = mesh.vertices;

		buffer = new Vector3[xVertexNum, zVertexNum];
	}

    Vector3 GerstnerWave(Vector3 position, Vector2 direction, float speed, float length, float amplitude, float roll)
    {
        Vector3 v = Vector3.zero;

        float coord = position.x * direction.x + position.z * direction.y;
        float k = 2 * Mathf.PI / length;
        float f = k * coord + speed;

        v.x = Mathf.Cos(f) * roll;
        v.y = Mathf.Sin(f) * amplitude;

        return v;
    }

    void Update()
	{
		// update vertex values with wave
		for (int z = 0; z < zVertexNum; z++)
		{
			float zPosition = ((float)z / (zVertexNum - 1) - 0.5f) * xSize;
			for (int x = 0; x < xVertexNum; x++)
			{
				Vector3 p = Vector3.zero;
				p.x = ((x / (float)xVertexNum - 1) - 0.5f) * xSize;
				p.z = ((z / (float)zVertexNum - 1) - 0.5f) * zSize;

				//Vector3 o = GerstnerWave(p, wave[0].direction, wave[0].rate, wave[0].length, wave[0].amplitude, wave[0].roll);
				Vector3 o = Vector3.zero;

				for (int y = 0; y < waves.Length; y++)
				{
                    Vector2 d = new Vector2(Mathf.Cos(Mathf.Deg2Rad * waves[y].direction), Mathf.Sin(Mathf.Deg2Rad * waves[y].direction));
                    d.Normalize();

                    o += GerstnerWave(p, d, Time.time * waves[y].rate, waves[y].length, waves[y].amplitude, waves[y].roll);
                }

				buffer[x, z] = p + o;
			}
		}

		// set vertices from buffer
		for (int x = 0; x < xVertexNum; x++)
		{
			for (int z = 0; z < zVertexNum; z++)
			{
				vertices[x + z * xVertexNum] = buffer[x, z];
			}
		}

		// recalculate mesh with new vertice values
		mesh.vertices = vertices;
		mesh.RecalculateNormals();
		mesh.RecalculateTangents();
		mesh.RecalculateBounds();
		meshCollider.sharedMesh = mesh;
	}
}
