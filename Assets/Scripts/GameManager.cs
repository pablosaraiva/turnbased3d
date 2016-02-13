using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;

public class GameManager : MonoBehaviour {

    public AICharacterControl[] aiCharacters;
    private AICharacterControl currentPlayer;

    void FixedUpdate()
    {
        if (currentPlayer == null || currentPlayer.isPlaying == false)
        {
            foreach (AICharacterControl player in aiCharacters)
            {
                player.currentCooldown--;
                if (player.currentCooldown == 0)
                {
                    currentPlayer = player;
                    player.beginTurn();
                    return;
                }
            }
        }
    }
}
