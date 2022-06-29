using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;
using SimpleJSON;

public class PokemonNetManager : NetworkingManager
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
        pokemonInfo.generalNode = await JSONWebRequest(dataURL);  
    }

    async Task SpecieRequest(int _index)
    {
        string dataURL = baseURL + speciesURL + _index.ToString();
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
}
