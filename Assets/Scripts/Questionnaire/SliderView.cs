using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderView : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI lowLabelText;
    [SerializeField] private TextMeshProUGUI highLabelText;

    public void UpdateView(float min, float max, string[] choices)
    {
        if (choices != null && choices.Length >= 2)
        {
            lowLabelText.text = choices[0];
            highLabelText.text = choices[1];
        }
        else
        {
            lowLabelText.text = "";
            highLabelText.text = "";
        }

        slider.minValue = min;
        slider.maxValue = max;
        slider.wholeNumbers = true;
        slider.value = min;
    }

    public float GetScore()
    {
        return slider.value;
    }

    public void SetScore(float score)
    {
        slider.value = score;
    }
}
