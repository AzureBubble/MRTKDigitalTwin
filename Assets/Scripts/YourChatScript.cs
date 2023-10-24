using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class YourChatScript : MonoBehaviour
{
    private string apiKey = "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"; // �滻Ϊ���ChatGPT API��Կ���������
    private string chatEndpoint = "https://api.openai.com/v1/chat/completions"; // �滻ΪChatGPT API������ӿڶ˵�

    public IEnumerator SendChatRequest(string message)
    {
        // ��������ͷ
        var headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");
        headers.Add("Authorization", "Bearer " + apiKey);

        // ������������
        string requestData = "{\"model\": \"gpt-3.5-turbo\", \"messages\": [{\"role\": \"system\", \"content\": \"You are a helpful assistant.\"}, {\"role\": \"user\", \"content\": \"" + message + "\"}]}";

        // ��������
        using (UnityWebRequest www = UnityWebRequest.Post(chatEndpoint, requestData))
        {
            foreach (var header in headers)
            {
                www.SetRequestHeader(header.Key, header.Value);
            }

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error sending chat request: " + www.error);
            }
            else
            {
                // ������Ӧ����
                string response = www.downloadHandler.text;
                Debug.Log("Chat Response: " + response);
            }
        }
    }

    // ����ʾ��
    private void Start()
    {
        StartCoroutine(SendChatRequest("Hello, how are you?"));
    }
}