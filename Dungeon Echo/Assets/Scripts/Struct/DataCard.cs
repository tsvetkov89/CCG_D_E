using System.Collections.Generic;
using EnumNamespace;
using UnityEngine;
using UnityEngine.UI;

public struct DataCard
{
    public CardType TypeCard;
    public GameClass GameClass;
    public SubTypeCard TypeSubCard;
    public CardRarity Rarity;
    public Membership Application;
    public string NameCard;
    public string DisplayNameCard;
    public string FullDescription;
    public Sprite Art;
    public Sprite ShirtCard;
    public Sprite ShirtMain;
    public List<CardsGameClass.StatsGameClass> Attribute;
    public List<CardUnit.StatsUnit> AttributeUnit;
    public List<CardSpell.StatsSpell> AttributeSpell;
    public List<string> SpawnUnits;
    public CardSpell.CostAttribute CostAttribute;
    public List<CardConsumables.EffectEqupment> Effect;
}