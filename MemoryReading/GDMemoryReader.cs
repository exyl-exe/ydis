using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Whydoisuck.MemoryReading
{
    class GDMemoryReader
    {
        const string GDProcessName = "GeometryDash";

        const int EXTENDED_NAME_CHECK_VALUE = 0x1F;//black magic imo

        const int baseOffset = 0x3222D0;//from module base address
        const int levelOffset = 0x164;//from base

        const int levelLengthOffset = 0x3B4;//from level
        const int attemptsOffset = 0x4A8;
        const int playerOffset = 0x224;
        const int levelMetadataOffset = 0x488;

        const int nameOffset = 0xFC;//from level metadata
        const int nameLengthOffset = 0x10C;
        const int nameCheckOffset = 0x110;
        const int onlineIDOffset = 0xF8;
        const int originalIDOffset = 0x2D4;
        const int revOffset = 0x1C8;
        const int objectCountOffset = 0x1D8;
        const int officialMusicIDOffset = 0x1C0;
        const int musicIDOffset = 0x1C4;

        const int isDeadOffset = 0x63F;//from player
        const int hasWonOffset = 0x662;
        const int xPositionOffset = 0x67C;

        const int NO_LEVEL_LOADED = 0x0;
        const int NO_PLAYER_LOADED = 0x0;

        public bool IsGDOpened { get { return Reader.IsProcessOpened; } }

        public MemoryReader Reader { get; set; } = new MemoryReader();

        public bool TryAttachToGD()
        {
            return Reader.AttachTo(GDProcessName);
        }

        public GameState GetGameState()
        {
            if (!Reader.IsProcessOpened || Reader.Process.MainModule == null) return null;

            var currentState = new GameState() { PlayedLevel = null, PlayerObject = null };

            var commonAddr = BitConverter.ToInt32(Reader.ReadBytes((int)Reader.MainModuleAddr + baseOffset, 4), 0);
            var levelAddr = BitConverter.ToInt32(Reader.ReadBytes(commonAddr + levelOffset, 4), 0);

            if(levelAddr != NO_LEVEL_LOADED)
            {
                currentState.PlayedLevel = GetLevelInfos(levelAddr);
                var playerAddr = BitConverter.ToInt32(Reader.ReadBytes(levelAddr + playerOffset, 4), 0);
                if (playerAddr != NO_PLAYER_LOADED)
                {
                   currentState.PlayerObject = GetPlayerInfos(playerAddr);
                }
            }
            return currentState;     
        }

        private GDPlayerInfos GetPlayerInfos(int playerStructAddr)
        {
            return new GDPlayerInfos
            {
                XPosition = BitConverter.ToSingle(Reader.ReadBytes(playerStructAddr + xPositionOffset, 4), 0),
                IsDead = BitConverter.ToBoolean(Reader.ReadBytes(playerStructAddr + isDeadOffset, 1), 0),
                HasWon = BitConverter.ToBoolean(Reader.ReadBytes(playerStructAddr + hasWonOffset, 1), 0)
            };
        }

        private GDLoadedLevelInfos GetLevelInfos(int levelStructAddr)
        {
            var levelMetadataAddr = BitConverter.ToInt32(Reader.ReadBytes(levelStructAddr + levelMetadataOffset, 4), 0);
            var levelName = GetLevelName(levelMetadataAddr);
            var levelID = BitConverter.ToInt32(Reader.ReadBytes(levelMetadataAddr + onlineIDOffset, 4), 0);
            var originalID = BitConverter.ToInt32(Reader.ReadBytes(levelMetadataAddr + originalIDOffset, 4), 0);
            var musicID = BitConverter.ToInt32(Reader.ReadBytes(levelMetadataAddr + musicIDOffset, 4), 0);
            return new GDLoadedLevelInfos
            {
                ID = levelID,
                IsOnline = (levelID != 0),//lazyness
                OriginalID = originalID,
                IsOriginal = (originalID!=0),//lazyness v2
                Name = levelName,
                Revision = BitConverter.ToInt32(Reader.ReadBytes(levelMetadataAddr + revOffset, 4), 0),
                ObjectCount = BitConverter.ToInt32(Reader.ReadBytes(levelMetadataAddr + objectCountOffset, 4), 0),
                MusicID = musicID,
                OfficialMusicID = BitConverter.ToInt32(Reader.ReadBytes(levelMetadataAddr + officialMusicIDOffset, 4), 0),
                IsCustomMusic = (musicID != 0),//It's how it's done in the game's code
                AttemptNumber = BitConverter.ToInt32(Reader.ReadBytes(levelStructAddr + attemptsOffset, 4), 0),
                Length = BitConverter.ToSingle(Reader.ReadBytes(levelStructAddr + levelLengthOffset, 4), 0)
            };
        }

        private string GetLevelName(int levelMetadata)
        {
            var extendedNameByte = BitConverter.ToInt32(Reader.ReadBytes(levelMetadata + nameCheckOffset, 4), 0);
            var nameLength = BitConverter.ToInt32(Reader.ReadBytes(levelMetadata + nameLengthOffset, 4), 0);
            int nameAddr;
            if (extendedNameByte == EXTENDED_NAME_CHECK_VALUE)
            {
                nameAddr = BitConverter.ToInt32(Reader.ReadBytes(levelMetadata + nameOffset, 4), 0);
            }
            else
            {
                nameAddr = levelMetadata + nameOffset;
            }
            return Reader.ReadString(nameAddr, nameLength);
        }
    }
}
