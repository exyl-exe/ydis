﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Whydoisuck.DataSaving;
using Whydoisuck.Model.MemoryReading.GameStateStructures;

namespace Whydoisuck.Model.MemoryReading
{
    /// <summary>
    /// Class <c>GDMemoryReader</c> has functions to read useful values in the game's memory.
    /// </summary>
    public class GDMemoryReader
    {
        // Name of the GD process
        const string GDProcessName = "GeometryDash";

        //Pointer offset to access the game's manager
        const int baseOffset = 0x3222D0;//from module base address
        //Pointer offset to access the currently played level structure
        const int levelOffset = 0x164;//from base
        
        // Offsets to access several useful values
        // about the currently played level.
        // Added to the address of the current level
        const int isRunningOffset = 0x2EC;
        const int isTestmodeOffset = 0x494;
        const int respawnPositionOffset = 0x4A0;
        const int levelLengthOffset = 0x3B4;
        const int attemptsOffset = 0x4A8;
        const int playerOffset = 0x224;
        const int levelMetadataOffset = 0x488;
        const int levelSettingsOffset = 0x22C;

        // Offsets to access several useful values
        // about the metadata of the current level.
        // Added to the address of the metadata structure
        const int nameOffset = 0xFC;
        const int nameLengthOffset = 0x10C;
        const int nameCheckOffset = 0x110;
        const int onlineIDOffset = 0xF8;
        const int originalIDOffset = 0x2D4;
        const int revOffset = 0x1C8;
        const int officialMusicIDOffset = 0x1C0;
        const int musicIDOffset = 0x1C4;

        // Offset to access the level's music starting offset
        // Added to the address of the level settings
        const int musicOffsetOffset = 0xFC;//from level settings

        // Offsets to access several useful values
        // about the player object.
        // Added to the address of the player object.
        const int isDeadOffset = 0x63F;
        const int hasWonOffset = 0x662;
        const int xPositionOffset = 0x67C;

        // Constants to know if/how some structures are initialized
        const int MANAGER_NOT_LOADED = 0x0;
        const int SETTINGS_NOT_LOADED = 0x0;
        const int NO_LEVEL_LOADED = 0x0;
        const int NO_PLAYER_LOADED = 0x0;
        const int EXTENDED_NAME_CHECK_VALUE = 0x1F;//No idea why

        /// <summary>
        /// Boolean true if GD is currently opened. False otherwise.
        /// </summary>
        public bool IsGDOpened { get { return Reader.IsProcessOpened; } }

        //Reader to read values in the memory of the GD process
        private MemoryReader Reader { get; set; } = new MemoryReader();

        /// <summary>
        /// Attempts to attach to the GD process
        /// </summary>
        /// <returns>True if it succeeded. False otherwise.</returns>
        public bool TryAttachToGD()
        {
            return Reader.AttachTo(GDProcessName);
        }

        /// <summary>
        /// Read the current state of the game from the process' memory
        /// </summary>
        /// <returns>
        /// null if the game hasn't finished loading/its process is closed
        /// the state of the game otherwise (<c>GameState</c> object).
        /// Some properties of the returned value might be null depending
        /// on if a level is being played
        /// </returns>
        public GameState GetGameState()
        {
            try
            {
                // Tests if the process is opened, and if the game has loaded
                if (!Reader.IsProcessOpened || Reader.Process.MainModule == null)
                {
                    return null;
                }
            }
            catch (Win32Exception) // Happens sometimes when the game is being launched
            {
                return null;
            }

            var commonAddr = Reader.ReadInt((int)Reader.MainModuleAddr + baseOffset);
            if (commonAddr == MANAGER_NOT_LOADED) return null; // game was launched but has not finished loading

            var currentState = new GameState(null, null, null);
            var levelAddr = Reader.ReadInt(commonAddr + levelOffset);

            if (levelAddr != NO_LEVEL_LOADED) // The user is not currently playing a level
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

        // Gets the useful values about the player object
        private GDPlayer GetPlayerInfo(int playerStructAddr)
        {
            var xPosition = Reader.ReadFloat(playerStructAddr + xPositionOffset);
            var isDead = Reader.ReadBoolean(playerStructAddr + isDeadOffset);
            var hasWon = Reader.ReadBoolean(playerStructAddr + hasWonOffset);
            return new GDPlayer(
                xPosition,
                isDead,
                hasWon);
        }

        // Gets the useful values about the played level
        private GDLoadedLevel GetLoadedLevelInfo(int levelStructAddr)
        {
            var isRunning = Reader.ReadBoolean(levelStructAddr + isRunningOffset);
            var isTestmode = Reader.ReadBoolean(levelStructAddr + isTestmodeOffset);
            var attemptNumber = Reader.ReadInt(levelStructAddr + attemptsOffset);
            var physicalLength = Reader.ReadFloat(levelStructAddr + levelLengthOffset);
            var startPosition = Reader.ReadFloat(levelStructAddr + respawnPositionOffset);
            return new GDLoadedLevel(
                isRunning,
                isTestmode,
                attemptNumber,
                physicalLength,
                startPosition);
        }

        // Gets the useful values about the level's metadata
        private GDLevelMetadata GetLevelInfo(int levelStructAddr)
        {
            var levelMetadataAddr = Reader.ReadInt(levelStructAddr + levelMetadataOffset);
            var levelSettingsAddr = Reader.ReadInt(levelStructAddr + levelSettingsOffset);

            var levelID = Reader.ReadInt(levelMetadataAddr + onlineIDOffset);
            var levelName = GetLevelName(levelMetadataAddr);
            var isOnline = levelID != 0;//lazyness
            var originalID = Reader.ReadInt(levelMetadataAddr + originalIDOffset);
            var isOriginal = originalID == 0;//lazyness v2
            var revision = Reader.ReadInt(levelMetadataAddr + revOffset);
            var musicID = Reader.ReadInt(levelMetadataAddr + musicIDOffset);
            var officialMusicID = Reader.ReadInt(levelMetadataAddr + officialMusicIDOffset);
            var isCustomMusic = musicID != 0;//It's how it's done in the game's code
            var musicOffset = levelSettingsAddr == SETTINGS_NOT_LOADED ? 0 : Reader.ReadFloat(levelSettingsAddr + musicOffsetOffset);
            return new GDLevelMetadata(
                levelID,
                levelName,
                isOnline,
                originalID,
                isOriginal,
                revision,
                isCustomMusic,
                musicID,
                officialMusicID,
                musicOffset);
        }

        // Gets the name of the level
        // Overly complicated because its stored differently depending
        // on the length of the name.
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