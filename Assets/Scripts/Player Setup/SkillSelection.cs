using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelection : MonoBehaviour
{
    PlayerSkills playerSkills;

    private bool programmingSelected = false;
    private bool designSelected = false;
    private bool financeSelected = false;
    // private bool marketingSelected = false;
    // private bool dataSelected = false;
    // private bool hrSelected = false;
    private bool productSelected = false;
    private bool qaSelected = false;

    [SerializeField] private Button programmingButton;
    [SerializeField] private Button designButton;
    [SerializeField] private Button financeButton;
    // [SerializeField] private Button marketingButton;
    // [SerializeField] private Button dataButton;
    // [SerializeField] private Button hrButton;
    [SerializeField] private Button productButton;
    [SerializeField] private Button qaButton;

    private int selections = 2;
    [SerializeField] private TextMeshProUGUI selectionsText;

    private void Awake()
    {
        playerSkills = GameObject.FindWithTag("Player").GetComponent<PlayerSkills>();
    }

    private void Update()
    {
        selectionsText.text = $"Choose {selections} skills that best represent your abilities.";
    }

    public void ChooseSkill(string skillName)
    {
        switch (skillName)
        {
            case "programming":
                if (!programmingSelected && selections > 0)
                {
                    playerSkills.skills["Programming"]++;
                    Debug.Log("Programming skill level: " + playerSkills.skills["Programming"]);
                    programmingSelected = true;
                    programmingButton.GetComponent<Image>().color = Color.gray;
                    selections--;
                }
                else if (programmingSelected)
                {
                    playerSkills.skills["Programming"]--;
                    Debug.Log("Programming skill level: " + playerSkills.skills["Programming"]);
                    programmingSelected = false;
                    programmingButton.GetComponent<Image>().color = new Color(0.9607843f, 0.9607843f, 0.8627451f, 1f);
                    selections++;
                }
                break;

            case "design":
                if (!designSelected && selections > 0)
                {
                    playerSkills.skills["Design"]++;
                    Debug.Log("Design skill level: " + playerSkills.skills["Design"]);
                    designSelected = true;
                    designButton.GetComponent<Image>().color = Color.gray;
                    selections--;
                }
                else if (designSelected)
                {
                    playerSkills.skills["Design"]--;
                    Debug.Log("Design skill level: " + playerSkills.skills["Design"]);
                    designSelected = false;
                    designButton.GetComponent<Image>().color = new Color(0.9607843f, 0.9607843f, 0.8627451f, 1f);
                    selections++;
                }
                break;

            case "finance":
                if (!financeSelected && selections > 0)
                {
                    playerSkills.skills["Finance"]++;
                    Debug.Log("Finance skill level: " + playerSkills.skills["Finance"]);
                    financeSelected = true;
                    financeButton.GetComponent<Image>().color = Color.gray;
                    selections--;
                }
                else if (financeSelected)
                {
                    playerSkills.skills["Finance"]--;
                    Debug.Log("Finance skill level: " + playerSkills.skills["Finance"]);
                    financeSelected = false;
                    financeButton.GetComponent<Image>().color = new Color(0.9607843f, 0.9607843f, 0.8627451f, 1f);
                    selections++;
                }
                break;

            /*case "marketing":
                if (!marketingSelected && selections > 0)
                {
                    playerSkills.skills["Marketing"]++;
                    Debug.Log("Marketing skill level: " + playerSkills.skills["Marketing"]);
                    marketingSelected = true;
                    marketingButton.GetComponent<Image>().color = Color.gray;
                    selections--;
                }
                else if (marketingSelected)
                {
                    playerSkills.skills["Marketing"]--;
                    Debug.Log("Marketing skill level: " + playerSkills.skills["Marketing"]);
                    marketingSelected = false;
                    marketingButton.GetComponent<Image>().color = new Color(0.9607843f, 0.9607843f, 0.8627451f, 1f);
                    selections++;
                }
                break;*/

            /*case "data":
                if (!dataSelected && selections > 0)
                {
                    playerSkills.skills["Data Analysis"]++;
                    Debug.Log("Data Analysis skill level: " + playerSkills.skills["Data Analysis"]);
                    dataSelected = true;
                    dataButton.GetComponent<Image>().color = Color.gray;
                    selections--;
                }
                else if (dataSelected)
                {
                    playerSkills.skills["Data Analysis"]--;
                    Debug.Log("Data Analysis skill level: " + playerSkills.skills["Data Analysis"]);
                    dataSelected = false;
                    dataButton.GetComponent<Image>().color = new Color(0.9607843f, 0.9607843f, 0.8627451f, 1f);
                    selections++;
                }
                break;*/

            /*case "hr":
                if (!hrSelected && selections > 0)
                {
                    playerSkills.skills["Human Resources"]++;
                    Debug.Log("Human Resources skill level: " + playerSkills.skills["Human Resources"]);
                    hrSelected = true;
                    hrButton.GetComponent<Image>().color = Color.gray;
                    selections--;
                }
                else if (hrSelected)
                {
                    playerSkills.skills["Human Resources"]--;
                    Debug.Log("Human Resources skill level: " + playerSkills.skills["Human Resources"]);
                    hrSelected = false;
                    hrButton.GetComponent<Image>().color = new Color(0.9607843f, 0.9607843f, 0.8627451f, 1f);
                    selections++;
                }
                break;*/

            case "product":
                if (!productSelected && selections > 0)
                {
                    playerSkills.skills["Product Management"]++;
                    Debug.Log("Product Management skill level: " + playerSkills.skills["Product Management"]);
                    productSelected = true;
                    productButton.GetComponent<Image>().color = Color.gray;
                    selections--;
                }
                else if (productSelected)
                {
                    playerSkills.skills["Product Management"]--;
                    Debug.Log("Product Management skill level: " + playerSkills.skills["Product Management"]);
                    productSelected = false;
                    productButton.GetComponent<Image>().color = new Color(0.9607843f, 0.9607843f, 0.8627451f, 1f);
                    selections++;
                }
                break;

            case "qa":
                if (!qaSelected && selections > 0)
                {
                    playerSkills.skills["Quality Assurance"]++;
                    Debug.Log("Quality Assurance skill level: " + playerSkills.skills["Quality Assurance"]);
                    qaSelected = true;
                    qaButton.GetComponent<Image>().color = Color.gray;
                    selections--;
                }
                else if (qaSelected)
                {
                    playerSkills.skills["Quality Assurance"]--;
                    Debug.Log("Quality Assurance skill level: " + playerSkills.skills["Quality Assurance"]);
                    qaSelected = false;
                    qaButton.GetComponent<Image>().color = new Color(0.9607843f, 0.9607843f, 0.8627451f, 1f);
                    selections++;
                }
                break;
        }
    }

    public void ContinueButton()
    {
        if (selections == 0)
        {
            GetComponent<Animator>().Play("FadeOut");
        }
    }
}
