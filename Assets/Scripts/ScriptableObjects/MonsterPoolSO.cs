using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterGroup { All, Dolguns, Buseuls, Bees, Trees, Slimes, MidBoss1, Boss1 }
[CreateAssetMenu(menuName = "Monster/Pools", fileName = "MonsterPool")]
public class MonsterPoolSO : ScriptableObject
{
    public GameObject[] Stage1All;
    public GameObject[] Stage1Dolguns;
    public GameObject[] Stage1Buseuls;
    public GameObject[] Stage1Bees;
    public GameObject[] Stage1Trees;
    public GameObject[] Stage1Slimes;
    public GameObject Stage1MidBoss;
    public GameObject Stage1Boss;
}
