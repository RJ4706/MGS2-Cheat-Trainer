﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGS2_MC
{
    internal class MGS2Constants
    {
        /*useful information from @ANTIBigBoss irt GoG port CT:
         * 
______________________________
Values to make some cheats work
----------------------------------------------------
Regular cheats
  - Walk through walls: 39 = on, 36=off
  - Enable Stealth: 64 = invisible :not from cameras ='(]
 
Weapon values
 - If you want to disable weapons: set value to -1 or 65535
 - this also means for items.
 - Whitesnoop ~
Alert mode = Put 1
Evasion = just lock for infinite evation
Caution = 5000 or whatever number to get Caution
------
Knockout (normal) takes 9 punches
         * 
         * !!!!!!!!!!! Unfortunately, it seems like the offsets in the GoG version are NOT translating to the MC version :( !!!!!!!
         * (Statics we can manipulate) = (type) -- (original offset in hex == original offset in decimal)
         * 
         * ------------
         * "Main" stuff
         * ------------
         * MaxLife = 2 bytes
         * AmmoInClip = 4 bytes -- 618B2C == 6392620
         * FPV State = array bytes -- 9C18C == 639372
         * FPV = byte -- 618B03 == 6392579
         * CurrentItem = 2 bytes -- D8AEC6 == 14200518
         * CurrentLevel = 7 char string -- D8ADEC == 14200300
         * CurrentWeapon = 2 bytes -- D8AEC4 == 14200516
         * Life = 2 bytes -- 618BD0 == 6392784
         * LifeText = 15 char string
         * End if found = 1 byte
         * Walk through walls = 1 byte
         * Walk through walls(soft) = 1 byte
         * Set character at load = 7 char string -- D8C374 == 14205812
         * VR end(34)?? = 4 bytes 
         * Set start point ROT = 2 bytes -- D8FE28 == 14220840
         * Set start point X = 2 bytes -- D8FE30 == 14220848
         * Set start point Y = 2 bytes -- D8FE34 == 14220852
         * Set start point Z = 2 bytes -- D8FE38 == 14220856
         * Load level = 16 char string -- D8C384 == 14205828
         * Previous level = 7 char string -- 664FC4 == 6705092
         * Difficulty = 1 byte -- D8C368 == 14205800
         * Difficulty(RO) = 1 byte -- D8ADD0 == 14200272
         * Screen visible(codec eg?) = 1 byte -- 9B7044 == 10186820
         * X(?) = float -- 6164E0 == 6382816
         * Y(?) = float -- 6164E4 == 6382820
         * Z(?) = float -- 6164E8 == 6382824
         * LocationX(?) = byte? -- 4A9910 == 4888848
         * LocationY(?) = 2 bytes -- 4A990B == 4888843
         * 
         * ----------------
         * VR mission stuff
         * ----------------
         * Bomb disposal = 2 bytes -- B60CFC == 11930876
         * Enemies = 2 bytes -- B6DE20 == 11984416
         * Score = 4 bytes -- B60C20 == 11930656
         * Targets = 2 bytes -- B60C04 == 11930628
         * Time = 2 bytes -- B60CF8 == 11930872
         * 
         * ----------------
         * Alert mode stuff
         * ----------------
         * Caution = 4 bytes -- 6160C8 == 6381768
         * Current Mode = 1 byte -- D8AEDA == 14200538
         * 
         * ------------
         * Random stuff
         * ------------
         * Character luminance = float -- 5FE990 == 6285712
         * Flip screen = float -- 5FE2F4 == 6284020
         * Hud horizontal placement = float -- 5FE610 == 6284816
         * Hud vertical placement = float -- 5FE614 == 6284820
         * Function starter? = 4 bytes -- B60A0C == 11930124
         * FOV = float -- 5FE1C8 == 6283720
         * Horizontal Stretch = float -- 5FE1B4 == 6283700
         * Vertical stretch = float -- 5FE1A0 == 6283680
         * Screen Y pos = float -- 5FD704 == 6280964
         */


        /* 12/10: Interesting CE learnings -
         * 
         * Certain game stats are tracked GLOBALLY and reset on launch(kill count, shot count, holdups, choke outs, prolly more)
         * It looks like the memory accesses these counts AT LEAST once on game load, once on screen load and twice on gameplay
         * 
         * 
         * 
         * 12/23: More CE investigations - 
         * (all of these findings are based off of a NG, Hard file. need to confirm it is consistent across difficulties/NG state
         * Pullup count is stored -90 bytes from dynamic anchor
         * SnakeHasCold is stored -128 bytes: cannot be changed after snake has already used cold meds, can change before
         * Currently equipped weapon is -68 bytes
         * Currently equipped item is -130 bytes
         * Something related to sneezing is at -108(2bytes). Whenever snake sneezes, it changes to 2D 00, then changes to another value. maybe time for next sneeze?
         * -154 and -146 seem to be related to player position? unsure.
         * -134 seems to represent current stance(00 is standing, 01 is crouching, 02 is prone: not quite sure how to leverage these yet)
         * 
         * Things that might be interesting to find/mess around with:
         * 
         * - Can we change max health? On hard its set to 75, but i can't see _what_ is changing it
         * - Can we manually modify boss health?
         * - Are there any AOBs we can use to find static strings/values?
         */

        #region True Constants
        public const string PROCESS_NAME = "METAL GEAR SOLID2";
        #endregion

        #region Offset Finders
        //if the region is dynamic(i.e. PlayerOffsetAoB), it will change on area load. The others will only (possibly) change with game updates
        #region Dynamic AoBs
        //these commented offsets are old possible anchors i found that weren't consistent/usable
        //public static byte[] PlayerOffsetBytes = new byte[6] { 104, 01, 100, 00, 100, 00 }; // the CURRENT player offset will be the second to last one in memory.
        //public static IntPtr PlayerOffsetPtr = (IntPtr)0x680164006400;
        //public static byte[] PlayerOffsetBytes = new byte[] { 82, 82, 74, 41, 37, 148, 82, 74, 41, 145, 66, 170, 148, 82, 74, 41, 1, 132, 18, 80, 74, 165, 144, 145, 145, 145, 82, 162, 164, 148, 145, 82, 74, 41, 165, 148, 82, 74, 41, 73, 72, 33 };
        //public static IntPtr PlayerOffsetPtr = (IntPtr)0x52524A292594524A299142AA94524A29018412504AA59092929252A2A49492524A29A594524A29494821;
        //public static byte[] OldPlayerOffsetBytes = new byte[] { 00, 00, 00, 00, 01, 00, 46, 00 };
        public static byte[] PlayerInfoFinderAoB = new byte[30] { 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 01, 00 }; //TODO: determine if this breaks when we have m9 max ammo >255
        #endregion
        #region Static AoBs
        public static byte[] MenuNamesFinderAoB = new byte[5] { 0x6D, 0x61, 0x70, 0x2E, 0x63 }; //TODO: prove this is valid
        public static byte[] DifficultyAndAreaNamesAoB = new byte[3] { 0x2F, 0x44, 0x2A }; //TODO: prove this is valid
        public static byte[] LifeAndGripNamesAoB = new byte[8] { 0x72, 0x61, 0x69, 0x64, 0x65, 0x6E, 0x2E, 0x63 }; //TODO: prove this is valid
        public static byte[] RayNamesAoB = new byte[10] { 0x6D, 0x69, 0x6E, 0x69, 0x5F, 0x73, 0x63, 0x6E, 0x2E, 0x63 }; //TODO: prove this is valid
        public static byte[] RationMedsBandagePentazeminDescriptionsAoB = new byte[17] { 0xA4, 0xE3, 0x81, 0xAF, 0xE3, 0x81, 0x9A, 0xE3, 0x81, 0xA0, 0xEE, 0x80, 0x80, 0xE3, 0x80, 0x82, 0x0A }; //TODO: prove this is valid
                                                                                                                                                                                                 //weapon & item descriptions dispersed through out. seems to start around +00613CCB or so in the memory print?
        #endregion
        #endregion

        #region Offsets
        #region Value offsets
        #region Calculated From PlayerInfo
        public static readonly MemoryBytes BASE_WEAPON = new MemoryBytes(-42, 0); //if a "new" playerOffsetBytes is chosen, only need to update this value and the item offset will update.
        public static readonly MemoryBytes BASE_ITEM = new MemoryBytes(BASE_WEAPON.OffsetStart + 144, BASE_WEAPON.OffsetStart + 144 + 80);
        public static readonly MemoryBytes SHOT_COUNT = new MemoryBytes(-96); //TODO: prove this is valid
        public static readonly MemoryBytes HOLD_UP_COUNT = new MemoryBytes(5108); //TODO: prove this is valid, prolly isnt tbqh
        public static readonly MemoryBytes PULL_UP_COUNT = new MemoryBytes(-90); //TODO: prove this is valid 
        public static readonly MemoryBytes PLAYER_COLD = new MemoryBytes(-128); //TODO: prove this is valid
        public static readonly MemoryBytes CURRENT_EQUIPPED_ITEM = new MemoryBytes(-130); //TODO: prove this is valid
        public static readonly MemoryBytes CURRENT_EQUIPPED_WEAPON = new MemoryBytes(-68); //TODO: prove this is valid
        public static readonly MemoryBytes PLAYER_STANCE = new MemoryBytes(-134); //TODO: prove this is valid
        public static readonly MemoryBytes PLAYER_SNEEZING = new MemoryBytes(-108, -107); //TODO: prove this is valid
        #endregion

        #region Calculated from Unknown Finder AoBs
        public const int TIMES_FOUND_GAME_LAUNCH_OFFSET = 0x17B786C; //TODO: need to get an OffsetFinderAoB for this and perform offset calculations
        public const int HOLD_UPS_GAME_LAUNCH_OFFSET = 0x1673DB0; //TODO: need to get an OffsetFinderAoB for this and perform offset calculations
        public const int CHOKE_OUTS_GAME_LAUNCH_OFFSET = 0x1673DBC; //TODO: need to get an OffsetFinderAoB for this and perform offset calculations

        //TODO: add more of the game stats here


        //These values are PRESENTLY unknown
        //internal const int HealthPointerOffset = 0x00AE49D8; 
        //internal const int CurrentHealthOffset = 0x684;
        //internal const int MaxHealthOffset = 0x686;
        //internal const int StaminaOffset = 0xA4A;
        //internal static IntPtr HudOffset = (IntPtr)0xADB40F;
        //internal static IntPtr CamOffset = (IntPtr)0xAE3B37;
        //internal static IntPtr AlertStatusOffset = (IntPtr)0x1D9C3D8;
        #endregion
        #endregion

        #region String offsets
        #region Calculated From LifeAndGrip
        public static readonly MemoryBytes LIFE_TEXT = new MemoryBytes(10, 13); //TODO: prove this is valid
        public static readonly MemoryBytes GRIP_Lv1_TEXT = new MemoryBytes(22, 29); //TODO: prove this is valid
        public static readonly MemoryBytes GRIP_Lv2_TEXT = new MemoryBytes(-156, -149); //TODO: prove this is valid
        public static readonly MemoryBytes GRIP_Lv3_TEXT = new MemoryBytes(-172, -165); //TODO: prove this is valid
        #endregion

        #region Calculated From RationMedsBandagePentazeminDescriptions
        //TODO: fill these out
        #endregion

        #region Calculated From RayNames
        public static readonly MemoryBytes Ray_01 = new MemoryBytes(78, 85); //TODO: prove these are valid (these values are using the endByte, cuz im a fool)
        //public static readonly MemoryBytes Ray_01 = new MemoryBytes(68, 75); //This should work if the uncommented one does not
        public static readonly MemoryBytes Ray_02 = new MemoryBytes(Ray_01.OffsetStart + 16, Ray_01.OffsetEnd + 16);
        public static readonly MemoryBytes Ray_03 = new MemoryBytes(Ray_02.OffsetStart + 16, Ray_02.OffsetEnd + 16);
        public static readonly MemoryBytes Ray_04 = new MemoryBytes(Ray_03.OffsetStart + 16, Ray_03.OffsetEnd + 16);
        public static readonly MemoryBytes Ray_05 = new MemoryBytes(Ray_04.OffsetStart + 16, Ray_04.OffsetEnd + 16);
        public static readonly MemoryBytes Ray_06 = new MemoryBytes(Ray_05.OffsetStart + 16, Ray_05.OffsetEnd + 16);
        public static readonly MemoryBytes Ray_07 = new MemoryBytes(Ray_06.OffsetStart + 16, Ray_06.OffsetEnd + 16);
        public static readonly MemoryBytes Ray_08 = new MemoryBytes(Ray_07.OffsetStart + 16, Ray_07.OffsetEnd + 16);
        public static readonly MemoryBytes Ray_09 = new MemoryBytes(Ray_08.OffsetStart + 16, Ray_08.OffsetEnd + 16);
        public static readonly MemoryBytes Ray_10 = new MemoryBytes(Ray_09.OffsetStart + 16, Ray_09.OffsetEnd + 16);
        public static readonly MemoryBytes Ray_11 = new MemoryBytes(Ray_10.OffsetStart + 16, Ray_10.OffsetEnd + 16);
        public static readonly MemoryBytes Ray_12 = new MemoryBytes(Ray_11.OffsetStart + 16, Ray_11.OffsetEnd + 16);
        public static readonly MemoryBytes Ray_13 = new MemoryBytes(Ray_12.OffsetStart + 16, Ray_12.OffsetEnd + 16);
        public static readonly MemoryBytes Ray_14 = new MemoryBytes(Ray_13.OffsetStart + 16, Ray_13.OffsetEnd + 16);
        public static readonly MemoryBytes Ray_15 = new MemoryBytes(Ray_14.OffsetStart + 16, Ray_14.OffsetEnd + 16);
        public static readonly MemoryBytes Ray_16 = new MemoryBytes(Ray_15.OffsetStart + 16, Ray_15.OffsetEnd + 16);
        public static readonly MemoryBytes Ray_17 = new MemoryBytes(Ray_16.OffsetStart + 16, Ray_16.OffsetEnd + 16);
        public static readonly MemoryBytes Ray_18 = new MemoryBytes(Ray_17.OffsetStart + 16, Ray_17.OffsetEnd + 16);
        public static readonly MemoryBytes Ray_19 = new MemoryBytes(Ray_18.OffsetStart + 16, Ray_18.OffsetEnd + 16);
        public static readonly MemoryBytes Ray_20 = new MemoryBytes(Ray_19.OffsetStart + 16, Ray_19.OffsetEnd + 16);
        public static readonly MemoryBytes Ray_21 = new MemoryBytes(Ray_20.OffsetStart + 16, Ray_20.OffsetEnd + 16);
        public static readonly MemoryBytes Ray_22 = new MemoryBytes(Ray_21.OffsetStart + 16, Ray_21.OffsetEnd + 16);
        public static readonly MemoryBytes Ray_23 = new MemoryBytes(Ray_22.OffsetStart + 16, Ray_22.OffsetEnd + 16);
        public static readonly MemoryBytes Ray_24 = new MemoryBytes(Ray_23.OffsetStart + 16, Ray_23.OffsetEnd + 16);
        public static readonly MemoryBytes Ray_25 = new MemoryBytes(Ray_24.OffsetStart + 16, Ray_24.OffsetEnd + 16);
        #endregion
        #endregion
        #endregion

        #region Item Table
        public const int RationOffset = 0;
        public const int SnakeScopeOffset = 2;
        public const int ColdMedicineOffset = 4;
        public const int BandageOffset = 6;
        public const int PentazeminOffset = 8;
        public const int BDUOffset = 10;
        public const int BodyArmorOffset = 12;
        public const int StealthOffset = 14;
        public const int MineDetectorOffset = 16;
        public const int SensorAOffset = 18;
        public const int SensorBOffset = 20;
        public const int NightVisionGogglesOffset = 22;
        public const int ThermalGogglesOffset = 24;
        public const int RaidenScopeOffset = 26;
        public const int DigitalCameraOffset = 28;
        public const int Box1Offset = 30;
        public const int CigarettesOffset = 32;
        public const int CardOffset = 34;
        public const int ShaverOffset = 36;
        public const int PhoneOffset = 38;
        public const int Camera1Offset = 40;
        public const int Box2Offset = 42;
        public const int Box3Offset = 44;
        public const int WetBoxOffset = 46;
        public const int APSensorOffset = 48;
        public const int Box4Offset = 50;
        public const int Box5Offset = 52;
        public const int UnknownItemOffset = 54; //razor?
        public const int SocomSuppressorOffset = 56;
        public const int AKSuppressorOffset = 58;
        public const int Camera2Offset = 60;
        public const int BandanaOffset = 62;
        public const int DogTagsOffset = 64;
        public const int MODiscOffset = 66;
        public const int USPSuppressorOffset = 68;
        public const int InfinityWigOffset = 70;
        public const int BlueWigOffset = 72;
        public const int OrangeWigOffset = 74;
        public const int ColorWigOffset = 76;
        public const int ColorWig2Offset = 78;
        #endregion

        #region Weapon Table
        public const int M9Offset = 0;
        public const int USPOffset = 2;
        public const int SOCOMOffset = 4;
        public const int PSG1Offset = 6;
        public const int RGB6Offset = 8;
        public const int NikitaOffset = 10;
        public const int StingerOffset = 12;
        public const int ClaymoreOffset = 14;
        public const int C4Offset = 16;
        public const int ChaffGrenadeOffset = 18;
        public const int StunGrenadeOffset = 20;
        public const int DMicOffset = 22;
        public const int HighFrequencyBladeOffset = 24;
        public const int CoolantOffset = 26;
        public const int AKS74uOffset = 28;
        public const int MagazineOffset = 30;
        public const int GrenadeOffset = 32;
        public const int M4Offset = 34;
        public const int PSG1TOffset = 36;
        public const int DMic2Offset = 38;
        public const int BookOffset = 40;
        #endregion

        #region Character Usable Object Lists
        public static List<MGS2Object> SnakeUsableObjects = new List<MGS2Object>
        {
            MGS2UsableObjects.AKSuppressor,
            MGS2UsableObjects.APSensor,
            MGS2UsableObjects.Bandage,
            MGS2UsableObjects.Bandana,
            MGS2UsableObjects.Box1,
            MGS2UsableObjects.Camera1,
            MGS2UsableObjects.Camera2,
            MGS2UsableObjects.Card,
            MGS2UsableObjects.ChaffGrenade,
            MGS2UsableObjects.Cigarettes,
            MGS2UsableObjects.ColdMedicine,
            MGS2UsableObjects.DigitalCamera,
            MGS2UsableObjects.DogTags,
            MGS2UsableObjects.Grenade,
            MGS2UsableObjects.Magazine,
            MGS2UsableObjects.MineDetector,
            MGS2UsableObjects.MODisc,
            MGS2UsableObjects.M9,
            MGS2UsableObjects.Pentazemin,
            MGS2UsableObjects.Phone,
            MGS2UsableObjects.RaidenScope,
            MGS2UsableObjects.Ration,
            MGS2UsableObjects.SensorA,
            MGS2UsableObjects.SensorB,
            MGS2UsableObjects.Shaver,
            MGS2UsableObjects.SnakeScope,
            MGS2UsableObjects.SocomSuppressor,
            MGS2UsableObjects.Stealth,
            MGS2UsableObjects.StunGrenade,
            MGS2UsableObjects.ThermalGoggles,
            MGS2UsableObjects.USP,
            MGS2UsableObjects.USPSuppressor,
            MGS2UsableObjects.WetBox
        };

        public static List<MGS2Object> RaidenUsableObjects = new List<MGS2Object>
        {
            MGS2UsableObjects.AKS74u,
            MGS2UsableObjects.AKSuppressor,
            MGS2UsableObjects.APSensor,
            MGS2UsableObjects.Bandage,
            MGS2UsableObjects.BDU,
            MGS2UsableObjects.BlueWig,
            MGS2UsableObjects.BodyArmor,
            MGS2UsableObjects.Book,
            MGS2UsableObjects.Box1,
            MGS2UsableObjects.Box2,
            MGS2UsableObjects.Box3,
            MGS2UsableObjects.Box4,
            MGS2UsableObjects.Box5,
            MGS2UsableObjects.C4,
            MGS2UsableObjects.Camera1,
            MGS2UsableObjects.Camera2,
            MGS2UsableObjects.Card,
            MGS2UsableObjects.ChaffGrenade,
            MGS2UsableObjects.Cigarettes,
            MGS2UsableObjects.Claymore,
            MGS2UsableObjects.ColdMedicine,
            MGS2UsableObjects.ColorWig,
            MGS2UsableObjects.ColorWig2,
            MGS2UsableObjects.Coolant,
            MGS2UsableObjects.DigitalCamera,
            MGS2UsableObjects.DMic1,
            MGS2UsableObjects.DMic2,
            MGS2UsableObjects.DogTags,
            MGS2UsableObjects.Grenade,
            MGS2UsableObjects.HighFrequencyBlade,
            MGS2UsableObjects.InfinityWig,
            MGS2UsableObjects.M4,
            MGS2UsableObjects.M9,
            MGS2UsableObjects.Magazine,
            MGS2UsableObjects.MineDetector,
            MGS2UsableObjects.MODisc,
            MGS2UsableObjects.NightVisionGoggles,
            MGS2UsableObjects.Nikita,
            MGS2UsableObjects.OrangeWig,
            MGS2UsableObjects.Pentazemin,
            MGS2UsableObjects.Phone,
            MGS2UsableObjects.PSG1,
            MGS2UsableObjects.PSG1T,
            MGS2UsableObjects.RaidenScope,
            MGS2UsableObjects.Ration,
            MGS2UsableObjects.RGB6,
            MGS2UsableObjects.SensorA,
            MGS2UsableObjects.SensorB,
            MGS2UsableObjects.Shaver,
            MGS2UsableObjects.SnakeScope,
            MGS2UsableObjects.SOCOM,
            MGS2UsableObjects.SocomSuppressor,
            MGS2UsableObjects.Stealth,
            MGS2UsableObjects.StunGrenade,
            MGS2UsableObjects.ThermalGoggles,
            MGS2UsableObjects.USPSuppressor,
            MGS2UsableObjects.WetBox
        };
        #endregion
    };
}
