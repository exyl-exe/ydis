using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Whydoisuck.DataSaving;

namespace Whydoisuck.MemoryReading
{
    class GDMemoryReader
    {
        const string GDProcessName = "GeometryDash";

        const int baseOffset = 0x3222D0;//from module base address
        const int levelOffset = 0x164;//from base

        const int isRunningOffset = 0x2EC;//from level
        const int isTestmodeOffset = 0x494;
        const int respawnPositionOffset = 0x4A0;
        const int levelLengthOffset = 0x3B4;
        const int attemptsOffset = 0x4A8;
        const int playerOffset = 0x224;
        const int levelMetadataOffset = 0x488;
        const int levelSettingsOffset = 0x22C;

        const int nameOffset = 0xFC;//from level metadata
        const int nameLengthOffset = 0x10C;
        const int nameCheckOffset = 0x110;
        const int onlineIDOffset = 0xF8;
        const int originalIDOffset = 0x2D4;
        const int revOffset = 0x1C8;
        const int officialMusicIDOffset = 0x1C0;
        const int musicIDOffset = 0x1C4;

        const int musicOffsetOffset = 0xFC;//from level settings

        const int isDeadOffset = 0x63F;//from player
        const int hasWonOffset = 0x662;
        const int xPositionOffset = 0x67C;

        const int MANAGER_NOT_LOADED = 0x0;
        const int SETTINGS_NOT_LOADED = 0x0;
        const int NO_LEVEL_LOADED = 0x0;
        const int NO_PLAYER_LOADED = 0x0;
        const int EXTENDED_NAME_CHECK_VALUE = 0x1F;//black magic imo

        public bool IsGDOpened { get { return Reader.IsProcessOpened; } }

        public MemoryReader Reader { get; set; } = new MemoryReader();

        public bool TryAttachToGD()
        {
            return Reader.AttachTo(GDProcessName);
        }

        public GameState GetGameState()
        {
            bool GDClosed = false;
            try
            {
                GDClosed = !Reader.IsProcessOpened || Reader.Process.MainModule == null;
            }
            catch (Win32Exception)
            {
                return null;
            }
            if (GDClosed) return null;


            var commonAddr = Reader.ReadInt((int)Reader.MainModuleAddr + baseOffset);
            if (commonAddr == MANAGER_NOT_LOADED) return null;//game was launched but has not finished loading

            var currentState = new GameState() { LevelMetadata = null, PlayerObject = null };
            var levelAddr = Reader.ReadInt(commonAddr + levelOffset);

            if (levelAddr != NO_LEVEL_LOADED)
            {
                currentState.LevelMetadata = GetLevelInfo(levelAddr);
                currentState.LoadedLevel = GetLoadedLevelInfo(levelAddr);
                var playerAddr = Reader.ReadInt(levelAddr + playerOffset);
                if (playerAddr != NO_PLAYER_LOADED)
                {
                    currentState.PlayerObject = GetPlayerInfo(playerAddr);
                }
            }
            return currentState;
        }

        private GDPlayer GetPlayerInfo(int playerStructAddr)
        {
            return new GDPlayer
            {
                XPosition = Reader.ReadFloat(playerStructAddr + xPositionOffset),
                IsDead = Reader.ReadBoolean(playerStructAddr + isDeadOffset),
                HasWon = Reader.ReadBoolean(playerStructAddr + hasWonOffset)
            };
        }

        private GDLoadedLevel GetLoadedLevelInfo(int levelStructAddr)
        {
            return new GDLoadedLevel
            {
                IsRunning = Reader.ReadBoolean(levelStructAddr + isRunningOffset),
                IsTestmode = Reader.ReadBoolean(levelStructAddr + isTestmodeOffset),
                AttemptNumber = Reader.ReadInt(levelStructAddr + attemptsOffset),
                PhysicalLength = Reader.ReadFloat(levelStructAddr + levelLengthOffset),
                StartPosition = Reader.ReadFloat(levelStructAddr + respawnPositionOffset)
            };
        }


        private GDLevelMetadata GetLevelInfo(int levelStructAddr)
        {
            var levelMetadataAddr = Reader.ReadInt(levelStructAddr + levelMetadataOffset);
            var levelSettingsAddr = Reader.ReadInt(levelStructAddr + levelSettingsOffset);
            var levelName = GetLevelName(levelMetadataAddr);
            var levelID = Reader.ReadInt(levelMetadataAddr + onlineIDOffset);
            var originalID = Reader.ReadInt(levelMetadataAddr + originalIDOffset);
            var musicID = Reader.ReadInt(levelMetadataAddr + musicIDOffset);
            var musicOffset = levelSettingsAddr == SETTINGS_NOT_LOADED ? 0 : Reader.ReadFloat(levelSettingsAddr + musicOffsetOffset);
            return new GDLevelMetadata
            {
                ID = levelID,
                IsOnline = (levelID != 0),//lazyness
                OriginalID = originalID,
                IsOriginal = (originalID == 0),//lazyness v2
                Name = levelName,
                Revision = Reader.ReadInt(levelMetadataAddr + revOffset),
                MusicID = musicID,
                OfficialMusicID = Reader.ReadInt(levelMetadataAddr + officialMusicIDOffset),
                IsCustomMusic = (musicID != 0),//It's how it's done in the game's code
                MusicOffset = musicOffset
            };
        }

        private string GetLevelName(int levelMetadata)
        {
            var extendedNameByte = Reader.ReadInt(levelMetadata + nameCheckOffset);
            var nameLength = Reader.ReadInt(levelMetadata + nameLengthOffset);
            int nameAddr;
            if (extendedNameByte == EXTENDED_NAME_CHECK_VALUE)
            {
                nameAddr = Reader.ReadInt(levelMetadata + nameOffset);
            }
            else
            {
                nameAddr = levelMetadata + nameOffset;
            }
            return Reader.ReadString(nameAddr, nameLength);
        }
    }
}
