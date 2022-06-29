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
    public string speciesURL;

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
        pokemonInfo.general = await JSONWebRequest(dataURL);  
    }

    async Task SpecieRequest(int _index)
    {
        string dataURL = baseURL + speciesURL + _index.ToString();
        pokemonInfo.species = await JSONWebRequest(dataURL);
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
        Debug.Log(fieldGroup1URL);
        jsonRequests[0] = JSONWebRequest(fieldGroup1URL);

        string fieldGroup2URL = pokemonInfo.field2URL;
        Debug.Log(fieldGroup2URL);
        jsonRequests[1] = JSONWebRequest(fieldGroup2URL);

        await Task.WhenAll(jsonRequests);
        pokemonInfo.field1 = jsonRequests[0].Result;
        pokemonInfo.field2 = jsonRequests[1].Result;

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
    public JSONNode general;
    public JSONNode species;    
    public JSONNode field1;
    public JSONNode field2;
    public string frontDefaultURL => general["sprites"]["front_default"];
    public string field1URL => species["egg_groups"][0]["url"];
    public string field2URL => species["egg_groups"][1]["url"];
    public Texture frontDefaultTexture;
}
