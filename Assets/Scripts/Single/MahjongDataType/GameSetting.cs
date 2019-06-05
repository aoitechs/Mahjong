using System;
using UnityEngine;
using Utils;

namespace Single.MahjongDataType
{
    [Serializable]
    public class GameSetting
    {
        public GameMode GameMode;
        public GamePlayers GamePlayers;
        public RoundCount RoundCount;
        public MinimumFanConstraintType MinimumFanConstraintType;
        public PointsToGameEnd PointsToGameEnd;
        public bool GameEndsWhenAllLastTop;
        public bool AllowDiscardSameAfterOpen;
        public bool AllowRichiWhenPointsLow;
        public bool AllowRichiWhenNotReady;
        public bool AllowChows;
        public bool AllowPongs;
        public int InitialPoints;
        public int FirstPlacePoints;
        public int RichiMortgagePoints;
        public int ExtraRoundBonusPerPlayer;
        public int NotReadyPunishPerPlayer;
        public int FalseRichiPunishPerPlayer;
        public bool Allow3RongDraw;
        public bool Allow4RichiDraw;
        public bool Allow4KongDraw;
        public bool Allow4WindDraw;
        public bool Allow9OrphanDraw;
        public bool AllowHint; // todo -- add setting panel entry
        public int BaseTurnTime = 5;
        public int BonusTurnTime = 20;
        public int DiceMin = 2;
        public int DiceMax = 12;
        public int MountainReservedTiles = 14;
        public int LingshangTilesCount = 4;
        public int InitialDora = 1;
        public int MaxDora = 5;

        public Tile[] redTiles = new Tile[] {
            new Tile(Suit.M, 5, true),
            new Tile(Suit.P, 5, true),
            new Tile(Suit.S, 5, true)
        };

        public Tile[] GetAllTiles()
        {
            switch (GamePlayers)
            {
                case GamePlayers.Two:
                    return MahjongConstants.TwoPlayerTiles.ToArray();
                case GamePlayers.Three:
                    return MahjongConstants.ThreePlayerTiles.ToArray();
                case GamePlayers.Four:
                    return MahjongConstants.FullTiles.ToArray();
                default:
                    Debug.LogError($"This should not happen, GamePlayers: {GamePlayers}");
                    return null;
            }
        }
        public int MaxPlayer => GetPlayerCount(GamePlayers);

        public static int GetPlayerCount(GamePlayers playerSetting)
        {
            switch (playerSetting)
            {
                case GamePlayers.Two:
                    return 2;
                case GamePlayers.Three:
                    return 3;
                case GamePlayers.Four:
                    return 4;
                default:
                    Debug.LogError($"Unknown GamePlayers option: {playerSetting}");
                    return 4;
            }
        }

        public bool CheckConstraint(PointInfo point)
        {
            int baseFan = point.FanWithoutDora;
            int fan = point.TotalFan;
            int basePoint = point.BasePoint;
            switch (MinimumFanConstraintType)
            {
                case MinimumFanConstraintType.One:
                    return baseFan >= 1;
                case MinimumFanConstraintType.Two:
                    return baseFan >= 1 && fan >= 2;
                case MinimumFanConstraintType.Three:
                    return baseFan >= 1 && fan >= 3;
                case MinimumFanConstraintType.Four:
                    return baseFan >= 1 && fan >= 4;
                case MinimumFanConstraintType.Mangan:
                    return baseFan >= 1 && basePoint >= MahjongConstants.Mangan;
                case MinimumFanConstraintType.Haneman:
                    return baseFan >= 1 && basePoint >= MahjongConstants.Haneman;
                case MinimumFanConstraintType.Baiman:
                    return baseFan >= 1 && basePoint >= MahjongConstants.Baiman;
                case MinimumFanConstraintType.Yakuman:
                    return baseFan >= 1 && basePoint >= MahjongConstants.Yakuman;
                default:
                    Debug.LogError($"Unknown type {MinimumFanConstraintType}");
                    return false;
            }
        }

        public int GetMultiplier(bool isDealer, int totalPlayers)
        {
            return isDealer ? 6 : 4; // this is for 4-player mahjong -- todo
        }

        public bool IsAllLast(int oyaIndex, int field, int totalPlayers)
        {
            return (oyaIndex == totalPlayers - 1 && field == FieldThreshold - 1) || field >= FieldThreshold;
        }

        public bool GameForceEnd(int oyaIndex, int field, int totalPlayers)
        {
            return oyaIndex == totalPlayers - 1 && field == MaxField - 1;
        }

        private int FieldThreshold
        {
            get
            {
                switch (RoundCount)
                {
                    case RoundCount.E:
                        return 1;
                    case RoundCount.ES:
                        return 2;
                    case RoundCount.FULL:
                        return 4;
                    default:
                        Debug.LogError($"Unknown type {RoundCount}");
                        return 2;
                }
            }
        }

        private int MaxField
        {
            get
            {
                switch (RoundCount)
                {
                    case RoundCount.E:
                        return 2;
                    case RoundCount.ES:
                        return 3;
                    case RoundCount.FULL:
                        return 4;
                    default:
                        Debug.LogError($"Unknown type {RoundCount}");
                        return 2;
                }
            }
        }

        public override string ToString()
        {
            return this.ToJson(true);
        }
    }

    public enum GameMode
    {
        Normal, QTJ
    }

    public enum GamePlayers
    {
        Two, Three, Four
    }

    public enum RoundCount
    {
        E, ES, FULL
    }

    public enum MinimumFanConstraintType
    {
        One, Two, Three, Four, Mangan, Haneman, Baiman, Yakuman
    }

    public enum PointsToGameEnd
    {
        Negative, Zero, Never
    }
}