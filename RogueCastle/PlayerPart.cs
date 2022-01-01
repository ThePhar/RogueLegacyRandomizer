/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using Microsoft.Xna.Framework;

namespace RogueCastle
{
    public class PlayerPart
    {
        public const int None = -1;
        public const int Wings = 0;
        public const int Cape = 1;
        public const int Legs = 2;
        public const int ShoulderB = 3;
        public const int Chest = 4;
        public const int Boobs = 5;
        public const int Arms = 6;
        public const int Hair = 7;
        public const int Neck = 8;
        public const int ShoulderA = 9;
        public const int Sword1 = 10;
        public const int Sword2 = 11;
        public const int Head = 12;
        public const int Bowtie = 13;
        public const int Glasses = 14;
        public const int Extra = 15;
        public const int Light = 16;
        public const int NumHeadPieces = 5;
        public const int NumChestPieces = 5;
        public const int NumShoulderPieces = 5;
        public const int DragonHelm = 6;
        public const int IntroHelm = 7;

        public static Vector3 GetPartIndices(int category)
        {
            switch (category)
            {
                case 0:
                    return new Vector3(10f, 11f, -1f);

                case 1:
                    return new Vector3(12f, 7f, -1f);

                case 2:
                    return new Vector3(4f, 3f, 9f);

                case 3:
                    return new Vector3(6f, 2f, -1f);

                case 4:
                    return new Vector3(1f, 8f, -1f);

                default:
                    return new Vector3(-1f, -1f, -1f);
            }
        }
    }
}