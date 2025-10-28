using UnityEngine;
using TMPro;

public class QuestionnaireView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questionnaireNameText;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI questionIndexText;
    [SerializeField] private TextMeshProUGUI instructionText; 

    // Main UI control
    public void UpdateView(string qName, string qText, int currentIndex, int totalQuestions)
    {
        SetQuestionnaireName(qName);
        SetQuestionText(qText);
        SetQuestionIndex(currentIndex, totalQuestions);
        SetInstructionText(""); 
    }

    // for individual control

    public void SetQuestionnaireName(string text)
    {
        if (questionnaireNameText != null)
        {
            questionnaireNameText.text = text;
            questionnaireNameText.gameObject.SetActive(!string.IsNullOrEmpty(text));
        }
    }

    public void SetQuestionText(string text)
    {
        if (questionText != null)
        {
            questionText.text = text;
            questionText.gameObject.SetActive(!string.IsNullOrEmpty(text));
        }
    }

    public void SetQuestionIndex(int currentIndex, int totalQuestions)
    {
        if (questionIndexText != null)
        {
            bool showIndex = currentIndex >= 0; // Show index only for actual questions
            questionIndexText.gameObject.SetActive(showIndex);
            if (showIndex)
            {
                questionIndexText.text = $"{currentIndex + 1} / {totalQuestions}";
            }
        }
    }

    public void SetInstructionText(string text)
    {
        if (instructionText != null)
        {
            instructionText.text = text;
            instructionText.gameObject.SetActive(!string.IsNullOrEmpty(text)); 
        }
    }

    public void SetupTitleScreen(string title, string instructions)
    {
        SetQuestionnaireName("");
        SetQuestionIndex(-1, 0);  
        SetQuestionText(title);  
        SetInstructionText(instructions); 
    }
}