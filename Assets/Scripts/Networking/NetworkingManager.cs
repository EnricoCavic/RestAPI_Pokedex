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
        tasks[0] = PkmnRequest(343);
        tasks[1] = SpecieRequest(343);

        await Task.WhenAll(tasks);

        onInformationArrived?.Invoke(pokemonInfo);

    }

    async Task PkmnRequest(int _index)
    {
        string dataURL = baseURL + pkmURL + _index.ToString();
        pokemonInfo.general = await JSONWebRequest(dataURL);  

        string imgURL = pokemonInfo.frontDefaultURL;
        pokemonInfo.frontDefaultTexture = await TextureWebRequest(imgURL);

    }

    async Task SpecieRequest(int _index)
    {
        string dataURL = baseURL + speciesURL + _index.ToString();
        pokemonInfo.species = await JSONWebRequest(dataURL);
    }


    async Task<JSONNode> JSONWebRequest(string dataURL)
    {
        UnityWebRequest request = UnityWebRequest.Get(dataURL);
        await WebRequest(request);

        return JSON.Parse(request.downloadHandler.text);            
    }

    async Task<Texture> TextureWebRequest(string dataURL)
    {
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
    public string frontDefaultURL => general["sprites"]["front_default"];
    public Texture frontDefaultTexture;
}
