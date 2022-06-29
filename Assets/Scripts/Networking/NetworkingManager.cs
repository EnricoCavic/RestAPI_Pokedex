using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using SimpleJSON;
using System.Threading.Tasks;

public class NetworkingManager : MonoBehaviour
{

    public string baseURL; 
    public string pkmURL;
    public string speciesNodeURL;

    PokemonInfo pokemonInfo;
    public UnityEvent<PokemonInfo> onInformationArrived;

    public async void GetPokemon(int _index)
    {
        pokemonInfo = new PokemonInfo();
        int rand = Random.Range(1, 808);
        var tasks = new Task[2];
        tasks[0] = PkmnRequest(rand);
        tasks[1] = SpecieRequest(rand);

        await Task.WhenAll(tasks);

        tasks = new Task[2];
        tasks[0] = SpriteRequest();
        tasks[1] = FieldGroupRequest();

        await Task.WhenAll(tasks);   

        onInformationArrived?.Invoke(pokemonInfo);
    }

    async Task PkmnRequest(int _index)
    {
        string dataURL = baseURL + pkmURL + _index.ToString();
        pokemonInfo.generalNode = await JSONWebRequest(dataURL);  
    }

    async Task SpecieRequest(int _index)
    {
        string dataURL = baseURL + speciesNodeURL + _index.ToString();
        pokemonInfo.speciesNode = await JSONWebRequest(dataURL);
    }


    async Task SpriteRequest()
    {
        string imgURL = pokemonInfo.frontDefaultURL;
        pokemonInfo.frontDefaultTexture = await TextureWebRequest(imgURL);                
    }

    async Task FieldGroupRequest()
    {
        var jsonRequests = new Task<JSONNode>[2];

        string fieldGroup1URL = pokemonInfo.field1URL;
        jsonRequests[0] = JSONWebRequest(fieldGroup1URL);

        string fieldGroup2URL = pokemonInfo.field2URL;
        jsonRequests[1] = JSONWebRequest(fieldGroup2URL);

        await Task.WhenAll(jsonRequests);
        pokemonInfo.field1Node = jsonRequests[0].Result;
        pokemonInfo.field2Node = jsonRequests[1].Result;

    }


    async Task<JSONNode> JSONWebRequest(string dataURL)
    {
        if(dataURL == null)
            return null;

        UnityWebRequest request = UnityWebRequest.Get(dataURL);
        await WebRequest(request);

        return JSON.Parse(request.downloadHandler.text);            
    }

    async Task<Texture> TextureWebRequest(string dataURL)
    {
        if(dataURL == null)
            return null;

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(dataURL);
        await WebRequest(request);

        Texture t = DownloadHandlerTexture.GetContent(request);
        t.filterMode = FilterMode.Point;           
        return t;
    }

    async Task WebRequest(UnityWebRequest request)   
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

public struct PokemonInfo
{
    public JSONNode generalNode;
    public JSONNode speciesNode;    
    public JSONNode field1Node;
    public JSONNode field2Node;
    public Texture frontDefaultTexture;
    public string frontDefaultURL => generalNode["sprites"]["front_default"];
    public string field1URL => speciesNode["egg_groups"][0]["url"];
    public string field2URL => speciesNode["egg_groups"][1]["url"];

    public string genus => speciesNode["genera"][7]["genus"];
    public string latestEnFlavorText => FindLastestEnFlavorText(speciesNode["flavor_text_entries"]);
    public string field1Name => field1Node["names"][6]["name"];
    public string field2Name => field2Node["names"][6]["name"];
    public string GetTypeName(int i) => generalNode["types"][i]["type"]["name"];
    public int GetStatEffort(int i) => generalNode["stats"][i]["effort"];
    public string GetStatName(int i) => generalNode["stats"][i]["stat"]["name"];

    string FindLastestEnFlavorText(JSONNode _flavorTexts)
    {
        for (int i = _flavorTexts.Count; i > 0; i--)
        {
            if(_flavorTexts[i]["language"]["name"] == "en")
            {
                string txt = _flavorTexts[i]["flavor_text"];
                txt = txt.Replace("\n", " ");
                return txt;           
            }
        }      

        return "";  
    }
}
