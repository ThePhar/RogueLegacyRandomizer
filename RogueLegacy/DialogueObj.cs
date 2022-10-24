// Rogue Legacy Randomizer - DialogueObj.cs
// Last Modified 2022-10-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using DS2DEngine;

namespace RogueLegacy
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
