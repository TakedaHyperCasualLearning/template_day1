using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Donuts
{
    [System.Serializable]
    public class SplineNode
    {
        public Transform nodeTransform;
        public Transform controlPoint;
        public float distanceToNextNode = 0;//set by editor
    }

    public struct NodePoint
    {
        public Vector3 position;
        public Quaternion rotation;
    }


    public class SplineMeshComponent : MonoBehaviour
    {
        public MeshFilter meshFilter;
        public MeshRenderer meshRenderer;
        public List<SplineNode> nodes = new List<SplineNode>();
        public float splineWidth = 1;
        public float splineHeight = 1;
        public float splineDensity = 0.2f;
        public bool isLoop = false;
        public float totalDistance = 0;


        [Header("shape is from 0 ~ 1 0.5 is the center")]
        public AnimationCurve shape = AnimationCurve.Linear(0, 0, 1, 0);

        [Header("Editor Use")]
        public float nodeSize = 0.5f;


        //Get Point
        private static Quaternion identityRot = Quaternion.identity;
        
        public NodePoint GetPoint(float distance, Vector3 offset = new Vector3())
        {
            int length = nodes.Count;
            float calculated;
            NodePoint tempNodePoint = new NodePoint();
            int index = 0;
            if (isLoop)
            {

                distance = distance % totalDistance;
                calculated = distance - nodes[index].distanceToNextNode;
                while (calculated > 0)
                {
                    distance = calculated;
                    index = (index + 1) % length;
                    calculated = distance - nodes[index].distanceToNextNode;
                }
            }
            else
            {
                if (distance < totalDistance)
                {

                    for (int i = 0; i < length; i++)
                    {
                        index = i;
                        calculated = distance - nodes[index].distanceToNextNode;
                        if (calculated <= 0)
                        {
                            break;
                        }
                        distance = calculated;
                    }
                }
                else
                {
                    index = length - 1;
                }
            }
            int nextIndex = isLoop ? (index + 1) % length : Mathf.Min(length - 1, index + 1);
            float t = distance / nodes[index].distanceToNextNode;
            float invertT = 1 - t;
            float t2 = t * t;
            float invertT2 = invertT * invertT;
            float controlT = 2 * t * invertT;
            Vector3 startPos = nodes[index].nodeTransform.position + offset;
            Vector3 controlPos = nodes[index].controlPoint.position + offset;
            Vector3 targetPos = nodes[nextIndex].nodeTransform.position + offset;
            Quaternion startRot = nodes[index].nodeTransform.rotation;
            Quaternion controlRot = nodes[index].controlPoint.rotation;
            Quaternion targetRot = nodes[nextIndex].nodeTransform.rotation;

            tempNodePoint.position = t2 * targetPos + controlT * controlPos + invertT2 * startPos;
            tempNodePoint.rotation = Quaternion.Lerp(startRot, targetRot, t);
            return tempNodePoint;
        }

    }
}