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

        public GDLevelInfos Level{ get ; private set ; }
        public GDPlayerInfos Player { get; private set; }
        public bool IsInitialized { get { return Reader.IsInitialized; } }

        public MemoryReader Reader { get; set; } = new MemoryReader();

        public bool Initialize()
        {
            return Reader.AttachTo(GDProcessName);
        }

        public void Update()
        {
            if (!Reader.IsInitialized) return;

            var commonAddr = BitConverter.ToInt32(Reader.ReadBytes((int)Reader.MainModuleAddr + baseOffset, 4), 0);
            var levelAddr = BitConverter.ToInt32(Reader.ReadBytes(commonAddr + levelOffset, 4), 0);

            if(levelAddr == NO_LEVEL_LOADED)
            {
                Level = null;
                Player = null;
                return;
            }

            var playerAddr = BitConverter.ToInt32(Reader.ReadBytes(levelAddr + playerOffset, 4), 0);

            var levelMetadata = BitConverter.ToInt32(Reader.ReadBytes(levelAddr + levelMetadataOffset, 4), 0);
            var levelNameLength = BitConverter.ToInt32(Reader.ReadBytes(levelMetadata + nameSizeOffset, 4),0);
            int nameAddr;
            if(levelNameLength > MAX_POINTERLESS_NAME_SIZE)
            {
                nameAddr = BitConverter.ToInt32(Reader.ReadBytes(levelMetadata + nameOffset, 4), 0);
            } else
            {
                nameAddr = levelMetadata+nameOffset;
            }
            var levelName = Reader.ReadString(nameAddr, levelNameLength);


            Level = new GDLevelInfos//TODO find a way to avoid instantiating
            {
                Name = levelName,
                CurrentAttempt = BitConverter.ToInt32(Reader.ReadBytes(levelAddr + attemptsOffset, 4), 0),
                Length = BitConverter.ToSingle(Reader.ReadBytes(levelAddr + levelLengthOffset, 4), 0)
            };

            //TODO
            //Player may have not been instanciated yet, but will be soon
            //This loop ensure the player is always loaded
            //Bad code but for now there doesn't seem to be any major drawback, so it'll stay like that for now
            while(playerAddr == NO_PLAYER_LOADED)
            {
                Thread.Sleep(1);
                playerAddr = BitConverter.ToInt32(Reader.ReadBytes(levelAddr + playerOffset, 4), 0);
            }
            
            Player = new GDPlayerInfos//TODO player pointer might be null at this stage
            {
                XPosition = BitConverter.ToSingle(Reader.ReadBytes(playerAddr + xPositionOffset, 4), 0),
                IsDead = BitConverter.ToBoolean(Reader.ReadBytes(playerAddr + isDeadOffset, 1), 0),
                HasWon = BitConverter.ToBoolean(Reader.ReadBytes(playerAddr + hasWonOffset, 1), 0)
            };
            
        }
    }

    class GDLevelInfos
    {
        public int ID { get; } = 0;//TODO
        public string Name { get; set; }
        //public float StartPosition { get { throw new NotImplementedException(); } } TODO couldn't find how to get it from memory
        public int CurrentAttempt { get; set; }
        public float Length { get; set; }
    }

    class GDPlayerInfos
    {
        public float XPosition { get; set; }
        public bool IsDead { get; set; }
        public bool HasWon { get; set; }
    }
}
