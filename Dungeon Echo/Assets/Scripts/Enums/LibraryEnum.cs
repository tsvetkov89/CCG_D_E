namespace EnumNamespace
 {
    public enum CardType
    {
        Undefined,
        GameClass,
        GameArea,
        GameEvent,
        Enemy,
        Equipment,
        Spell,
        Consumables,
        GameUnit
    }
    public enum SubTypeCard
    {
        Undefined,
        GameClass,
        GameArea,
        GameEvent,
        Weapons,
        Armor,
        Decoration,
        AttackingSpell,
        ImprovingSpell,
        CurseSpell,
        Consumables,
        LightEnemy,
        AverageEnemy,
        ComplicatedEnemy,
        Boss,
        Unit
    }
    public enum CardRarity
    {
        Undefined,
        Common,
        Rare,
        Legendary
    }
    public enum EffectCard
    {
        Undefined,
        BlockChance,
        RegenMana,
        Poison,
        Breaking,
        Stamina,
        Mana,
        Health,
        VampirismNum,
        VampirismPer
    }
    public enum GameClass
    {
        Undefined,
        Red,
        Green,
        Blue,
        Purple
    }
    public enum DropChance
    {
        Undefined,
        ChanceEqupment,
        ChanceSpell,
        ChanceConsumables,
        ChanceArea,
        StartСhanceRarity,
        ChanceCommonRarity,
        ChanceReward,
        ChanceEvent,
    }
    public enum StatsEqupment
    {
        Undefined,
        Damage,
        Armor,
        CoolDown
    }
    public enum PriceEqupment
    {
        Undefined,
        Buy,
        Sell,
        Upgrade
    }
    public enum MainAttribute
    {
        Undefined,
        Health,
        Stamina,
        Mana,
        Damage,
    }
    
    public enum AreaEffectCard
    {
        Undefined,
        Single,
        Mass,
        Random
    }
    public enum StatsSpell
    {
        Undefined,
        Duration,
        Damage,
        Periodicdamage,
        Cooldown 
    }
    public enum Membership
    {
        Undefined,
        Player,
        Enemy
    }

    public enum GameEventName
    {
        Undefined,

        //---------------------stage
        GoStageStartNewGame,
        GoStageSelectGameClass,
        GoStageAddCardEquipment,
        GoStageAddCardSpell,
        GoStageAddCardConsumables,
        GoStageAddCardArea,
        GoStageAddCardEvent,
        GoStageAddCardEnemy,
        GoStageRandomEvent,
        GoOpenPanelPlayers,
        GoStageBattle,
        GoNextTurn,
        GoEndTurnPlayer,
        GoEndTurnEnemy,
        GoPlayerTurn,
        GoEnemyAttack,
        GoPlayerAttack,
        GoDeadEnemy,
        GoFinishBattle,
        GoStartEvent,
        //---------------------end
        GoEndRandomEvent,
        //---------------------open
        GoOpenPopupWithDescriptionCard,
        GoOpenPopupNoDescription,
        GoOpenInventory,
        GoOpenUnequipmentSlot,
        GoOpenPlaceInSlot,
        GoOpenGameMenu,
        //---------------------close
        GoClosePopupDescriptionCard,
        GoClosePlaceInSlot,
        GoCloseInventory,
        GoCloseGameMenu,
        //---------------------add
        GoAddCardInInventory,
        //---------------------select
        GoSelectGameClass,
        GoSelectCardInInventory,
        GoSelectCardSpell,
        GoSelectCardConsumables,
        GoSelectCardArea,
        GoSelectCardEvent,
        GoSelectCardEnemy,
        GoSelectedIconPlayer,
        //---------------------set
        GoSetIdSlotInventory,
        GoSetNextStage,
        //---------------------equpment
        GoEquipmentCard,
        //---------------------unequpment
        GoUnequipmentCard,
        //---------------------un
        GoUnPlaceInSlot,
        //---------------------save
        GoSaveGameClass,
        //---------------------create
        GoCreateBarsPlayers,
        //---------------------spawn
        SpawnPlayer,
        SpawnEnemy,
        //---------------------activate
        GoActivateCard,
        ActivateCardPlayer,
        GoDamageEnemy,
        //---------------------finish
        GoFinishStageEvent,
    }

    public enum SortLayer
    {
        Default,
        UiLayer0,
        UiLayer1
    }

    public enum AdventureEvent
    {
        DoubleCrystall,
    }

    public enum NameEvent
    {
        TouchGoldenPart,
        TouchSilverPart,
        Pass
    }
 }