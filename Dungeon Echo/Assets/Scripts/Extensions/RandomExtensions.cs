using EnumNamespace;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Класс отвечающий за рандом
/// </summary>
public static partial class RandomExtensions
{
    private static readonly IDictionary<DropChance,float[]> DictionaryArrayPercent;
    private static readonly System.Random _random;
    static RandomExtensions()
    {
        DictionaryArrayPercent = new Dictionary<DropChance,float[]>();
        _random  = new System.Random();
        InitDictionaryPercents();
    }
    //-----------------Инициализация словаря шансов выподения карты экипировки
    private static void InitDictionaryPercents()
    {
        //-------------проценты выпадения карт при стартовой раздаче
        DictionaryArrayPercent.Add(DropChance.ChanceEqupment,new [] {1f});
        DictionaryArrayPercent.Add(DropChance.ChanceSpell,new [] {0.34f, 0.33f, 0.33f});
        DictionaryArrayPercent.Add(DropChance.ChanceConsumables,new [] {1f});
        DictionaryArrayPercent.Add(DropChance.ChanceArea,new [] {1f});
        //-------------проценты редкости карт
        DictionaryArrayPercent.Add(DropChance.StartСhanceRarity,new [] {0.90f, 0.10f, 0.0f});
        DictionaryArrayPercent.Add(DropChance.ChanceCommonRarity,new [] {1f, 0.0f, 0.0f});
        //-------------проценты выпадения награды
        DictionaryArrayPercent.Add(DropChance.ChanceReward,new [] {0.70f, 0.30f});
        //-------------проценты выпадения события
        DictionaryArrayPercent.Add(DropChance.ChanceEvent,new [] {0.3f, 0.7f, 0.00f});
        //-------------проценты выпадения хорошего/плохого исхода
        DictionaryArrayPercent.Add(DropChance.ChanceOutcome,new [] {0.65f, 0.35f});
    }
    public static int GetRandomElementDictionary(DropChance typeEnum)
    {
        float[] array;
        var isExists = DictionaryArrayPercent.TryGetValue(typeEnum, out array);
        if (isExists == false)
        {
            throw new UnityException("It is unknown GameStage in class RandomExtensions");
        }
        return RandomElementArray(array);
    }
    //-----------------Выбор элемента из диапозона из списка
    public static T Random<T>(List<T> list)
    {
        //var index = _random.Next(list.Count);
        var value = list[UnityEngine.Random.Range(0,list.Count)];
        return value;
    }
    public static int Random(int count)
    {
        return UnityEngine.Random.Range(0,count);
    }
    //-----------------Выбор элементов с разной вероятностью
    private static int RandomElementArray(IList<float> array)
    {
        var total = array.Sum();
        var randomPoint = (float)_random.NextDouble()*total;
        for (var i = 0; i < array.Count; i++)
        {
            if (randomPoint < array[i])
                return i;
            randomPoint -= array[i];
        }
        return array.Count - 1;
    }
    //-----------------Алгоритм Фишера – Йетса
    public static List<T> Shuffle<T>(List<T> list)
    {
        var result = list;
        for (var i = list.Count - 1; i >= 1; i--)
        {
            var j = _random.Next(i + 1);
            var tmp = result[j];
            result[j] = result[i];
            result[i] = tmp;
        }
        return result;
    }
}  
