using UnityEngine;

public class Level : MonoBehaviour 
{
    public int experience;
    
    public int ExperienceLevel => experience / 750;
}
