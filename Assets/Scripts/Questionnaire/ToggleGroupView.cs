using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

public class ToggleGroupView : MonoBehaviour
{
    public static event Action OnSelectionMade;

    [SerializeField] private List<Toggle> toggles;
    [SerializeField] private List<TextMeshProUGUI> toggleLabels;

    [Header("Radio View Labels (optional)")]
    [SerializeField] private TextMeshProUGUI radioLeftLabelText;
    [SerializeField] private TextMeshProUGUI radioRightLabelText;

    private ToggleGroup toggleGroup;

    void Awake()
    {
        toggleGroup = GetComponent<ToggleGroup>();
        if (toggleGroup == null)
            Debug.LogError($"No ToggleGroup component found on {name}");

        foreach (var toggle in toggles)
            toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnToggleChanged(bool isOn)
    {
        if (isOn)
            OnSelectionMade?.Invoke();
    }

    // For Radio View (7-step)
    public void UpdateRadioView(List<string> choices, int steps)
    {
        string left = choices != null && choices.Count > 0 ? choices[0] : "";
        string right = choices != null && choices.Count > 1 ? choices[choices.Count - 1] : "";

        if (radioLeftLabelText) radioLeftLabelText.text = left;
        if (radioRightLabelText) radioRightLabelText.text = right;

        for (int i = 0; i < toggles.Count; i++)
            toggles[i].gameObject.SetActive(i < steps);

        toggleGroup?.SetAllTogglesOff(false);
    }


    // For Multiple Choice (variable options)
    public void UpdateMultipleChoiceView(List<string> choices)
    {
        for (int i = 0; i < toggles.Count; i++)
        {
            bool active = i < choices.Count;
            toggles[i].gameObject.SetActive(active);

            if (active && i < toggleLabels.Count)
                toggleLabels[i].text = choices[i];
        }

        toggleGroup?.SetAllTogglesOff(false);
    }

    public void ResetView() => toggleGroup?.SetAllTogglesOff(false);

    public float GetScore()
    {
        for (int i = 0; i < toggles.Count; i++)
            if (toggles[i].isOn)
                return i;
        return -1f;
    }

    public void SetScore(float score)
    {
        int index = Mathf.RoundToInt(score);
        if (index >= 0 && index < toggles.Count)
            toggles[index].isOn = true;
    }
}
