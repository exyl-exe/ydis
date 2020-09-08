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

        const byte MAX_POINTERLESS_NAME_SIZE = 15;

        const int baseOffset = 0x3222D0;//from module base address
        const int levelOffset = 0x164;//from base

        const int levelLengthOffset = 0x3B4;//from level
        const int attemptsOffset = 0x4A8;
        const int playerOffset = 0x224;
        const int levelMetadataOffset = 0x488;

        const int nameOffset = 0xFC;//from level metadata
        const int nameSizeOffset = 0x10C;
        const int onlineID = 0xF8;
        const int originalID = 0x2D4;
        const int revOffset = 0x1C8;

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
            if (!Reader.IsProcessOpened) return null;

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
            var levelName = GetLevelName(levelStructAddr);
            return new GDLoadedLevelInfos
            {
                Name = levelName,
                AttemptNumber = BitConverter.ToInt32(Reader.ReadBytes(levelStructAddr + attemptsOffset, 4), 0),
                Length = BitConverter.ToSingle(Reader.ReadBytes(levelStructAddr + levelLengthOffset, 4), 0)
            };
        }

        private string GetLevelName(int levelStructAddr)
        {
            var levelMetadata = BitConverter.ToInt32(Reader.ReadBytes(levelStructAddr + levelMetadataOffset, 4), 0);
            var levelNameLength = BitConverter.ToInt32(Reader.ReadBytes(levelMetadata + nameSizeOffset, 4), 0);
            int nameAddr;
            if (levelNameLength > MAX_POINTERLESS_NAME_SIZE)
            {
                nameAddr = BitConverter.ToInt32(Reader.ReadBytes(levelMetadata + nameOffset, 4), 0);
            }
            else
            {
                nameAddr = levelMetadata + nameOffset;
            }
            return Reader.ReadString(nameAddr, levelNameLength);
        }
    }
}
