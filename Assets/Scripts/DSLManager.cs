using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class Character
{
    public string E_Name, K_Name;
    public int price;
    public bool selected, purchased;

    public Character(string E_Name, string K_Name, int price, bool selected, bool purchased)
    {
        this.E_Name = E_Name;
        this.K_Name = K_Name;
        this.price = price;
        this.selected = selected;
        this.purchased = purchased;
    }
}

public class Ranking
{
    public int score, characterIndex;

    public Ranking(int score, int characterIndex)
    {
        this.score = score;
        this.characterIndex = characterIndex;
    }
}

public class Inform
{
    public int money;
    public bool bgmOn, soundEffectOn, vibrationOn, Retry;

    public Inform(int money, bool bgmOn, bool soundEffectOn, bool vibrationOn, bool Retry)
    {
        this.money = money;
        this.bgmOn = bgmOn;
        this.soundEffectOn = soundEffectOn;
        this.vibrationOn = vibrationOn;
        this.Retry = Retry;
    }
}

public class DSLManager : MonoBehaviour
{
    List<Character> characters = new List<Character>();
    List<Ranking> ranking = new List<Ranking>();
    List<Inform> inform = new List<Inform>();

    private void Awake()
    {
        if(!File.Exists(Application.persistentDataPath + "/Characters.json"))
        {
            characters.Add(new Character("BusinessMan", "회사원", 0, true, true));
            characters.Add(new Character("Rapper", "래퍼", 0, false, false));
            characters.Add(new Character("Secretary", "비서", 0, false, false));
            characters.Add(new Character("Boxer", "복서", 0, false, false));
            characters.Add(new Character("CheerLeader", "치어리더", 0, false, false));
            characters.Add(new Character("Sheriff", "보안관", 0, false, false));
            characters.Add(new Character("Plumber", "배관공", 0, false, false));

            ranking.Add(new Ranking(0, 7));
            ranking.Add(new Ranking(0, 7));
            ranking.Add(new Ranking(0, 7));
            ranking.Add(new Ranking(0, 7));

            inform.Add(new Inform(0, true, true, true, false));

            DataSave();
        }

        DataLoad();
    }

    public void DataSave()
    {
        string jdata_0 = JsonConvert.SerializeObject(characters);
        string jdata_1 = JsonConvert.SerializeObject(ranking);
        string jdata_2 = JsonConvert.SerializeObject(inform);

        byte[] bytes_0 = System.Text.Encoding.UTF8.GetBytes(jdata_0);
        byte[] bytes_1 = System.Text.Encoding.UTF8.GetBytes(jdata_1);
        byte[] bytes_2 = System.Text.Encoding.UTF8.GetBytes(jdata_2);

        string format_0 = System.Convert.ToBase64String(bytes_0);
        string format_1 = System.Convert.ToBase64String(bytes_1);
        string format_2 = System.Convert.ToBase64String(bytes_2);

        File.WriteAllText(Application.persistentDataPath + "/Characters.json", format_0);
        File.WriteAllText(Application.persistentDataPath + "/Ranking.json", format_1);
        File.WriteAllText(Application.persistentDataPath + "/Inform.json", format_2);
    }

    public void DataLoad()
    {
        string jdata_0 = File.ReadAllText(Application.persistentDataPath + "/Characters.json");
        string jdata_1 = File.ReadAllText(Application.persistentDataPath + "/Ranking.json");
        string jdata_2 = File.ReadAllText(Application.persistentDataPath + "/Inform.json");

        byte[] bytes_0 = System.Convert.FromBase64String(jdata_0);
        byte[] bytes_1 = System.Convert.FromBase64String(jdata_1);
        byte[] bytes_2 = System.Convert.FromBase64String(jdata_2);

        string reformat_0 = System.Text.Encoding.UTF8.GetString(bytes_0);
        string reformat_1 = System.Text.Encoding.UTF8.GetString(bytes_1);
        string reformat_2 = System.Text.Encoding.UTF8.GetString(bytes_2);

        characters = JsonConvert.DeserializeObject<List<Character>>(reformat_0);
        ranking = JsonConvert.DeserializeObject<List<Ranking>>(reformat_1);
        inform = JsonConvert.DeserializeObject<List<Inform>>(reformat_2);
    }
}
