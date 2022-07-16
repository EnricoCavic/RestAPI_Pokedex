using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using SimpleJSON;

public class PokemonRequests : RequestSender
{

    public async Task SpriteRequest(PokemonInfo _info)
    {
        _info.frontDefaultTexture = await TextureWebRequest(_info.frontDefaultURL);        
    }

    public async Task MiniatureRequest(PokemonInfo _info)
    {
        _info.miniatureTexture = await TextureWebRequest(_info.miniatureURL);        
    }

    public async Task FieldGroupRequest(PokemonInfo _info)
    {
        var jsonRequests = new Task<JSONNode>[2];

        jsonRequests[0] = JSONWebRequest(_info.field1URL);
        jsonRequests[1] = JSONWebRequest(_info.field2URL);

        await Task.WhenAll(jsonRequests);

        _info.field1Node = jsonRequests[0].Result;
        _info.field2Node = jsonRequests[1].Result;
    }
}
