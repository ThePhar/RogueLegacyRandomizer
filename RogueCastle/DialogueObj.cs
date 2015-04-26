/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System;
using DS2DEngine;

namespace RogueCastle
{
    public class DialogueObj : IDisposableObj
    {
        public DialogueObj(string[] speakers, string[] dialogue)
        {
            if (speakers.Length != dialogue.Length)
            {
                throw new Exception("Cannot create dialogue obj with mismatching speakers and dialogue");
            }
            Speakers = speakers;
            Dialogue = dialogue;
        }

        public string[] Speakers { get; set; }
        public string[] Dialogue { get; set; }
        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                Array.Clear(Dialogue, 0, Dialogue.Length);
                Dialogue = null;
                Array.Clear(Speakers, 0, Speakers.Length);
                Speakers = null;
                IsDisposed = true;
            }
        }
    }
}