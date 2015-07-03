/***************************************************************************
 *
 * $Author: Jeff Boulanger
 * 
 * This work is protected by the Creative Commons Attribution-Noncommercial-No 
 * Derivative Works 3.0 Unported License.  All information regarding this 
 * license can be found at http://creativecommons.org/licenses/by-nc-nd/3.0/
 *
 ***************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections;

namespace Ultima.GDK
{
    public class XmlSettings
    {
        private Dictionary<string, object> settings;

        public XmlSettings()
        {
            settings = new Dictionary<string, object>();
        }
        
        public object this[string key]
        {
            get
            {
                if (settings.ContainsKey(key))
                {
                    return settings[key];
                }

                return null;
            }

            set
            {
                if (settings.ContainsKey(key))
                {
                    settings[key] = value;
                }
                else
                {
                    settings.Add(key, value);
                }
            }
        }

        public T GetValue<T>(string key, T defaultValue)
        {
            object obj;

            if (!settings.TryGetValue(key, out obj))
            {
                settings.Add(key, defaultValue);
                obj = settings[key];
            }

            return (T)obj;
        }

        public void SetValue<T>(string key, T value)
        {
            if (settings.ContainsKey(key))
            {
                settings[key] = value;
            }
            else
            {
                settings.Add(key, value);
            }
        }

        public void Save(string path)
        {
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.CloseOutput = true;
            writerSettings.ConformanceLevel = ConformanceLevel.Document;
            writerSettings.Encoding = Encoding.Unicode;
            writerSettings.Indent = true;
            writerSettings.IndentChars = "\t";
            writerSettings.NewLineChars = "\n";
            writerSettings.NewLineHandling = NewLineHandling.Entitize;
            writerSettings.NewLineOnAttributes = false;
            writerSettings.OmitXmlDeclaration = true;
 
            XmlWriter writer = XmlWriter.Create(path, writerSettings);
            writer.WriteStartElement("configuration");

            ICollection<string> keys = settings.Keys;   
            IEnumerator<string> enumerator = keys.GetEnumerator();

            while(enumerator.MoveNext())
            {
                writer.WriteStartElement("setting");
                writer.WriteAttributeString("type", settings[enumerator.Current].GetType().ToString());
                writer.WriteAttributeString("key", enumerator.Current);
                writer.WriteAttributeString("value", settings[enumerator.Current].ToString());
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.Close();
        }

        public static XmlSettings Load(string path)
        {
            XmlSettings sett = new XmlSettings();

            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlElement root = doc.DocumentElement;
            XmlNodeList nodes = root.GetElementsByTagName("setting");

            for (int i = 0; i < nodes.Count; i++)
            {
                Type type = Type.GetType(nodes[i].Attributes["type"].Value);

                object convert = nodes[i].Attributes["value"].Value;
                object obj = Convert.ChangeType(convert, type);

                sett.settings.Add(nodes[i].Attributes["key"].Value, 
                    obj);
            }

            return sett;
        }
    }
}
