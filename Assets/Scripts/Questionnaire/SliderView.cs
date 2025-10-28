using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SliderView : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI lowLabelText;
    [SerializeField] private TextMeshProUGUI highLabelText;

    public void UpdateView(float min, float max, string lowLabel, string highLabel)
    {
        lowLabelText.text = lowLabel;
        highLabelText.text = highLabel;

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