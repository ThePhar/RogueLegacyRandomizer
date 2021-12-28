// 
// RogueLegacyArchipelago - LegacyLocations.cs
// Last Modified 2021-12-27
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, former creators' copyright notice applies to the original disassembly.
// 
// Original Disassembled Source - © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
// 

using System.Collections.Generic;

namespace Archipelago.Legacy
{
    public class LegacyLocations : Dictionary<string, int>
    {
        public LegacyLocations()
        {
            // Boss Rewards
            Add("Khindr's Reward Left",         4445000);
            Add("Khindr's Reward Right",        4445001);
            Add("Alexander's Reward Left",      4445002);
            Add("Alexander's Reward Right",     4445003);
            Add("Ponce de Leon's Reward Left",  4445004);
            Add("Ponce de Leon's Reward Right", 4445005);
            Add("Herodotus's Reward Left",      4445006);
            Add("Herodotus's Reward Right",     4445007);

            // Manor Purchases
            Add("Manor Observatory Telescope",  4445008);
            Add("Manor Observatory Base",       4445009);
            Add("Manor Right High Tower",       4445010);
            Add("Manor Right High Upper",       4445011);
            Add("Manor Right High Base",        4445012);
            Add("Manor Right Big Roof",         4445013);
            Add("Manor Right Big Upper",        4445014);
            Add("Manor Right Big Base",         4445015);
            Add("Manor Right Wing Roof",        4445016);
            Add("Manor Right Wing Window",      4445017);
            Add("Manor Right Wing Base",        4445018);
            Add("Manor Right Extension",        4445019);
            Add("Manor Left Far Roof",          4445020);
            Add("Manor Left Far Base",          4445021);
            Add("Manor Left Big Roof",          4445022);
            Add("Manor Left Big Upper 2",       4445023);
            Add("Manor Left Big Upper",         4445024);
            Add("Manor Left Big Windows",       4445025);
            Add("Manor Left Big Base",          4445026);
            Add("Manor Left Wing Roof",         4445027);
            Add("Manor Left Wing Window",       4445028);
            Add("Manor Left Wing Base",         4445029);
            Add("Manor Left Extension",         4445030);
            Add("Manor Main Roof",              4445031);
            Add("Manor Main Base",              4445032);
            Add("Manor Front Top Windows",      4445033);
            Add("Manor Front Bottom Windows",   4445034);
            Add("Manor Ground Road",            4445035);
            Add("Manor Left Tree 1",            4445036);
            Add("Manor Left Tree 2",            4445037);
            Add("Manor Right Tree",             4445038);

            // Special Rooms
            Add("Carnival",                     4445039);
            Add("Cheapskate Elf",               4445040);
            Add("Jukebox",                      4445041);
            Add("Secret Room Chest",            4445042);

            // Diaries
            Add("Diary 1",                      4445043);
            Add("Diary 2",                      4445044);
            Add("Diary 3",                      4445045);
            Add("Diary 4",                      4445046);
            Add("Diary 5",                      4445047);
            Add("Diary 6",                      4445048);
            Add("Diary 7",                      4445049);
            Add("Diary 8",                      4445050);
            Add("Diary 9",                      4445051);
            Add("Diary 10",                     4445052);
            Add("Diary 11",                     4445053);
            Add("Diary 12",                     4445054);
            Add("Diary 13",                     4445055);
            Add("Diary 14",                     4445056);
            Add("Diary 15",                     4445057);
            Add("Diary 16",                     4445058);
            Add("Diary 17",                     4445059);
            Add("Diary 18",                     4445060);
            Add("Diary 19",                     4445061);
            Add("Diary 20",                     4445062);
            Add("Diary 21",                     4445063);
            Add("Diary 22",                     4445064);
            Add("Diary 23",                     4445065);
            Add("Diary 24",                     4445066);

            // Fairy Chests
            Add("Castle Fairy Chest 1",         4445067);
            Add("Castle Fairy Chest 2",         4445068);
            Add("Castle Fairy Chest 3",         4445069);
            Add("Castle Fairy Chest 4",         4445070);
            Add("Castle Fairy Chest 5",         4445071);
            Add("Castle Fairy Chest 6",         4445072);
            Add("Castle Fairy Chest 7",         4445073);
            Add("Castle Fairy Chest 8",         4445074);
            Add("Castle Fairy Chest 9",         4445075);
            Add("Castle Fairy Chest 10",        4445076);
            Add("Garden Fairy Chest 1",         4445077);
            Add("Garden Fairy Chest 2",         4445078);
            Add("Garden Fairy Chest 3",         4445079);
            Add("Garden Fairy Chest 4",         4445080);
            Add("Garden Fairy Chest 5",         4445081);
            Add("Garden Fairy Chest 6",         4445082);
            Add("Garden Fairy Chest 7",         4445083);
            Add("Garden Fairy Chest 8",         4445084);
            Add("Garden Fairy Chest 9",         4445085);
            Add("Garden Fairy Chest 10",        4445086);
            Add("Tower Fairy Chest 1",          4445087);
            Add("Tower Fairy Chest 2",          4445088);
            Add("Tower Fairy Chest 3",          4445089);
            Add("Tower Fairy Chest 4",          4445090);
            Add("Tower Fairy Chest 5",          4445091);
            Add("Tower Fairy Chest 6",          4445092);
            Add("Tower Fairy Chest 7",          4445093);
            Add("Tower Fairy Chest 8",          4445094);
            Add("Tower Fairy Chest 9",          4445095);
            Add("Tower Fairy Chest 10",         4445096);
            Add("Dungeon Fairy Chest 1",        4445097);
            Add("Dungeon Fairy Chest 2",        4445098);
            Add("Dungeon Fairy Chest 3",        4445099);
            Add("Dungeon Fairy Chest 4",        4445100);
            Add("Dungeon Fairy Chest 5",        4445101);
            Add("Dungeon Fairy Chest 6",        4445102);
            Add("Dungeon Fairy Chest 7",        4445103);
            Add("Dungeon Fairy Chest 8",        4445104);
            Add("Dungeon Fairy Chest 9",        4445105);
            Add("Dungeon Fairy Chest 10",       4445106);

            // Chests
            Add("Castle Chest 1",               4445107);
            Add("Castle Chest 2",               4445108);
            Add("Castle Chest 3",               4445109);
            Add("Castle Chest 4",               4445110);
            Add("Castle Chest 5",               4445111);
            Add("Castle Chest 6",               4445112);
            Add("Castle Chest 7",               4445113);
            Add("Castle Chest 8",               4445114);
            Add("Castle Chest 9",               4445115);
            Add("Castle Chest 10",              4445116);
            Add("Castle Chest 11",              4445117);
            Add("Castle Chest 12",              4445118);
            Add("Castle Chest 13",              4445119);
            Add("Castle Chest 14",              4445120);
            Add("Castle Chest 15",              4445121);
            Add("Castle Chest 16",              4445122);
            Add("Castle Chest 17",              4445123);
            Add("Castle Chest 18",              4445124);
            Add("Castle Chest 19",              4445125);
            Add("Castle Chest 20",              4445126);
            Add("Garden Chest 1",               4445127);
            Add("Garden Chest 2",               4445128);
            Add("Garden Chest 3",               4445129);
            Add("Garden Chest 4",               4445130);
            Add("Garden Chest 5",               4445131);
            Add("Garden Chest 6",               4445132);
            Add("Garden Chest 7",               4445133);
            Add("Garden Chest 8",               4445134);
            Add("Garden Chest 9",               4445135);
            Add("Garden Chest 10",              4445136);
            Add("Garden Chest 11",              4445137);
            Add("Garden Chest 12",              4445138);
            Add("Garden Chest 13",              4445139);
            Add("Garden Chest 14",              4445140);
            Add("Garden Chest 15",              4445141);
            Add("Garden Chest 16",              4445142);
            Add("Garden Chest 17",              4445143);
            Add("Garden Chest 18",              4445144);
            Add("Garden Chest 19",              4445145);
            Add("Garden Chest 20",              4445146);
            Add("Tower Chest 1",                4445147);
            Add("Tower Chest 2",                4445148);
            Add("Tower Chest 3",                4445149);
            Add("Tower Chest 4",                4445150);
            Add("Tower Chest 5",                4445151);
            Add("Tower Chest 6",                4445152);
            Add("Tower Chest 7",                4445153);
            Add("Tower Chest 8",                4445154);
            Add("Tower Chest 9",                4445155);
            Add("Tower Chest 10",               4445156);
            Add("Tower Chest 11",               4445157);
            Add("Tower Chest 12",               4445158);
            Add("Tower Chest 13",               4445159);
            Add("Tower Chest 14",               4445160);
            Add("Tower Chest 15",               4445161);
            Add("Tower Chest 16",               4445162);
            Add("Tower Chest 17",               4445163);
            Add("Tower Chest 18",               4445164);
            Add("Tower Chest 19",               4445165);
            Add("Tower Chest 20",               4445166);
            Add("Dungeon Chest 1",              4445167);
            Add("Dungeon Chest 2",              4445168);
            Add("Dungeon Chest 3",              4445169);
            Add("Dungeon Chest 4",              4445170);
            Add("Dungeon Chest 5",              4445171);
            Add("Dungeon Chest 6",              4445172);
            Add("Dungeon Chest 7",              4445173);
            Add("Dungeon Chest 8",              4445174);
            Add("Dungeon Chest 9",              4445175);
            Add("Dungeon Chest 10",             4445176);
            Add("Dungeon Chest 11",             4445177);
            Add("Dungeon Chest 12",             4445178);
            Add("Dungeon Chest 13",             4445179);
            Add("Dungeon Chest 14",             4445180);
            Add("Dungeon Chest 15",             4445181);
            Add("Dungeon Chest 16",             4445182);
            Add("Dungeon Chest 17",             4445183);
            Add("Dungeon Chest 18",             4445184);
            Add("Dungeon Chest 19",             4445185);
            Add("Dungeon Chest 20",             4445186);
        }
    }
}
