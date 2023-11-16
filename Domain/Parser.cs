using System.Text.Json;

namespace Domain;

public class Parser
{
    public static Survey? JsonSurvey(string content)
    {
        return JsonSerializer.Deserialize<Survey>(content);
    }

    public static Survey SRVToSurvey(string content)
    {
        Survey survey = new();
        string[] lines = content.Split("\n");
        for (var i = 0; i < lines.Length; i++)
        {
            if (lines[i].StartsWith("#") || string.IsNullOrWhiteSpace(lines[i]))
            {
                continue;
            }

            if (lines[i].StartsWith("data:"))
            {
                string[] parts = lines[i].Split(":");
                string name = parts[1].Trim();
                int j = 1;
                DynamicDataType type = new()
                {
                    Name = name,
                };
                while (j < lines.Length)
                {
                    if (lines[i + j].StartsWith("data_end"))
                    {
                        i += j;
                        break;
                    }

                    lines[i + j] = lines[i + j].Trim();
                    string[] fieldParts = lines[i + j].Split(";");
                    string fieldId = fieldParts[0].Trim();
                    string fieldType = fieldParts[1].Trim();
                    string fieldName = fieldParts[2].Trim();
                    string fieldMapping = fieldParts[3].Trim();
                    string fieldInitialValue = fieldParts[4].Trim();
                    dynamic fieldValue = ParseStringValue(fieldInitialValue, fieldType);
                    type.Fields.Add(new DynamicDataField
                    {
                        FieldId = fieldId,
                        FieldName = fieldName,
                        Value = fieldValue,
                        Mapping = ParseMapping(fieldMapping, fieldValue.GetType()),
                    });
                    j++;
                }
                survey.Data.Add(type);
            }
            else if (lines[i].StartsWith("name:"))
            {
                survey.Name = lines[i].Substring(5);
            }
            else if (lines[i].StartsWith("start:"))
            {
                survey.Start = int.Parse(lines[i].Substring(6));
            }
            else if (lines[i].StartsWith("question:"))
            {
                string[] parts = lines[i].Split(";");
                int id = int.Parse(parts[0].Substring(9));
                string text = parts[1];
                survey.Questions.Add(new Question
                {
                    Id = id,
                    Text = text,
                });
            }
            else if (lines[i].StartsWith("answer:"))
            {
                string[] parts = lines[i].Split(";");
                int questionId = int.Parse(parts[0].Substring(7));
                string text = parts[1].Trim();
                int to = int.Parse(parts[2]);
                SurveyAction? action = null;
                if (lines[i].Contains("action:"))
                {
                    string actionStr = lines[i].Split("action:")[1].Trim();
                    action = ParseAction(actionStr, survey);
                }
                survey.Answers.Add(new Answer
                {
                    QuestionId = questionId,
                    Text = text,
                    To = to,
                    Action = action,
                });

            }
            else if (lines[i].StartsWith("end:"))
            {
                string[] parts = lines[i].Split(";");
                int id = int.Parse(parts[0].Substring(4));
                string text = parts[1].Trim();
                object value;
                if (text.StartsWith("*"))
                {
                    DynamicDataType? type = survey.Data.FirstOrDefault(d => d.Name == text.Substring(1));
                    value = type ?? throw new Exception("No data type found");
                }
                else
                {
                    value = text;
                }
                survey.Ends.Add(new SurveyEnd
                {
                    Id = id,
                    Value = value,
                });
            }
        }

        return survey;
    }

    private static dynamic ParseStringValue(string value, string type)
    {
        Type t = type.Trim().ToLower() switch
        {
            "int" => typeof(int),
            "string" => typeof(string),
            "bool" => typeof(bool),
            "double" => typeof(double),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown type of data field"),
        };
        return Convert.ChangeType(value, t);
    }

    private static dynamic ParseStringValue(string value, Type type)
    {
        return Convert.ChangeType(value, type);
    }
    
    private static Dictionary<dynamic, string> ParseMapping(string mapping, Type type)
    {
        Dictionary<dynamic, string> result = new();
        mapping = mapping.Replace('{', ' ');
        mapping = mapping.Replace('}', ' ');
        mapping = mapping.Trim();
        string[] parts = mapping.Split(",");
        foreach (var part in parts)
        {
            string[] subParts = part.Split(":");
            dynamic key = Convert.ChangeType(subParts[0], type);
            string value = subParts[1];
            result.Add(key, value);
        }

        return result;
    }

    public static SurveyAction ParseAction(string action, Survey survey)
    {
        var tokens = action.Trim().Split(' ');
        string target = tokens[0];
        string actionType = tokens[1];
        string actionValue = tokens[2];
        
        DynamicDataType type = survey.Data.FirstOrDefault(t => t.Name == target.Trim().Split('.')[0]) ?? throw new Exception("No data type found");
        DynamicDataField field = type.Fields.FirstOrDefault(f => f.FieldId == target.Trim().Split('.')[1]) ?? throw new Exception("No data field found");
        dynamic value = ParseStringValue(actionValue, field.Value.GetType());
        var surveyAction = new SurveyAction
        {
            Field = field,
            Operator = actionType,
            B = value,
        };
        
        return surveyAction;
    }
}