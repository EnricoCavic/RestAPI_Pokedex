using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using SimpleJSON;
using System.Threading.Tasks;

public class RequestSender : MonoBehaviour
{

    public async Task<JSONNode> JSONWebRequest(string dataURL)
    {
        if(dataURL == null)
            return null;

        UnityWebRequest request = UnityWebRequest.Get(dataURL);
        await WebRequest(request);

        return JSON.Parse(request.downloadHandler.text);            
    }

    public async Task<Texture> TextureWebRequest(string dataURL)
    {
        if(dataURL == null)
            return null;

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(dataURL);
        await WebRequest(request);

        Texture t = DownloadHandlerTexture.GetContent(request);
        t.filterMode = FilterMode.Point;           
        return t;
    }

    public async Task WebRequest(UnityWebRequest request)   
    {
        request.SendWebRequest();

        while(!request.isDone)
            await Task.Yield();

        if(request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
            return;
        }
    }
}

