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
        /// <summary>
        /// 反序列化方式读取xml文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="schem">引用传递全局的TSchem类作为赋值对象</param>
        public void LoadXml(string path, ref TSchem schem)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TSchem));
            FileStream fs1 = new FileStream(path, FileMode.Open);
            XmlReader reader = XmlReader.Create(fs1);
            schem = (TSchem)serializer.Deserialize(reader);
            fs1.Close();
        }
        /// <summary>
        /// 将TSchem类序列化为xml文件并保存
        /// </summary>
        /// <param name="path">文件保存路径</param>
        /// <param name="schem">待序列化的类</param>
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
