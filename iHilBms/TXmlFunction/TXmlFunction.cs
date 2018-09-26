using iinterface;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace TXmlFunction
{
    [Export("TXmlFunction", typeof(iXmlFunction))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class TXmlFunction:iXmlFunction
    {
        public void LoadXml(string path, ref TSchem schem)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TSchem));
            FileStream fs1 = new FileStream(path, FileMode.Open);
            XmlReader reader = XmlReader.Create(fs1);
            schem = (TSchem)serializer.Deserialize(reader);
            fs1.Close();
        }
        public void SaveXml(string path, TSchem schem)
        {
            XmlSerializer xs = new XmlSerializer(schem.GetType());
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = Encoding.GetEncoding("gb2312");
            settings.Indent = true;
            XmlWriter writer = XmlWriter.Create(path, settings);
            ns.Add("", "");
            xs.Serialize(writer, schem, ns);
            writer.Close();
        }
    }
}
