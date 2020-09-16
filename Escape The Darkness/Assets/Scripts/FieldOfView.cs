using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask = default;

    private Mesh mesh;   
    private Vector3 origin;
    private float startingAngle;
    public float fov;
    public float viewDistance;

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        fov = 40f;
        viewDistance = 15f;
        origin = Vector3.zero;
    }

    private void LateUpdate()
    {
        int rayCount = 50;
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;

        Vector3[] verticies = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[verticies.Length];
        int[] triangles = new int[rayCount * 3];

        verticies[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;

        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, layerMask);

            if (raycastHit2D.collider == null)
            {
                //No Hit
                vertex = origin + GetVectorFromAngle(angle) * viewDistance;
            }
            else
            {
                //Hit Object
                vertex = raycastHit2D.point;
            }

            verticies[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }

        mesh.vertices = verticies;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(origin, Vector3.one * 1000f);
    }

    public void SetOrigin(Vector3 _origin)
    {
        this.origin = _origin;
    }

    public void SetAimDirection(Vector3 _aimDirection)
    {
        startingAngle = GetAngleFromVectorFloat(_aimDirection) + fov / 2f;
    }

    public void SetFoV(float _fov)
    {
        this.fov = _fov;
    }

    public void SetViewDistance(float _viewDistance)
    {
        this.viewDistance = _viewDistance;
    }

    public static Vector3 GetVectorFromAngle(float _angle)
    {
        //angle = 0 => 360
        float angleRad = _angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public static float GetAngleFromVectorFloat(Vector3 _dir)
    {
        _dir = _dir.normalized;
        float n = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;

        if (n < 0)
        {
            n += 360;
        }

        return n;
    }
}
