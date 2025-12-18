using UnityEngine;
using TMPro;

public class AddPlanUI : MonoBehaviour
{
    public TMP_InputField nameInput;
    public PlanListUI planListUI;

    public void Add()
    {
        if (nameInput == null)
        {
            Debug.LogError("nameInput is not assigned");
            return;
        }

        if (planListUI == null)
        {
            Debug.LogError("planListUI is not assigned");
            return;
        }

        if (string.IsNullOrEmpty(nameInput.text))
            return;

        planListUI.AddItem(nameInput.text);
        nameInput.text = "";
    }
}
