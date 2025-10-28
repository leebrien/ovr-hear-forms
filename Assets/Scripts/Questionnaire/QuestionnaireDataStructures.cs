using System;
using System.Collections.Generic;

[Serializable]
public class ResponseItem
{
    public string id;
    public float score;

    public ResponseItem(string id, float score)
    {
        this.id = id;
        this.score = score;
    }
}

[Serializable]
public class QuestionnaireResult
{
    public string questionnaireName;
    public string timestamp;
    public List<ResponseItem> responses = new List<ResponseItem>();
}


[Serializable]
public class ParticipantData
{
    public string participantID;
    public List<QuestionnaireResult> questionnaireResults = new List<QuestionnaireResult>();
}

[Serializable]
public class AllResultsData
{
    public List<ParticipantData> participants = new List<ParticipantData>();
}




[Serializable]
public class QuestionData
{
    // Common fields
    public string id;
    public string type;
    public string text;
    public string lowLabel;
    public string highLabel;
    // Slider-specific fields
    public float minValue;
    public float maxValue;
    // Radio-specific field
    public int steps;
}

[Serializable]
public class QuestionnaireData
{
    public string questionnaireName;
    public List<QuestionData> questions;
}