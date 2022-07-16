using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;
using SimpleJSON;

public class PokemonNetManager : MonoBehaviour
{

    public string baseURL; 
    public string pkmURL;
    public string speciesURL;

    public UnityEvent<PokemonInfo> onInformationArrived;
    public UnityEvent<PokemonInfo> onMiniatureArrived;

    PokemonRequests rq;


    private void Awake() 
    {
        rq = GetComponent<PokemonRequests>();    
    }

    public async void GetPokemonInformation(int _index)
    {
        PokemonInfo pokemonInfo = new PokemonInfo();
        int rand = Random.Range(1, 808);

        var jsonTask = new Task<JSONNode>[2];
        jsonTask[0] = rq.JSONWebRequest(baseURL + pkmURL + rand.ToString());
        jsonTask[1] = rq.JSONWebRequest(baseURL + speciesURL + rand.ToString());

        await Task.WhenAll(jsonTask);
        pokemonInfo.generalNode = jsonTask[0].Result;
        pokemonInfo.speciesNode = jsonTask[1].Result;

        var tasks = new Task[2];
        tasks[0] = rq.SpriteRequest(pokemonInfo);
        tasks[1] = rq.FieldGroupRequest(pokemonInfo);

        await Task.WhenAll(tasks);          

        onInformationArrived?.Invoke(pokemonInfo);
    }

    public async void GetPokemonMiniature(int _index)
    {
        PokemonInfo pokemonInfo = new PokemonInfo();
        pokemonInfo.generalNode = await rq.JSONWebRequest(baseURL + pkmURL + _index.ToString());


        onMiniatureArrived?.Invoke(pokemonInfo);
    }




}

public struct Information
{
    public Texture texture;
    public JSONNode[] fieldsArray;
}
