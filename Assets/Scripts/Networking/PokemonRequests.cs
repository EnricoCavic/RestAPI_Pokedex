using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using SimpleJSON;

public class PokemonRequests : RequestSender
{
    public async Task<JSONNode> PkmnRequest(string dataURL)
    {
        return await JSONWebRequest(dataURL);  
    }

    public async Task<JSONNode> SpecieRequest(string dataURL)
    {
        return await JSONWebRequest(dataURL);
    }


    public async Task<Information> SpriteRequest(string imgURL)
    {
        Information inf = new Information();
        inf.texture = await TextureWebRequest(imgURL);
        return inf;              
    }

    public async Task<Information> FieldGroupRequest(string fieldGroup1URL, string fieldGroup2URL)
    {
        var jsonRequests = new Task<JSONNode>[2];

        jsonRequests[0] = JSONWebRequest(fieldGroup1URL);
        jsonRequests[1] = JSONWebRequest(fieldGroup2URL);

        await Task.WhenAll(jsonRequests);
        JSONNode[] fieldsArray = new JSONNode[2];
        fieldsArray[0] = jsonRequests[0].Result;
        fieldsArray[1] = jsonRequests[1].Result;

        Information inf = new Information();
        inf.fieldsArray = new JSONNode[2];
        inf.fieldsArray[0] = fieldsArray[0];
        inf.fieldsArray[1] = fieldsArray[1];
        return inf;

    }
}
