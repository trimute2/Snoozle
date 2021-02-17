using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Mainly controls time slowing
public class BattleManager : MonoBehaviour
{
    public float timeMultiplier = 1f; //1 = normal time, 0.5 = half-speed, etc. Should be set by player action.
	public int numhands = 2;

	public void removeHand()
	{
		numhands -= 1;
		if (numhands==0)
		{
			//victory here
			GameStats.Instance.EndGame(true);
		}
	}
}
