using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

namespace Donuts
{
    [CustomEditor(typeof(SplineMeshComponent))]
    public class SplineMeshComponentEditor : Editor
    {
        private SplineMeshComponent splineMeshComponent;
        private static bool isEditMode = false;
        private bool isRealTimeMode = false;
        private SplineMeshComponent copyTarget;

        private const string resourcesPath = "Assets/Resources/";
        private const string path = "Assets/Resources/GeneratedMesh/";
        public void OnEnable()
        {

            splineMeshComponent = target as SplineMeshComponent;
            if (splineMeshComponent.meshFilter == null)
            {
                splineMeshComponent.meshFilter = splineMeshComponent.gameObject.AddComponent<MeshFilter>();
            }
            else
            {
                splineMeshComponent.meshFilter = splineMeshComponent.gameObject.GetComponent<MeshFilter>();
            }
            if (splineMeshComponent.meshRenderer == null)
            {
                splineMeshComponent.meshRenderer = splineMeshComponent.gameObject.AddComponent<MeshRenderer>();
            }
            else
            {
                splineMeshComponent.meshRenderer = splineMeshComponent.gameObject.GetComponent<MeshRenderer>();
            }
            if (splineMeshComponent.meshRenderer.sharedMaterial == null)
            {
                GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Plane);
                Material diffuse = primitive.GetComponent<MeshRenderer>().sharedMaterial;
                DestroyImmediate(primitive);
                splineMeshComponent.meshRenderer.sharedMaterial = diffuse;
            }
        }


        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            CheckDataSafe();
            EditModeButton();
            SaveMeshButton();
            DrowCopyFromOther();
        }

        private void CheckDataSafe()
        {
            splineMeshComponent.splineDensity = Mathf.Max(0.01f, splineMeshComponent.splineDensity);
            int count =splineMeshComponent.nodes.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                if(splineMeshComponent.nodes[i].controlPoint == null || splineMeshComponent.nodes[i].nodeTransform == null)
                {
                    splineMeshComponent.nodes.RemoveAt(i);
                }
            }
        }

        private void EditModeButton()
        {
            if (isEditMode == false)
            {
                if (GUILayout.Button("Edit mode"))
                {
                    SceneView.duringSceneGui -= UpdateEditeModeSceneView;
                    SceneView.duringSceneGui += UpdateEditeModeSceneView;
                    isEditMode = true;
                }
                return;
            }
            bool selected = GUILayout.Toggle(true, "Out Edit Mode", "Button");
            if (selected == false)
            {
                SceneView.duringSceneGui -= UpdateEditeModeSceneView;
                isEditMode = false;
            }
        }

        private void SaveMeshButton()
        {
            if (splineMeshComponent.meshFilter == null)
            {
                return;
            }
            EditorGUI.BeginDisabledGroup(splineMeshComponent.meshFilter.sharedMesh == null || splineMeshComponent.meshFilter.sharedMesh.vertices.Length <= 0);
            if (GUILayout.Button("Save Mesh"))
            {
                if (Directory.Exists(resourcesPath) == false)
                {
                    Directory.CreateDirectory(resourcesPath);
                }
                if (Directory.Exists(path) == false)
                {
                    Directory.CreateDirectory(path);
                }
                if (File.Exists(path + splineMeshComponent.name + ".asset"))
                {
                    AssetDatabase.SaveAssets();
                }
                else
                {
                    //Debug.Log(path + splineMeshComponent.name + ".asset");
                    AssetDatabase.CreateAsset(splineMeshComponent.meshFilter.sharedMesh, path + splineMeshComponent.name + ".asset");
                    AssetDatabase.SaveAssets();
                }
            }
            EditorGUI.EndDisabledGroup();
        }

        private void DrowCopyFromOther()
        {
            copyTarget = (SplineMeshComponent)EditorGUILayout.ObjectField("Copy target", copyTarget, typeof(SplineMeshComponent), true);
            EditorGUI.BeginDisabledGroup(copyTarget == null);
            if (GUILayout.Button("Copy fromOther"))
            {
                CopyFromTarget();
            }
            EditorGUI.EndDisabledGroup();
        }

        private void UpdateEditeModeSceneView(SceneView sceneView)
        {
            
            UpdateSceneViewUI();
            UpdateKeyboardInput();

            CheckDataSafe();

            CheckGenerateMesh();
            DrawHandles();
            //Draw handle
        }

        private void UpdateSceneViewUI()
        {
            if (isEditMode == false || splineMeshComponent == null)
            {
                SceneView.duringSceneGui -= UpdateEditeModeSceneView;
                return;
            }
            Handles.BeginGUI();
            bool selected = GUILayout.Toggle(true, "Out Edit Mode", "Button", GUILayout.Width(100));
            if (selected == false)
            {
                SceneView.duringSceneGui -= UpdateEditeModeSceneView;
                isEditMode = false;
            }

            isRealTimeMode = GUILayout.Toggle(isRealTimeMode, "Real Time Mode", "Button", GUILayout.Width(100));
            //auto refresh button
            Handles.EndGUI();
        }

        private void UpdateKeyboardInput()
        {
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.G)
            {
                CreateNewNode();
            }
        }

        private void CheckGenerateMesh()
        {
            if (isEditMode == false)
            {
                return;
            }
            if (Event.current.type == EventType.MouseDown || (isRealTimeMode || Event.current.type == EventType.MouseDrag))
            {

                bool isChild = CheckIsSelectedChild();

                if (isChild)
                {
                    RecalculateDistance();
                    ReGenerateMesh();
                }
            }
        }
        private bool CheckIsSelectedChild()
        {
            GameObject selected = Selection.activeObject as GameObject;
            if (selected != null)
            {
                Transform parent = selected.transform.parent;
                while (parent != null)
                {
                    if (parent == splineMeshComponent.transform)
                    {
                        return true;
                    }
                    parent = parent.parent;
                }
            }
            return false;
        }

        private void CreateNewNode()
        {
            int length = splineMeshComponent.nodes.Count;
            SplineNode node = new SplineNode();

            GameObject child1 = new GameObject("Node" + length);
            child1.transform.SetParent(splineMeshComponent.transform);
            GameObject control = new GameObject("NodeControl " + length);
            control.transform.SetParent(child1.transform);

            node.nodeTransform = child1.transform;
            node.controlPoint = control.transform;
            if (length <= 0)
            {
                child1.transform.position = splineMeshComponent.transform.position;
                control.transform.position = splineMeshComponent.transform.position;
            }
            else
            {
                child1.transform.position = splineMeshComponent.nodes[length - 1].nodeTransform.position;

                child1.transform.rotation = splineMeshComponent.nodes[length - 1].nodeTransform.rotation;

                control.transform.position = splineMeshComponent.nodes[length - 1].nodeTransform.position + child1.transform.forward * splineMeshComponent.splineDensity;
                control.transform.rotation = splineMeshComponent.nodes[length - 1].nodeTransform.rotation;
            }

            splineMeshComponent.nodes.Add(node);
            Selection.activeObject = node.nodeTransform.gameObject;
        }

        private void RecalculateDistance()
        {
            int count = splineMeshComponent.nodes.Count;
            float distance = 0;
            for (int i = 0; i < count; i++)
            {
                int next = (i + 1) % count;
                splineMeshComponent.nodes[i].distanceToNextNode = (splineMeshComponent.nodes[i].nodeTransform.position - splineMeshComponent.nodes[i].controlPoint.position).magnitude
                    + (splineMeshComponent.nodes[next].nodeTransform.position - splineMeshComponent.nodes[i].controlPoint.position).magnitude;
                if (splineMeshComponent.isLoop == false && i == count - 1)
                {
                    splineMeshComponent.nodes[i].distanceToNextNode = 0;
                }
                distance += splineMeshComponent.nodes[i].distanceToNextNode;
            }
            splineMeshComponent.totalDistance = distance;
        }

        private void ReGenerateMesh()
        {
            if (splineMeshComponent.meshFilter.sharedMesh == null)
            {
                splineMeshComponent.meshFilter.sharedMesh = new Mesh();
            }
            SplineMeshGenerator.GenerateMeshData(splineMeshComponent, splineMeshComponent.meshFilter.mesh);
        }

        private void DrawHandles()
        {
            int count = splineMeshComponent.nodes.Count;
            Vector3 size = Vector3.one * splineMeshComponent.nodeSize;
            Vector3 up = Vector3.up;
            Color yellow = Color.yellow;
            Color green = Color.green;

            for (int i = 0; i < count; i++)
            {
                Vector3 pos = splineMeshComponent.nodes[i].nodeTransform.position;
                Vector3 controlPos = splineMeshComponent.nodes[i].controlPoint.position;

                Handles.color = green;
                Handles.DrawWireCube(pos, size);
                Handles.DrawLine(pos, controlPos);
                Handles.color = yellow;
                Handles.DrawWireCube(controlPos, size);
            }

        }

        private void CopyFromTarget()
        {
            List<SplineNode> nodes = new List<SplineNode>();
            int count = copyTarget.nodes.Count;
            for (int i = 0; i < count; i++)
            {
                Transform cloned = GameObject.Instantiate<Transform>(copyTarget.nodes[i].nodeTransform);
                SplineNode n = new SplineNode();
                cloned.SetParent(splineMeshComponent.transform);
                n.nodeTransform = cloned;
                n.controlPoint = cloned.GetChild(0);
                n.distanceToNextNode = copyTarget.nodes[i].distanceToNextNode;
                nodes.Add(n);
            }
            splineMeshComponent.nodes.AddRange(nodes);
            EditorUtility.SetDirty(splineMeshComponent);
            serializedObject.ApplyModifiedProperties();
        }
    }
}