
using UnityEngine;

public class PlayerController 
{
    PlayerModel playerModel;
    PlayerView playerView;

    public PlayerController(PlayerView playerView, SO_Player soPlayer)
    {
        playerModel= new PlayerModel(soPlayer);
        Object.Instantiate(playerView);
    }
}
