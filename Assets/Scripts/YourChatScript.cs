using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class YourChatScript : MonoBehaviour
{
    private string apiKey = "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"; // 替换为你的ChatGPT API密钥或访问令牌
    private string chatEndpoint = "https://api.openai.com/v1/chat/completions"; // 替换为ChatGPT API的聊天接口端点

    public IEnumerator SendChatRequest(string message)
    {
        // 设置请求头
        var headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");
        headers.Add("Authorization", "Bearer " + apiKey);

        // 创建请求数据
        string requestData = "{\"model\": \"gpt-3.5-turbo\", \"messages\": [{\"role\": \"system\", \"content\": \"You are a helpful assistant.\"}, {\"role\": \"user\", \"content\": \"" + message + "\"}]}";

        // 发送请求
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
                // 处理响应数据
                string response = www.downloadHandler.text;
                Debug.Log("Chat Response: " + response);
            }
        }
    }

    // 调用示例
    private void Start()
    {
        StartCoroutine(SendChatRequest("Hello, how are you?"));
    }
}