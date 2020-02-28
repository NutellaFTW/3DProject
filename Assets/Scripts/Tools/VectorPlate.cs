using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorPlate : MonoBehaviour {

    public ToolManager toolManager;

    public Transform spotLight;
    public Light spotLightColor;
    public Color spotLightColorToBe;
    public float spotLightHeight = 5f;

    public Movement player;

    public GameObject spotLightPrefab;

    public List<GameObject> jumpPads = new List<GameObject>();
    
    private MeshFilter meshFilter;
    private Material material;

    public void Start() {
        meshFilter = GetComponent<MeshFilter>();
        material = GetComponent<Renderer>().sharedMaterial;
    }

    // Get location player clicks
    public void Update() {

        if (toolManager.sceneLimits[1] == 0)
            return;
        
        if (toolManager.currentTool != "Vector Plate") {
            if (spotLight.gameObject.activeSelf)
                spotLight.gameObject.SetActive(false);
            return;
        }

        if (!spotLight.gameObject.activeSelf)
            spotLight.gameObject.SetActive(true);

        if (Input.GetKeyDown(KeyCode.R) && gameObject.name == "PlaneController")
            spotLight.Rotate(0, 0, -90);

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

            if (containsLight(worldPosition))
                return;
            
            hoverSquare(worldPosition);

            if (Input.GetMouseButtonDown(0)) {
                placePad(worldPosition);
                toolManager.useTool(1);
            }

        } 
        
    }

    private void placePad(Vector3 position) {
        
        Vector3 lightPosition = position;
        lightPosition.y = position.y + spotLightHeight;

        jumpPads.Add(Instantiate(spotLightPrefab, lightPosition, spotLight.rotation));
        
    }

    private bool containsLight(Vector3 position) {
        
        Vector3 lightPosition = position;
        lightPosition.y = position.y + spotLightHeight;
        
        foreach (GameObject pad in jumpPads)
            if (pad.transform.position == lightPosition)
                return true;
        
        return false;
    }

    private void hoverSquare(Vector3 position) {

        Vector3 lightPosition = position;
        lightPosition.y = position.y + spotLightHeight;

        spotLightColor.color = spotLightColorToBe;
        spotLight.position = lightPosition;

    }

    private int findVertex(Vector3 vector) {
        Vector3[] vertices = meshFilter.mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
            if (vertices[i] == vector)
                return i;
        return -1;
    }

}
