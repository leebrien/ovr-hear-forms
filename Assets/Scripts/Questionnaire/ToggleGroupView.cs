using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

public class ToggleGroupView : MonoBehaviour
{
    public static event Action OnSelectionMade;

    [SerializeField] private List<Toggle> toggles;

    [SerializeField] private TextMeshProUGUI lowLabelText;
    [SerializeField] private TextMeshProUGUI highLabelText;

    private ToggleGroup toggleGroup;
    private bool hasLabels;

    void Awake()
    {
        // Find the ToggleGroup
        toggleGroup = GetComponent<ToggleGroup>();
        if (toggleGroup == null)
        {
            Debug.LogError($"No ToggleGroup component.");
        }

        // Check if optional labels were assigned (for 7-toggle radio view)
        hasLabels = (lowLabelText != null && highLabelText != null);

        foreach (var toggle in toggles)
        {
            toggle.onValueChanged.AddListener(OnToggleChanged);
        }
    }

    private void OnToggleChanged(bool isOn)
    {
        if (isOn)
        {
            OnSelectionMade?.Invoke();
        }
    }

    // This UpdateView is for the 7-toggle view
    public void UpdateView(string lowLabel, string highLabel, int steps)
    {
        // 1. Update semantic labels (if they exist)
        if (hasLabels)
        {
            lowLabelText.text = lowLabel;
            highLabelText.text = highLabel;
        }

        // 2. Activate the correct number of toggles
        for (int i = 0; i < toggles.Count; i++)
        {
            toggles[i].gameObject.SetActive(i < steps);
        }

        // 3. Deselect all
        if (toggleGroup != null)
        {
            toggleGroup.SetAllTogglesOff(false);
        }
    }

    public void ResetView()
    {
        if (toggleGroup != null)
        {
            toggleGroup.SetAllTogglesOff(false);
        }
    }

    public float GetScore()
    {
        for (int i = 0; i < toggles.Count; i++)
        {
            if (toggles[i].isOn)
            {
                return i;
            }
        }
        return -1f;
    }

    public void SetScore(float score)
    {
        int index = Mathf.RoundToInt(score);

        if (index >= 0 && index < toggles.Count)
        {
            toggles[index].isOn = true;
        }
    }
}