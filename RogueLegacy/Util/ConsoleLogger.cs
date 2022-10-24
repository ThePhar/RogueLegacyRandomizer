// Rogue Legacy Randomizer - ConsoleLogger.cs
// Last Modified 2022-10-24
// 
// This project is based on the modified disassembly of Rogue Legacy's engine, with permission to do so by its
// original creators. Therefore, the former creators' copyright notice applies to the original disassembly.
// 
// Original Source © 2011-2015, Cellar Door Games Inc.
// Rogue Legacy™ is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.

using System;
using System.IO;
using System.Text;

namespace RogueLegacy.Util;

public class ConsoleLogger : IDisposable
{
    private          StringWriter _logWriter;
    private          TextWriter   _doubleWriter;
    private readonly TextWriter   _oldOut;

    private class DoubleWriter : TextWriter
    {
        private readonly TextWriter _one;
        private readonly TextWriter _two;

        public DoubleWriter(TextWriter one, TextWriter two)
        {
            _one = one;
            _two = two;
        }

        public override Encoding Encoding => _one.Encoding;

        public override void Flush()
        {
            _one.Flush();
            _two.Flush();
        }

        public override void Write(char value)
        {
            _one.Write(value);
            _two.Write(value);
        }

    }

    public ConsoleLogger()
    {
        _oldOut = Console.Out;

        try
        {
            _logWriter = new StringWriter();
            _doubleWriter = new DoubleWriter(_logWriter, _oldOut);
        }
        catch (Exception e)
        {
            Console.WriteLine("Cannot start both Writers.");
            Console.WriteLine(e.Message);
            return;
        }
        Console.SetOut(_doubleWriter);
    }

    public void Dispose()
    {
        Console.SetOut(_oldOut);
        if (_logWriter == null)
        {
            return;
        }

        _logWriter.Flush();
        _logWriter.Close();
        _logWriter = null;
    }

    public string Log => _logWriter.ToString();
}
