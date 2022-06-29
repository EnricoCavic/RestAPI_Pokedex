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
        jsonTask[0] = rq.PkmnRequest(baseURL + pkmURL + rand.ToString());
        jsonTask[1] = rq.SpecieRequest(baseURL + speciesURL + rand.ToString());

        await Task.WhenAll(jsonTask);
        pokemonInfo.generalNode = jsonTask[0].Result;
        pokemonInfo.speciesNode = jsonTask[1].Result;

        var tasks = new Task<Information>[2];
        tasks[0] = rq.SpriteRequest(pokemonInfo.frontDefaultURL);
        tasks[1] = rq.FieldGroupRequest(pokemonInfo.field1URL, pokemonInfo.field2URL);

        await Task.WhenAll(tasks);   
        
        pokemonInfo.frontDefaultTexture = tasks[0].Result.texture;

        pokemonInfo.field1Node = tasks[1].Result.fieldsArray[0];
        pokemonInfo.field2Node = tasks[1].Result.fieldsArray[1];


        onInformationArrived?.Invoke(pokemonInfo);

    }

    public async void GetPokemonMiniature(int _index)
    {
        PokemonInfo pokemonInfo = new PokemonInfo();
        await rq.PkmnRequest(baseURL + pkmURL + _index.ToString());
    }




}

public class Information
{
    public Texture texture;
    public JSONNode[] fieldsArray;
}
