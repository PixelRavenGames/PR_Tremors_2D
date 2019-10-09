using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ClipperLib;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(PolygonCollider2D))]
public class Cuttable : MonoBehaviour
{
    [HideInInspector] public PolygonCollider2D polygonCollider;

    private Camera cullCamera;
    private Mesh cullMesh;

    private static float lastClippingZ = 0;
    
    private void Start()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
            
        InitializeMask();
        SetVertices(polygonCollider.GetPath(0));
    }

    public void SetVertices(Vector2[] points)
    {
        polygonCollider.SetPath(0, points);
        
        cullMesh.Clear();
        cullMesh.vertices = points.Select(vertex => new Vector3(vertex.x, vertex.y, 0)).ToArray();
        cullMesh.triangles = new Triangulator(points).Triangulate();
    }

    private void InitializeMask()
    {
        lastClippingZ += 0.1f;
        cullMesh = new Mesh();
        
        var sr = GetComponent<SpriteRenderer>();
        if (!sr) return;
        
        var sprite = sr.sprite;

        var width = sprite.texture.width;
        var height = sprite.texture.height;
        if (sr.drawMode == SpriteDrawMode.Tiled)
        {
            width = (int) (width * sr.size.x);
            height = (int) (height * sr.size.y);
        }

        var rt = new RenderTexture(width, height, 16, RenderTextureFormat.ARGB32);
        rt.Create();
            
        var newcamera = new GameObject("Culling Camera");
        newcamera.transform.parent = transform;
        newcamera.transform.localPosition = new Vector3(0, 0, lastClippingZ);
        cullCamera = newcamera.AddComponent<Camera>();
        cullCamera.nearClipPlane = 0.01f;
        cullCamera.farClipPlane = 0.09f;
        cullCamera.orthographic = true;
        cullCamera.orthographicSize = sprite.bounds.size.y / 2;
        cullCamera.targetTexture = rt;
        cullCamera.backgroundColor = Color.black;
        cullCamera.cullingMask = 1 << LayerMask.NameToLayer("Clipping");
            
        var clippingmask = new GameObject("Clipping Mask") {layer = LayerMask.NameToLayer("Clipping")};
        clippingmask.transform.parent = transform;
        clippingmask.transform.localPosition = new Vector3(0, 0, lastClippingZ + 0.05f);
        clippingmask.layer = LayerMask.NameToLayer("Clipping");
        clippingmask.AddComponent<MeshRenderer>();
        clippingmask.AddComponent<MeshFilter>().mesh = cullMesh;
            
        sr.material.SetTexture("_Mask", rt);
    }
}
