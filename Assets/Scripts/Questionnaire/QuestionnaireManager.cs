using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class QuestionnaireManager : MonoBehaviour
{
    [Header("Questionnaire Files")]
    [SerializeField] private List<TextAsset> questionnaireSequence;

    [Header("References")]
    // Responsible for displaying and handling the UI.
    [SerializeField] private QuestionnaireController questionnaireController;

  
    private int _currentSequenceIndex; // Tracks progress through the questionnaireSequence list

    // Some hardcoded/fallback values.
    // modify this for you own implementations
    private const string DefaultParticipantID = "P_Standalone";

    void Start()
    {
        // Since these are just hardcoded values, adjust based on your own context
        string participantID = DefaultParticipantID;

        Debug.Log($"Standalone mode. Using default ID: {participantID}");

        QuestionnaireController.OnQuestionnaireCompleted += HandleQuestionnaireCompleted;

        if (questionnaireController != null)
        {
            _currentSequenceIndex = 0;
            StartNextQuestionnaire(participantID);
        }
        else
        {
            Debug.LogError("QuestionnaireController not assigned in Inspector!");
        }
    }

    private void OnDestroy()
    {
        QuestionnaireController.OnQuestionnaireCompleted -= HandleQuestionnaireCompleted;
    }

    private void StartNextQuestionnaire(string participantID)
    {
        if (_currentSequenceIndex >= questionnaireSequence.Count)
        {
            Debug.Log($"Reached end of questionnaire sequence. Unloading scene.");

            HandleSequenceFinished();
            return;
        }

        TextAsset nextFile = questionnaireSequence[_currentSequenceIndex];
        Debug.Log($"Starting questionnaire file: {nextFile.name}");
        questionnaireController.StartQuestionnaire(nextFile, participantID);
    }

    private void HandleQuestionnaireCompleted()
    {
        // Default session data (change if needed)
        string participantID = DefaultParticipantID;

        _currentSequenceIndex++;
        StartNextQuestionnaire(participantID);
    }

    private void HandleSequenceFinished()
    {

        // Modify if needed

        Debug.Log("Questionnaire sequence is complete.");

    }
}