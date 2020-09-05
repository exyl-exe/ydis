using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whydoisuck.MemoryReading
{
    class GDMemoryReader
    {
        const string GDProcessName = "GeometryDash";

        const int baseOffset = 0x3222D0;//From module base address
        const int levelOffset = 0x164;//from base

        const int levelLengthOffset = 0x3B4;//From level
        const int attemptsOffset = 0x4A8;
        const int playerOffset = 0x224;

        const int isDeadOffset = 0x63F;//from player
        const int hasWonOffset = 0x662;
        const int xPositionOffset = 0x67C;

        const int NO_LEVEL_LOADED = 0x0;

        public GDLevel Level{ get ; private set ; }
        public GDPlayer Player { get; private set; }
        public bool IsInitialized { get { return reader.IsInitialized; } }

        private readonly MemoryReader reader = new MemoryReader();

        public bool Initialize()
        {
            return reader.AttachTo(GDProcessName);
        }

        public void Update()
        {
            if (!reader.IsInitialized) return;
            var commonAddr = BitConverter.ToInt32(reader.ReadBytes((int)reader.MainModuleAddr + baseOffset, 4), 0);
            var levelAddr = BitConverter.ToInt32(reader.ReadBytes(commonAddr + levelOffset, 4), 0);

            if(levelAddr == NO_LEVEL_LOADED)
            {
                Level = null;
                Player = null;
                return;
            }

            var playerAddr = BitConverter.ToInt32(reader.ReadBytes(levelAddr + playerOffset, 4), 0);

            Level = new GDLevel//TODO find a way to avoid instantiating
            {
                CurrentAttempt = BitConverter.ToInt32(reader.ReadBytes(levelAddr + attemptsOffset, 4), 0),
                Length = BitConverter.ToSingle(reader.ReadBytes(levelAddr + levelLengthOffset, 4), 0)
            };

            Player = new GDPlayer
            {
                XPosition = BitConverter.ToSingle(reader.ReadBytes(playerAddr + xPositionOffset, 4), 0),
                IsDead = BitConverter.ToBoolean(reader.ReadBytes(playerAddr + isDeadOffset, 1), 0),
                HasWon = BitConverter.ToBoolean(reader.ReadBytes(playerAddr + hasWonOffset, 1), 0)
            };
        }

        public string GetState()//TODO remove
        {
            if(Level == null)
            {
                return "Not in a level";
            } else
            {
                return $"Level ({Level.Length}), attempt {Level.CurrentAttempt}\nPlayer : pos {Player.XPosition}, isDead {Player.IsDead}, hasWon {Player.HasWon}";
            }
        }
    }

    class GDLevel//TODO ID, name, start position
    {
        public int CurrentAttempt { get; set; }
        public float Length { get; set; }
    }

    class GDPlayer
    {
        public float XPosition { get; set; }
        public bool IsDead { get; set; }
        public bool HasWon { get; set; }
    }
}
