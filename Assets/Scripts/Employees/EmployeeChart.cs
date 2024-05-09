using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class EmployeeChart : MonoBehaviour
{
    EmployeeSkills employeeSkills;

    private int programmingPoints;
    private int designPoints;
    private int financePoints;
    private int productManagementPoints;
    private int qualityAssurancePoints;

    [SerializeField] private TextMeshProUGUI programmingText;
    [SerializeField] private TextMeshProUGUI designText;
    [SerializeField] private TextMeshProUGUI financeText;
    [SerializeField] private TextMeshProUGUI productManagementText;
    [SerializeField] private TextMeshProUGUI qualityAssuranceText;

    [SerializeField] private GameObject programmingBar;
    [SerializeField] private GameObject designBar;
    [SerializeField] private GameObject financeBar;
    [SerializeField] private GameObject productManagementBar;
    [SerializeField] private GameObject qualityAssuranceBar;

    [SerializeField] private CanvasRenderer radarMeshCanvasRenderer;
    [SerializeField] private Material radarMaterial;
    [SerializeField] private Texture2D radarTexture;

    private void Awake()
    {
        employeeSkills = GetComponentInParent<EmployeeSkills>();

        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - employeeSkills.gameObject.transform.localPosition.y, 0);

        programmingText.text = $"Programming ({employeeSkills.skills["Programming"]})";
        designText.text = $"Design ({employeeSkills.skills["Design"]})";
        financeText.text = $"Finance ({employeeSkills.skills["Finance"]})";
        productManagementText.text = $"Product Management ({employeeSkills.skills["Product Management"]})";
        qualityAssuranceText.text = $"Quality Assurance ({employeeSkills.skills["Quality Assurance"]})";

        programmingPoints = employeeSkills.skills["Programming"];
        designPoints = employeeSkills.skills["Design"];
        financePoints = employeeSkills.skills["Finance"];
        productManagementPoints = employeeSkills.skills["Product Management"];
        qualityAssurancePoints = employeeSkills.skills["Quality Assurance"];
    }

    public void SetupMesh()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[6];
        Vector2[] uv = new Vector2[6];
        int[] triangles = new int[3 * 5];

        float radarChartSize = 145f;
        float angleIncrement = 360f / 5;

        Vector3 programmingVertex = Quaternion.Euler(0, 0, -angleIncrement * 0) * Vector3.up * radarChartSize * ((float)programmingPoints / employeeSkills.maxAttributePoints);
        int programmingVertexIndex = 1;

        Vector3 designVertex = Quaternion.Euler(0, 0, -angleIncrement * 1) * Vector3.up * radarChartSize * ((float)designPoints / employeeSkills.maxAttributePoints);
        int designVertexIndex = 2;

        Vector3 financeVertex = Quaternion.Euler(0, 0, -angleIncrement * 2) * Vector3.up * radarChartSize * ((float)financePoints / employeeSkills.maxAttributePoints);
        int financeVertexIndex = 3;

        Vector3 productManagementVertex = Quaternion.Euler(0, 0, -angleIncrement * 3) * Vector3.up * radarChartSize * ((float)productManagementPoints / employeeSkills.maxAttributePoints);
        int productManagementVertexIndex = 4;

        Vector3 qualityAssuranceVertex = Quaternion.Euler(0, 0, -angleIncrement * 4) * Vector3.up * radarChartSize * ((float)qualityAssurancePoints / employeeSkills.maxAttributePoints);
        int qualityAssuranceVertexIndex = 5;

        vertices[0] = Vector3.zero;
        vertices[programmingVertexIndex] = programmingVertex;
        vertices[designVertexIndex] = designVertex;
        vertices[financeVertexIndex] = financeVertex;
        vertices[productManagementVertexIndex] = productManagementVertex;
        vertices[qualityAssuranceVertexIndex] = qualityAssuranceVertex;

        uv[0] = Vector2.zero;
        uv[programmingVertexIndex] = Vector2.one;
        uv[designVertexIndex] = Vector2.one;
        uv[financeVertexIndex] = Vector2.one;
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
        triangles[8] = productManagementVertexIndex;

        triangles[9] = 0;
        triangles[10] = productManagementVertexIndex;
        triangles[11] = qualityAssuranceVertexIndex;

        triangles[12] = 0;
        triangles[13] = qualityAssuranceVertexIndex;
        triangles[14] = programmingVertexIndex;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        radarMeshCanvasRenderer.SetMesh(mesh);
        radarMeshCanvasRenderer.SetMaterial(radarMaterial, radarTexture);
    }
}