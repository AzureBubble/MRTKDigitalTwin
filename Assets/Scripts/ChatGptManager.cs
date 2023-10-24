using System.Collections;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ChatGptManager : MonoBehaviour
{
    public Text chatText;
    public InputField inputField;

    private string chatGptUrl = "https://api.openai.com/v1/engines/davinci/completions";
    private string apiKey = "YOUR_OPENAI_API_KEY";

    private void Awake()
    {
        // 启用完整的SSL证书验证
        ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
    }

    public void SendMessage()
    {
        string message = inputField.text;
        StartCoroutine(SendChatRequest(message));
    }

    private IEnumerator SendChatRequest(string message)
    {
        string requestData = "{\"prompt\": \"" + message + "\", \"max_tokens\": 100}";
        UnityWebRequest request = new UnityWebRequest(chatGptUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseJson = request.downloadHandler.text;
            ChatGptResponse response = JsonUtility.FromJson<ChatGptResponse>(responseJson);
            string reply = response.choices[0].text.Trim();
            AddChatMessage("Player: " + message);
            AddChatMessage("AI: " + reply);
        }
        else
        {
            Debug.LogError("Error sending chat request: " + request.error);
        }
    }

    private void AddChatMessage(string message)
    {
        chatText.text += message + "\n";
    }
}

[System.Serializable]
public class ChatGptResponse
{
    public ChatGptChoice[] choices;
}

[System.Serializable]
public class ChatGptChoice
{
    public string text;
}