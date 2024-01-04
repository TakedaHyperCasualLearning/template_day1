using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Donuts
{

    public struct SplineLoopSet
    {
        public Vector3[] positions;//hot code shapes
        public Vector3[] normals;
        public Vector2[] uvs;
    }

    public static class SplineMeshGenerator
    {
        public static Mesh GenerateMeshData(SplineMeshComponent splineMeshComponent, Mesh mesh)
        {
            SplineLoopSet loopSet = CalculateLoopSet(splineMeshComponent);
            SplineLoopSet[] loopSets = CalculateAllLoopSet(splineMeshComponent, loopSet);
            ExportLoopsetsToMesh(loopSets, mesh);
            return mesh;
        }

        private static SplineLoopSet CalculateLoopSet(SplineMeshComponent splineMeshComponent)
        {
            SplineLoopSet splineLoopSet = new SplineLoopSet();

            Keyframe[] keyframes = splineMeshComponent.shape.keys;

            int length = keyframes.Length;
            Vector3[] loopHeights = new Vector3[length];
            Vector3[] normals = new Vector3[length];
            Vector2[] uvs = new Vector2[length];
            Vector3 upVector = Vector2.up;
            
            for (int i = 0; i < length; i++)
            {
                loopHeights[i] = new Vector3();
                loopHeights[i].x = (keyframes[i].time - 0.5f) * splineMeshComponent.splineWidth;
                loopHeights[i].y = keyframes[i].value * splineMeshComponent.splineHeight;

                uvs[i] = new Vector2(keyframes[i].time - 0.5f, 0.5f);
                normals[i] = upVector;
                
            }
            splineLoopSet.positions = loopHeights;
            splineLoopSet.uvs = uvs;
            splineLoopSet.normals = normals;
            return splineLoopSet;
        }

        public static SplineLoopSet[] CalculateAllLoopSet(SplineMeshComponent splineMeshComponent, SplineLoopSet loopSet)
        {
            List<SplineLoopSet> loopSets = new List<SplineLoopSet>();
            float t = 0;
            NodePoint point;
            while (t < splineMeshComponent.totalDistance)
            {
                point = splineMeshComponent.GetPoint(t);
                SplineLoopSet newSet = CalculateLoopSetPosition(loopSet, point.position, point.rotation);
                loopSets.Add(newSet);
                t += splineMeshComponent.splineDensity;
            }
            point = splineMeshComponent.GetPoint(splineMeshComponent.totalDistance);
            SplineLoopSet lastSet = CalculateLoopSetPosition(loopSet, point.position, point.rotation);
            loopSets.Add(lastSet);
            return loopSets.ToArray();
        }

        public static SplineLoopSet CalculateLoopSetPosition(SplineLoopSet model, Vector3 offset, Quaternion rotation)
        {
            SplineLoopSet newObject = new SplineLoopSet();

            int length = model.positions.Length;
            Vector3[] modelPos = model.positions;

            Vector3[] newPos = new Vector3[length];
            Vector3[] newNormals = new Vector3[length];
            Vector3 pos;
            Vector3 upVector = rotation * Vector3.up;
            for (int i = 0; i < length; i++)
            {
                pos = modelPos[i];
                pos = rotation * pos + offset;
                newPos[i] = pos;
                newNormals[i] = upVector;
            }
            newObject.positions = newPos;
            newObject.normals = newNormals;
            newObject.uvs = model.uvs;
            return newObject;
        }

        private static void ExportLoopsetsToMesh(SplineLoopSet[] splineLoopSets, Mesh mesh)
        {
            List<Vector3> positions = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<int> triangles = new List<int>();
            int length = splineLoopSets.Length;
            if(length <= 0)
            {
                return;
            }
            
            positions.AddRange(splineLoopSets[0].positions);
            normals.AddRange(splineLoopSets[0].normals);
            uvs.AddRange(splineLoopSets[0].uvs);
            int nodeNumbers = splineLoopSets[0].positions.Length;
            
            for (int i = 1; i < length; i++)
            {
                positions.AddRange(splineLoopSets[i].positions);
                normals.AddRange(splineLoopSets[i].normals);
                uvs.AddRange(splineLoopSets[i].uvs);
                //connect triangles
                for (int j = 0; j < nodeNumbers - 1; j++)
                {
                    triangles.Add((i - 1) * nodeNumbers + j);
                    triangles.Add(i * nodeNumbers + j);
                    triangles.Add((i - 1) * nodeNumbers + 1 + j);

                    triangles.Add(i * nodeNumbers + j);
                    triangles.Add(i * nodeNumbers + j +  1);
                    triangles.Add((i - 1) * nodeNumbers + 1 + j);
                }
            }
            mesh.Clear();
            mesh.vertices = positions.ToArray();
            mesh.normals = normals.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
        }

    }
}