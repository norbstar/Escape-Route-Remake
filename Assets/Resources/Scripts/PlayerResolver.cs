using Tests;
using UnityEngine;

public class PlayerResolver : SingletonMonoBehaviour<PlayerResolver>
{
    [SerializeField] BasePlayer basePlayer;

    public BasePlayer BasePlayer => basePlayer;
}
