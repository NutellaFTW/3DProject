using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CutHole : MonoBehaviour {

    public ToolManager toolManager;

    public Transform spotLight;
    public Light spotLightColor;
    public float spotLightHeight = 5f;

    private MeshFilter meshFilter;
    private Material material;

    public VectorPlate vectorPlates;
    public JumpPad jumpPad;

    public void Start() {
        meshFilter = GetComponent<MeshFilter>();
        material = GetComponent<Renderer>().sharedMaterial;
    }
    
    // Get location player clicks
    public void Update() {

        if (toolManager.sceneLimits[0] == 0)
            return;
        
        if (toolManager.currentTool != "Hole Cutter") {
            if (spotLight.gameObject.activeSelf)
                spotLight.gameObject.SetActive(false);
            return;
        }
        
        if (!spotLight.gameObject.activeSelf)
            spotLight.gameObject.SetActive(true);

        RaycastHit raycastHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out raycastHit, 20f)) {

            if (raycastHit.collider.gameObject != gameObject)
                return;
            
            // Getting neighbouring triangle for square

            int hitTriangleIndex = raycastHit.triangleIndex;

            int[] triangles = meshFilter.mesh.triangles;
            Vector3[] vertices = meshFilter.mesh.vertices;

            Vector3[] points = new[] {
                vertices[triangles[hitTriangleIndex * 3 + 0]],
                vertices[triangles[hitTriangleIndex * 3 + 1]],
                vertices[triangles[hitTriangleIndex * 3 + 2]]
            };

            float[] edges = new[] {
                Vector3.Distance(points[0], points[1]),
                Vector3.Distance(points[0], points[2]),
                Vector3.Distance(points[1], points[2])
            };

            Vector3 shared1, shared2;

            if (edges[0] == edges[1]) {
                shared1 = points[1];
                shared2 = points[2];
            }
            else {
                shared1 = points[0];
                shared2 = edges[0] > edges[1] ? points[1] : points[2];
            }

            int vertex1 = findVertex(shared1);
            int vertex2 = findVertex(shared2);

            Vector3 midPoint = new Vector3((shared1.x + shared2.x) / 2, 0, (shared1.z + shared2.z) / 2);
            
            Vector3 worldPosition = transform.TransformPoint(midPoint);

            hoverSquare(worldPosition);

            if (Input.GetMouseButtonDown(0)) {
                deleteSquare(hitTriangleIndex,
                    findTriangle(vertices[vertex1], vertices[vertex2], hitTriangleIndex));
                toolManager.useTool(0);
                deleteJump(worldPosition);
                deleteVectorPlate(worldPosition);
            }

        } 
        
    }

    private void deleteJump(Vector3 position) {

        Vector3 lightPosition = position;
        lightPosition.y = position.y + jumpPad.spotLightHeight;

        foreach (GameObject pad in jumpPad.jumpPads) {
            if (pad.transform.position == lightPosition) {
                Destroy(pad);
                jumpPad.jumpPads.Remove(pad);
            }
        }

    }
    
    private void deleteVectorPlate(Vector3 position) {

        Vector3 lightPosition = position;
        lightPosition.y = position.y + vectorPlates.spotLightHeight;

        foreach (GameObject pad in vectorPlates.jumpPads) {
            if (pad.transform.position == lightPosition) {
                Destroy(pad);
                vectorPlates.jumpPads.Remove(pad);
            }
        }

    }

    private void hoverSquare(Vector3 position) {

        Vector3 lightPosition = position;
        lightPosition.y = position.y + spotLightHeight;

        Color color = material.color;

        spotLightColor.color = DarkenColor(color.r, color.g, color.b, 0.5f);
        spotLight.position = lightPosition;

    }

    private void deleteSquare(int index1, int index2) {
        
        Destroy(gameObject.GetComponent<MeshCollider>());
        
        Mesh mesh = meshFilter.mesh;

        int[] oldTriangles = mesh.triangles;
        int[] newTriangles = new int[oldTriangles.Length - 3];

        int i = 0;
        int x = 0;

        while (i < oldTriangles.Length) {
            if (i != index1 * 3 && i != index2 * 3) {
                newTriangles[x++] = oldTriangles[i++];
                newTriangles[x++] = oldTriangles[i++];
                newTriangles[x++] = oldTriangles[i++];
            } else
                i += 3;
        }

        meshFilter.mesh.triangles = newTriangles;
        gameObject.AddComponent<MeshCollider>();
        
    }

    private int findTriangle(Vector3 vector1, Vector3 vector2, int antiTriangleIndex) {

        int[] triangles = meshFilter.mesh.triangles;
        Vector3[] vertices = meshFilter.mesh.vertices;

        int i = 0;

        while (i < triangles.Length) {
            if (i / 3 != antiTriangleIndex) {
                if (vertices[triangles[i]] == vector1 &&
                    (vertices[triangles[i + 1]] == vector2 || vertices[triangles[i + 2]] == vector2))
                    return i / 3;
                if (vertices[triangles[i]] == vector2 &&
                    (vertices[triangles[i + 1]] == vector1 || vertices[triangles[i + 2]] == vector1))
                    return i / 3;
                if (vertices[triangles[i + 1]] == vector2 &&
                    (vertices[triangles[i]] == vector1 || vertices[triangles[i + 2]] == vector1))
                    return i / 3;
                if (vertices[triangles[i + 1]] == vector1 &&
                    (vertices[triangles[i]] == vector2 || vertices[triangles[i + 2]] == vector2))
                    return i / 3;

            }
            i += 3;
        }

        return -1;

    }

    private int findVertex(Vector3 vector) {
        Vector3[] vertices = meshFilter.mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
            if (vertices[i] == vector)
                return i;
        return -1;
    }
    
    private Color DarkenColor(float r, float g, float b, float factor) {
        return new Color(factor * r, factor * g, factor * b);
    }
    
}
