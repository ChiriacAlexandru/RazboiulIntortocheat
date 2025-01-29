using System.Collections.Generic;
using UnityEngine;

public class ArmyManager : MonoBehaviour
{
    public List<Soldier> soldiers = new List<Soldier>(); 

    public void AddSoldier(Soldier newSoldier)
    {
        soldiers.Add(newSoldier);
        Debug.Log("Soldier added to army. Total soldiers: " + soldiers.Count);
    }

    public void RemoveSoldier(Soldier soldier)
    {
        soldiers.Remove(soldier);
        Debug.Log("Soldier removed from army. Total soldiers: " + soldiers.Count);
    }
}
