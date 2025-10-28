using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;


public class QuestionnaireModel : MonoBehaviour
{
    private TextAsset jsonFile;

    private QuestionnaireData data;
    public int currentQuestionIndex { get; private set; }
    // Using float for numeric scores
    public Dictionary<string, float> sessionResponses = new Dictionary<string, float>();

    private const string LOGFileName = "VR_Questionnaire_Data.json";


    public void InitializeWithData(TextAsset newJsonFile)
    {
        jsonFile = newJsonFile;
        currentQuestionIndex = 0;
        sessionResponses.Clear();

        if (newJsonFile != null) 
        {
            try
            {
                data = JsonUtility.FromJson<QuestionnaireData>(newJsonFile.text);
                if (data == null || data.questions == null)
                {
                    data = new QuestionnaireData { questions = new List<QuestionData>() };
                    Debug.LogError($"JSON Data in '{newJsonFile.name}' is empty or questions list is missing/null.");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to deserialize JSON in {newJsonFile.name}: {e.Message}");
                data = new QuestionnaireData { questions = new List<QuestionData>() };
            }
        }
        else
        {
            data = new QuestionnaireData { questions = new List<QuestionData>() };
            Debug.LogError("JSON File provided to InitializeWithData is null.");
        }

    }

    public QuestionData GetCurrentQuestion()
    {
        if (data != null && data.questions != null && currentQuestionIndex >= 0 && currentQuestionIndex < data.questions.Count)
        {
            return data.questions[currentQuestionIndex];
        }
        return null;
    }

    public int GetTotalQuestions()
    {
        return (data != null && data.questions != null) ? data.questions.Count : 0;
    }

    public void AdvanceQuestion() => currentQuestionIndex++;
    public void RetreatQuestion() => currentQuestionIndex--;

    public void LogResponse(string qID, float score)
    {
        sessionResponses[qID] = score;
        //Debug.Log($"Logged response for {qID}: {score}");
    }

    // Accepts participantID from Controller
    public void SubmitData(string participantID)
    {
        // 1. Create the new result and populate it with all necessary info
        QuestionnaireResult newResult = new QuestionnaireResult
        {
            questionnaireName = this.QuestionnaireName,
            timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            responses = new List<ResponseItem>()
        };

        foreach (var response in sessionResponses)
        {
            newResult.responses.Add(new ResponseItem(response.Key, response.Value));
        }

        string filePath = Path.Combine(Application.persistentDataPath, LOGFileName);
        AllResultsData allResultsData;

        // Load existing data if the file exists
        if (File.Exists(filePath))
        {
            try
            {
                string existingJson = File.ReadAllText(filePath);
                allResultsData = JsonUtility.FromJson<AllResultsData>(existingJson);
                if (allResultsData == null || allResultsData.participants == null)
                    allResultsData = new AllResultsData();
            }
            catch (Exception e)
            {
                Debug.LogError($"[QuestionnaireModel] Failed to read file: {e.Message}");
                allResultsData = new AllResultsData();
            }
        }
        else
        {
            allResultsData = new AllResultsData();
        }

        // Find the correct participant or create a new one
        ParticipantData participant = allResultsData.participants
            .Find(p => p.participantID == participantID);

        if (participant == null)
        {
            participant = new ParticipantData { participantID = participantID };
            allResultsData.participants.Add(participant);
        }
       
        // 4. Add the new result directly to the participant's list
        participant.questionnaireResults.Add(newResult);

        // 5. Save the updated data back to the file
        try
        {
            string jsonToSave = JsonUtility.ToJson(allResultsData, true);
            File.WriteAllText(filePath, jsonToSave);
            Debug.Log($"Saved data for {participantID} at: {filePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save JSON: {e.Message}");
        }
    }

    public QuestionData GetQuestionAtIndex(int index)
    {
        if (data != null && data.questions != null && index >= 0 && index < data.questions.Count)
        {
            return data.questions[index];
        }
        return null;
    }

    public string QuestionnaireName
    {
        get { return data != null ? data.questionnaireName : "No Questionnaire Loaded"; }
    }
}