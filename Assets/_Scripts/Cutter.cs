using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ClipperLib;
using UnityEditor;
using Path = System.Collections.Generic.List<ClipperLib.IntPoint>;
using Paths = System.Collections.Generic.List<System.Collections.Generic.List<ClipperLib.IntPoint>>;
using Random = System.Random;

public class Cutter : MonoBehaviour
{
    public float CutResolution = 1000f;
    public float CutRadius = 0.5f;
    
    public void Cut()
    {
        var colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), CutRadius);
        
        foreach (var hitCollider in colliders)
        {
            if (hitCollider.gameObject == gameObject) continue;
            var cuttable = hitCollider.GetComponent<Cuttable>();
            if(cuttable == null) continue;

            var relativePos = transform.position - hitCollider.transform.position;
            var polygonCollider = cuttable.polygonCollider;

            var subjects = new Paths
            {
                polygonCollider.points.Select(point =>
                {
                    Vector2 rotatedPoint = polygonCollider.transform.TransformDirection(point);
                    return new IntPoint(rotatedPoint.x * CutResolution, rotatedPoint.y * CutResolution);
                }).ToList()
            };

            var clip = new Paths {new Path(20)};
            for (var i = 0; i < 20; i++)
            {
                var theta = i * 2 * Mathf.PI / 20f;
                clip[0].Add(new IntPoint((relativePos.x + (Math.Cos(theta) * CutRadius)) * CutResolution, (relativePos.y + (Math.Sin(theta) * CutRadius)) * CutResolution));
            }
            
            var solution = new Paths();
            var c = new Clipper();
            c.AddPaths(subjects, PolyType.ptSubject, true);
            c.AddPaths(clip, PolyType.ptClip, true);
            c.Execute(ClipType.ctDifference, solution, PolyFillType.pftEvenOdd, PolyFillType.pftEvenOdd);
    
            if (solution.Count > 0)
            {
                for (var i = 0; i < solution.Count; i++)
                {
                    if (i > 0)
                    {
                        var clone = Instantiate(cuttable, polygonCollider.transform.parent);
                        foreach (Transform child in clone.transform)
                        {
                            Destroy(child.gameObject);
                        }
                    }

                    cuttable.SetVertices(solution[i].Select(
                        point => (Vector2) polygonCollider.transform.InverseTransformDirection(new Vector2(point.X / CutResolution, point.Y / CutResolution))
                    ).ToArray());
                }
            }
            else
            {
                Destroy(polygonCollider.gameObject);
            }
        }
    }
}
