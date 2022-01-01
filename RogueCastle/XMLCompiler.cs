/*
  Rogue Legacy Enhanced

  This project is based on modified disassembly of Rogue Legacy's engine, with permission to do so by its creators.
  Therefore, former creators copyright notice applies to original disassembly. 

  Disassembled source Copyright(C) 2011-2015, Cellar Door Games Inc.
  Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.
*/

using System.Collections.Generic;
using System.Xml;

namespace RogueCastle
{
    public class XMLCompiler
    {
        public static void CompileEnemies(List<EnemyEditorData> enemyDataList, string filePath)
        {
            var xmlWriterSettings = new XmlWriterSettings
                { Indent = true, ConformanceLevel = ConformanceLevel.Fragment };
            var xmlWriter = XmlWriter.Create(filePath + "\\EnemyList.xml", xmlWriterSettings);
            var str = "<xml>";
            xmlWriter.WriteStartElement("xml");
            foreach (var current in enemyDataList)
            {
                str += "<EnemyObj>\n";
                xmlWriter.WriteStartElement("EnemyObj");
                var arg_79_0 = xmlWriter;
                var arg_79_1 = "Type";
                var type = current.Type;
                arg_79_0.WriteAttributeString(arg_79_1, type.ToString());
                xmlWriter.WriteAttributeString("SpriteName", current.SpriteName);
                var arg_AB_0 = xmlWriter;
                var arg_AB_1 = "BasicScaleX";
                var x = current.BasicScale.X;
                arg_AB_0.WriteAttributeString(arg_AB_1, x.ToString());
                var arg_CB_0 = xmlWriter;
                var arg_CB_1 = "BasicScaleY";
                var y = current.BasicScale.Y;
                arg_CB_0.WriteAttributeString(arg_CB_1, y.ToString());
                var arg_EB_0 = xmlWriter;
                var arg_EB_1 = "AdvancedScaleX";
                var x2 = current.AdvancedScale.X;
                arg_EB_0.WriteAttributeString(arg_EB_1, x2.ToString());
                var arg_10B_0 = xmlWriter;
                var arg_10B_1 = "AdvancedScaleY";
                var y2 = current.AdvancedScale.Y;
                arg_10B_0.WriteAttributeString(arg_10B_1, y2.ToString());
                var arg_12B_0 = xmlWriter;
                var arg_12B_1 = "ExpertScaleX";
                var x3 = current.ExpertScale.X;
                arg_12B_0.WriteAttributeString(arg_12B_1, x3.ToString());
                var arg_14B_0 = xmlWriter;
                var arg_14B_1 = "ExpertScaleY";
                var y3 = current.ExpertScale.Y;
                arg_14B_0.WriteAttributeString(arg_14B_1, y3.ToString());
                var arg_16B_0 = xmlWriter;
                var arg_16B_1 = "MinibossScaleX";
                var x4 = current.MinibossScale.X;
                arg_16B_0.WriteAttributeString(arg_16B_1, x4.ToString());
                var arg_18B_0 = xmlWriter;
                var arg_18B_1 = "MinibossScaleY";
                var y4 = current.MinibossScale.Y;
                arg_18B_0.WriteAttributeString(arg_18B_1, y4.ToString());
                xmlWriter.WriteEndElement();
                str += "</EnemyObj>\n";
            }

            xmlWriter.WriteEndElement();
            xmlWriter.Flush();
            xmlWriter.Close();
        }
    }
}