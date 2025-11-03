using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class QuestionnaireController : MonoBehaviour
{
    public static event Action OnQuestionnaireCompleted;

    private enum QuestionnaireState { ShowingTitle, ShowingQuestions }
    private QuestionnaireState currentState = QuestionnaireState.ShowingTitle;

    [SerializeField] private QuestionnaireModel model;
    [SerializeField] private GameObject questionnairePanel;
    [SerializeField] private QuestionnaireView questionnaireUIView;

    [SerializeField] private GameObject sliderViewGO;
    [SerializeField] private GameObject radioViewGO;
    [SerializeField] private GameObject multipleChoiceViewGO;

    private SliderView sliderView;
    private ToggleGroupView radioView;
    private ToggleGroupView multipleChoiceView;

    [SerializeField] private Button forwardButton;
    [SerializeField] private TextMeshProUGUI forwardButtonText;
    [SerializeField] private Button backButton;

    private bool isSubmitting = false;
    private string currentParticipantID;

    void Awake()
    {
        sliderView = sliderViewGO.GetComponent<SliderView>();
        radioView = radioViewGO.GetComponent<ToggleGroupView>();
        multipleChoiceView = multipleChoiceViewGO.GetComponent<ToggleGroupView>();

        ToggleGroupView.OnSelectionMade += HandleSelectionMade;
    }

    private void OnDestroy()
    {
        ToggleGroupView.OnSelectionMade -= HandleSelectionMade;
    }

    private void HandleSelectionMade()
    {
        if (currentState == QuestionnaireState.ShowingQuestions)
        {
            forwardButton.interactable = true;
        }
    }

    public void StartQuestionnaire(TextAsset jsonFile, string participantID)
    {
        currentParticipantID = participantID;

        model.InitializeWithData(jsonFile);
        questionnairePanel.SetActive(true);
        isSubmitting = false;

        currentState = QuestionnaireState.ShowingTitle;

        forwardButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();
        forwardButton.onClick.AddListener(OnForwardClicked);

        if (model.GetTotalQuestions() > 0)
        {
            DisplayTitleScreen();
        }
        else
        {
            Debug.LogError("No questionnaires loaded. Cannot start.");
            questionnairePanel.SetActive(false);
        }
    }

    private void DisplayTitleScreen()
    {
        DeactivateAllViews();

        string instructions = "";
        QuestionData firstQuestion = model.GetQuestionAtIndex(0); // Use the existing helper
        if (firstQuestion != null)
        {
            switch (firstQuestion.type)
            {
                case "slider":
                    instructions = "You will use a slider to indicate your responses.";
                    break;
                case "radio":
                case "multipleChoice":
                default:
                    instructions = "Please respond to the following questions.";
                    break;
            }
        }

        // Use the dedicated setup method in the view
        questionnaireUIView.SetupTitleScreen(model.QuestionnaireName, instructions);

        forwardButton.interactable = true;
        forwardButtonText.text = "Start";
        backButton.gameObject.SetActive(false);
    }


    private void DeactivateAllViews()
    {
        sliderViewGO.SetActive(false);
        radioViewGO.SetActive(false);
        multipleChoiceViewGO.SetActive(false);
    }

    private void DisplayCurrentQuestion()
    {
        QuestionData qData = model.GetCurrentQuestion();

        if (qData == null)
        {
            Debug.LogError("Tried to display question but qData is null!");
            return;
        }

        DeactivateAllViews();
        questionnaireUIView.SetInstructionText("");


        if (qData.type == "slider")
        {
            sliderViewGO.SetActive(true);
            forwardButton.interactable = true;
            questionnaireUIView.UpdateView(model.QuestionnaireName, qData.text, model.currentQuestionIndex, model.GetTotalQuestions());
            sliderView.UpdateView(qData.minValue, qData.maxValue, qData.choices.ToArray());
        }
        else if (qData.type == "radio")
        {
            radioViewGO.SetActive(true);
            forwardButton.interactable = false;
            questionnaireUIView.UpdateView(model.QuestionnaireName, qData.text, model.currentQuestionIndex, model.GetTotalQuestions());

            // Defensive: use first and last choice for labels
            string leftLabel = (qData.choices != null && qData.choices.Count > 0) ? qData.choices[0] : "";
            string rightLabel = (qData.choices != null && qData.choices.Count > 1) ? qData.choices[qData.choices.Count - 1] : "";

            radioView.UpdateRadioView(qData.choices, qData.steps);
        }

        else if (qData.type == "multipleChoice")
        {
            multipleChoiceViewGO.SetActive(true);
            forwardButton.interactable = false;
            questionnaireUIView.UpdateView(model.QuestionnaireName, qData.text, model.currentQuestionIndex, model.GetTotalQuestions());
            multipleChoiceView.UpdateMultipleChoiceView(qData.choices);
        }

        if (model.sessionResponses.ContainsKey(qData.id))
        {
            float savedScore = model.sessionResponses[qData.id];
            forwardButton.interactable = true;

            if (qData.type == "slider") { sliderView.SetScore(savedScore); }
            else if (qData.type == "radio") { radioView.SetScore(savedScore); }
            else if (qData.type == "multipleChoice") { multipleChoiceView.SetScore(savedScore); }
        }

        UpdateNavigationButtons();
    }

    private void UpdateNavigationButtons()
    {
        int total = model.GetTotalQuestions();
        int index = model.currentQuestionIndex;

        backButton.gameObject.SetActive(index > 0);
        backButton.onClick.RemoveListener(OnBackClicked); // Always remove first
        if (backButton.gameObject.activeSelf)
        {
            backButton.onClick.AddListener(OnBackClicked); // Add if active
        }

        forwardButtonText.text = (index == total - 1) ? "Submit" : "Next";
    }

    public void OnForwardClicked()
    {
        if (isSubmitting) return;

        if (currentState == QuestionnaireState.ShowingTitle)
        {
            currentState = QuestionnaireState.ShowingQuestions;
            DisplayCurrentQuestion();
        }
        else if (currentState == QuestionnaireState.ShowingQuestions)
        {
            QuestionData qData = model.GetCurrentQuestion();
            if (qData == null) return;

            float score = GetCurrentScore(qData);
            if (score != -1f)
            {
                model.LogResponse(qData.id, score);
            }

            if (model.currentQuestionIndex == model.GetTotalQuestions() - 1)
            {
                isSubmitting = true;
                model.SubmitData(currentParticipantID);
                questionnairePanel.SetActive(false);
                Debug.Log($"Questionnaire '{model.QuestionnaireName}' finished for {currentParticipantID}.");
                OnQuestionnaireCompleted?.Invoke();
                return;
            }
            else
            {
                model.AdvanceQuestion();
                DisplayCurrentQuestion();
            }
        }
    }

    public void OnBackClicked()
    {
        if (currentState != QuestionnaireState.ShowingQuestions) return;

        QuestionData qData = model.GetCurrentQuestion();
        if (qData != null)
        {
            float score = GetCurrentScore(qData);
            if (score != -1f)
            {
                model.LogResponse(qData.id, score);
            }
        }

        if (model.currentQuestionIndex > 0)
        {
            model.RetreatQuestion();
            DisplayCurrentQuestion();
        }
    }

    private float GetCurrentScore(QuestionData qData)
    {
        if (qData == null) return -1f;

        if (qData.type == "slider") { return sliderView.GetScore(); }
        else if (qData.type == "radio") { return radioView.GetScore(); }
        else if (qData.type == "multipleChoice") { return multipleChoiceView.GetScore(); }
        return -1f;
    }
}