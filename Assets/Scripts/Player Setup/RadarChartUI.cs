using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarChartUI : MonoBehaviour
{
    PlayerSkills playerSkills;

    private int programmingPoints;
    private int designPoints;
    private int financePoints;
    private int marketingPoints;
    private int dataAnalysisPoints;
    private int humanResourcesPoints;
    private int productManagementPoints;
    private int qualityAssurancePoints;

    [SerializeField] private GameObject programmingBar;
    [SerializeField] private GameObject designBar;
    [SerializeField] private GameObject financeBar;
    [SerializeField] private GameObject marketingBar;
    [SerializeField] private GameObject dataAnalysisBar;
    [SerializeField] private GameObject humanResourcesBar;
    [SerializeField] private GameObject productManagementBar;
    [SerializeField] private GameObject qualityAssuranceBar;

    [SerializeField] private CanvasRenderer radarMeshCanvasRenderer;
    [SerializeField] private Material radarMaterial;
    [SerializeField] private Texture2D radarTexture;

    private void Awake()
    {
        playerSkills = GameObject.Find("Player").GetComponent<PlayerSkills>();

        programmingPoints = playerSkills.skills["Programming"];
        designPoints = playerSkills.skills["Design"];
        financePoints = playerSkills.skills["Finance"];
        marketingPoints = playerSkills.skills["Marketing"];
        dataAnalysisPoints = playerSkills.skills["Data Analysis"];
        humanResourcesPoints = playerSkills.skills["Human Resources"];
        productManagementPoints = playerSkills.skills["Product Management"];
        qualityAssurancePoints = playerSkills.skills["Quality Assurance"];

        /*programmingBar.transform.localScale = new Vector3(1, (float)programmingPoints / playerSkills.maxAttributePoints);
        designBar.transform.localScale = new Vector3(1, (float)designPoints / playerSkills.maxAttributePoints);
        financeBar.transform.localScale = new Vector3(1, (float)financePoints / playerSkills.maxAttributePoints);
        marketingBar.transform.localScale = new Vector3(1, (float)marketingPoints / playerSkills.maxAttributePoints);
        dataAnalysisBar.transform.localScale = new Vector3(1, (float)dataAnalysisPoints / playerSkills.maxAttributePoints);
        humanResourcesBar.transform.localScale = new Vector3(1, (float)humanResourcesPoints / playerSkills.maxAttributePoints);
        productManagementBar.transform.localScale = new Vector3(1, (float)productManagementPoints / playerSkills.maxAttributePoints);
        qualityAssuranceBar.transform.localScale = new Vector3(1, (float)qualityAssurancePoints / playerSkills.maxAttributePoints);*/

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[9];  
        Vector2[] uv = new Vector2[9];
        int[] triangles = new int[3 * 8];

        float radarChartSize = 148f;
        float angleIncrement = 360f / 8;

        Vector3 programmingVertex = Quaternion.Euler(0, 0, -angleIncrement * 0) * Vector3.up * radarChartSize * ((float)programmingPoints / playerSkills.maxAttributePoints);
        int programmingVertexIndex = 1;

        Vector3 designVertex = Quaternion.Euler(0, 0, -angleIncrement * 1) * Vector3.up * radarChartSize * ((float)designPoints / playerSkills.maxAttributePoints);
        int designVertexIndex = 2;

        Vector3 financeVertex = Quaternion.Euler(0, 0, -angleIncrement * 2) * Vector3.up * radarChartSize * ((float)financePoints / playerSkills.maxAttributePoints);
        int financeVertexIndex = 3;

        Vector3 marketingVertex = Quaternion.Euler(0, 0, -angleIncrement * 3) * Vector3.up * radarChartSize * ((float)marketingPoints / playerSkills.maxAttributePoints);
        int marketingVertexIndex = 4;

        Vector3 dataAnalysisVertex = Quaternion.Euler(0, 0, -angleIncrement * 4) * Vector3.up * radarChartSize * ((float)dataAnalysisPoints / playerSkills.maxAttributePoints);
        int dataAnalysisVertexIndex = 5;

        Vector3 humanResourcesVertex = Quaternion.Euler(0, 0, -angleIncrement * 5) * Vector3.up * radarChartSize * ((float)humanResourcesPoints / playerSkills.maxAttributePoints);
        int humanResourcesVertexIndex = 6;

        Vector3 productManagementVertex = Quaternion.Euler(0, 0, -angleIncrement * 6) * Vector3.up * radarChartSize * ((float)productManagementPoints / playerSkills.maxAttributePoints);
        int productManagementVertexIndex = 7;

        Vector3 qualityAssuranceVertex = Quaternion.Euler(0, 0, -angleIncrement * 7) * Vector3.up * radarChartSize * ((float)qualityAssurancePoints / playerSkills.maxAttributePoints);
        int qualityAssuranceVertexIndex = 8;

        vertices[0] = Vector3.zero;
        vertices[programmingVertexIndex] = programmingVertex;
        vertices[designVertexIndex] = designVertex;
        vertices[financeVertexIndex] = financeVertex;
        vertices[marketingVertexIndex] = marketingVertex;
        vertices[dataAnalysisVertexIndex] = dataAnalysisVertex;
        vertices[humanResourcesVertexIndex] = humanResourcesVertex;
        vertices[productManagementVertexIndex] = productManagementVertex;
        vertices[qualityAssuranceVertexIndex] = qualityAssuranceVertex;

        uv[0] = Vector2.zero;
        uv[programmingVertexIndex] = Vector2.one;
        uv[designVertexIndex] = Vector2.one;
        uv[financeVertexIndex] = Vector2.one;
        uv[marketingVertexIndex] = Vector2.one;
        uv[dataAnalysisVertexIndex] = Vector2.one;
        uv[humanResourcesVertexIndex] = Vector2.one;
        uv[productManagementVertexIndex] = Vector2.one;
        uv[qualityAssuranceVertexIndex] = Vector2.one;

        triangles[0] = 0;
        triangles[1] = programmingVertexIndex;
        triangles[2] = designVertexIndex;

        triangles[3] = 0;
        triangles[4] = designVertexIndex;
        triangles[5] = financeVertexIndex;

        triangles[6] = 0;
        triangles[7] = financeVertexIndex;
        triangles[8] = marketingVertexIndex;

        triangles[9] = 0;
        triangles[10] = marketingVertexIndex;
        triangles[11] = dataAnalysisVertexIndex;

        triangles[12] = 0;
        triangles[13] = dataAnalysisVertexIndex;
        triangles[14] = humanResourcesVertexIndex;

        triangles[15] = 0;
        triangles[16] = humanResourcesVertexIndex;
        triangles[17] = productManagementVertexIndex;

        triangles[18] = 0;
        triangles[19] = productManagementVertexIndex;
        triangles[20] = qualityAssuranceVertexIndex;

        triangles[21] = 0;
        triangles[22] = qualityAssuranceVertexIndex;
        triangles[23] = programmingVertexIndex;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        radarMeshCanvasRenderer.SetMesh(mesh);
        radarMeshCanvasRenderer.SetMaterial(radarMaterial, radarTexture);
    }
}
