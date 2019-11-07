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
        Unit,
        SummonSpell
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
        ChanceOutcome
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
        Cooldown,
    }
    public enum Membership
    {
        Undefined,
        Player,
        Enemy,
        AlliesArea,
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
        GoStageMainMenu,
        GoStageStartGame,
        GoRewardEvent,
        //---------------------end
        GoEndGiveTokensReward,
        GoEndSelectTokenReward,
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
        GoClosePopupReward,
        GoClosePanelPlayers,
        //---------------------add
        GoAddCardInInventory,
        GoSelectTokenReward,
        GoAddTokenReward,
        //---------------------remove
        GoRemoveTokenReward,
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
        //----------------------get
        GoGetPositionActivateCard,
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
        SpawnAllies,
        //---------------------activate
        GoActivateCard,
        ActivateCardPlayer,
        GoDamageEnemy,
        GoActivateTargetPlayer,
        GoDeActivateTargetsPlayer,
        //---------------------finish
        GoFinishStageEvent,
        //---------------------target
        TargetСapture,
        TargetEnemy,
        TargetPlayer,
        TargetingСapture,
        NonTargetСapture,
        NonTargetingСapture,
        TargetingPlayer,
        NonTargetingPlayer,
        TargetingArea,
        NonTargetingArea,
        TargetArea,
        //--------------------use
        GoUseAutoReward,
        GoUseConsumablesOnPlayer,
        GoSummonAllies,
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

    public enum StatusCard
    {
        InHand,
        OnBoard,
    }
    public enum TokenRewardEnum
    {
        Undefined,
        Pass,
        RestoreHealth,
        Bleeding,
    }
    public enum TokenUse
    {
        Auto,
        GivePlayer,
    }
 }