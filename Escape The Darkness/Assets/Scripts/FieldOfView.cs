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







//    public float viewRadius; 

//    [Range(0, 360)]
//    public float viewAngle;

//    public LayerMask targetMask;
//    public LayerMask obstacleMask;

//    public bool facingRight = true;

//    public List<Transform> visibleTargets = new List<Transform>();

//    public float meshResolution;


//    public int edgeResolveIterations;
//    public float edgeDstTreshold;


//    public MeshFilter viewMeshFilter;
//    Mesh viewMesh;

//    private void Start()
//    {
//        viewMesh = new Mesh();
//        viewMesh.name = "View Mesh";
//        viewMeshFilter.mesh = viewMesh;

//        StartCoroutine(FindTargetsWithDelay(.2f));
//    }

//    IEnumerator FindTargetsWithDelay(float delay)
//    {
//        while (true)
//        {
//            yield return new WaitForSeconds(delay);
//            FindVisibleTargets();
//        }
//    }

//    void FindVisibleTargets()
//    {
//        visibleTargets.Clear();

//        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);

//        for (int i = 0; i < targetsInViewRadius.Length; i++)
//        {
//            Transform target = targetsInViewRadius[i].transform;

//            Vector2 dirToTarget = (target.position - transform.position).normalized;

//            if (facingRight)
//            {
//                if (Vector2.Angle(transform.right, dirToTarget) < viewAngle / 2)
//                {
//                    float dstToTarget = Vector3.Distance(transform.position, target.position);

//                    if (!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
//                    {
//                        Debug.Log("Find");
//                        visibleTargets.Add(target);
//                    }
//                }
//            }

//            if (!facingRight)
//            {
//                if (Vector2.Angle(transform.right, -dirToTarget) < viewAngle / 2)
//                {
//                    float dstToTarget = Vector3.Distance(transform.position, target.position);

//                    if (!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
//                    {
//                        Debug.Log("Find");
//                        visibleTargets.Add(target);
//                    }
//                }
//            }
//        }
//    }

//    //public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
//    //{
//    //    if (!angleIsGlobal)
//    //    {
//    //        angleInDegrees += transform.eulerAngles.y;
//    //    }
//    //    return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);
//    //}

//    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
//    {
//        if (!angleIsGlobal) { angleInDegrees += transform.eulerAngles.z; }
//        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
//    }

//    private void Update()
//    {
//        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
//        if (directionalInput.x > 0 && !facingRight)
//        {
//            Flip();
//        }
//        else if (directionalInput.x < 0 && facingRight)
//        {
//            Flip();
//        }

//        DrawFieldOfView();
//    }

//    void Flip()
//    {
//        facingRight = !facingRight;
//    }

//    void DrawFieldOfView()
//    {
//        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
//        float stepAngleSize = viewAngle / stepCount;

//        List<Vector3> viewPoints = new List<Vector3>();

//        ViewCastInfo oldViewCast = new ViewCastInfo();

//        for (int i = 0; i <= stepCount; i++)
//        {
//            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
//            //Debug.DrawLine(transform.position, transform.position + DirFromAngle(angle, true) * viewRadius, Color.red);
//            ViewCastInfo newViewCast = ViewCast(angle);

//            if (i > 0)
//            {
//                bool edgeDstTresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstTreshold;
//                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstTresholdExceeded))
//                {
//                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);

//                    if (edge.PointA != Vector3.zero)
//                    {
//                        viewPoints.Add(edge.PointA);
//                    }
//                    if (edge.PointB != Vector3.zero)
//                    {
//                        viewPoints.Add(edge.PointB);
//                    }
//                }
//            }

//            viewPoints.Add(newViewCast.point);
//            oldViewCast = newViewCast;
//        }

//        int vertextCount = viewPoints.Count + 1;
//        Vector3[] vertices = new Vector3[vertextCount];
//        int[] triangles = new int[(vertextCount - 2) * 3];

//        vertices [0] = Vector3.zero;
//        for (int i = 0; i < vertextCount - 1; i++)
//        {
//            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

//            if (i < vertextCount - 2)
//            {
//                triangles[i * 3] = 0;
//                triangles[i * 3 + 1] = i + 1;
//                triangles[i * 3 + 2] = i + 2;
//            }
//        }

//        viewMesh.Clear();
//        viewMesh.vertices = vertices;
//        viewMesh.triangles = triangles;
//        viewMesh.RecalculateNormals();
//    }

//    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
//    {
//        float minAngle = minViewCast.angle;
//        float maxAngle = maxViewCast.angle;
//        Vector3 minPoint = Vector3.zero;
//        Vector3 maxPoint = Vector3.zero;


//        for (int i = 0; i < edgeResolveIterations; i++)
//        {
//            float angle = (minAngle + maxAngle) / 2;
//            ViewCastInfo newViewCast = ViewCast(angle);

//            bool edgeDstTresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstTreshold;
//            if (newViewCast.hit == minViewCast.hit && !edgeDstTresholdExceeded)
//            {
//                minAngle = angle;
//                minPoint = newViewCast.point;
//            }
//            else
//            {
//                maxAngle = angle;
//                maxPoint = newViewCast.point;
//            }
//        }
//        return new EdgeInfo(minPoint, maxPoint);
//    }

//    ViewCastInfo ViewCast(float gloabalAngle)
//    {
//        Vector3 dir = DirFromAngle(gloabalAngle, true);

//        //RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(dir.x, dir.y), viewRadius, obstacleMask);
//        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, viewRadius, obstacleMask);
//        if (hit.collider != null)
//        {
//            return new ViewCastInfo(true, hit.point, hit.distance, gloabalAngle);
//        }
//        else
//        {
//            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, gloabalAngle);
//        }

//        //RaycastHit2D hit;

//        //if (Physics2D.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask)
//        //{
//        //    return new ViewCastInfo(true, hit.point, hit.distance, gloabalAngle);
//        //}
//        //else
//        //{
//        //    return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, gloabalAngle);
//        //}
//    }

//    public struct ViewCastInfo
//    {
//        public bool hit;
//        public Vector3 point;
//        public float dst;
//        public float angle;

//        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
//        {
//            hit = _hit;
//            point = _point;
//            dst = _dst;
//            angle = _angle;
//        }
//    }

//    public struct EdgeInfo
//    {
//        public Vector3 PointA;
//        public Vector3 PointB;

//        public EdgeInfo(Vector3 _PointA, Vector3 _PointB)
//        {
//            PointA = _PointA;
//            PointB = _PointB;
//        }
//    }
//}
