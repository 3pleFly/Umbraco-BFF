
using System.Text;
using System.Text.Json;

namespace Form.Service;

public static class JsonHelper
{
    public static void FindProperties(byte[] jsonUtf8Bytes, string targetPropertyName)
    {
        var reader = new Utf8JsonReader(jsonUtf8Bytes);

        while (reader.Read())
        {
            if (reader.TokenType != JsonTokenType.PropertyName) continue;

            string propName = reader.GetString();

            if (propName == targetPropertyName)
            {
                if (!reader.Read()) break;

                if (reader.TokenType == JsonTokenType.String)
                {
                    string value = reader.GetString();
                    Console.WriteLine($"Found {targetPropertyName}: {value}");
                }

                else if (reader.TokenType == JsonTokenType.Number)
                {
                    Console.WriteLine($"Found {targetPropertyName}: {reader.GetDouble()}");
                }

                else if (reader.TokenType == JsonTokenType.StartObject ||
                         reader.TokenType == JsonTokenType.StartArray)
                {
                    Console.WriteLine($"Found {targetPropertyName}: (complex JSON structure)");
                }
                else if (reader.TokenType == JsonTokenType.True ||
                         reader.TokenType == JsonTokenType.False)
                {
                    Console.WriteLine($"Found {targetPropertyName}: {reader.GetBoolean()}");
                }
                else if (reader.TokenType == JsonTokenType.Null)
                {
                    Console.WriteLine($"Found {targetPropertyName}: null");
                }
            }
        }
    }
}