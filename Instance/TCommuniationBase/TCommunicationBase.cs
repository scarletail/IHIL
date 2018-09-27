using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iinterface;
using System.ComponentModel.Composition;
using System.Xml;

//将"TCommuniationBase"更名为"TCommunicationBase",修改了相应的接口名，并重新生成dll文件，重设工程的引用逻辑 @Deer

namespace instance
{
    [Export("TCommunicationBase", typeof(iCommunicationBase))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class TCommunicationBase : iCommunicationBase
    {
        public XmlElement FileNodeXe { get; set; }
        /// <summary>
        /// 变量名称
        /// </summary>
        private string varName;
        public string VarName
        {
            get
            { return varName; }
            set
            {
                varName = value;
                if (FileNodeXe != null)
                    FileNodeXe.SetAttribute("id", value);
            }
        }
        /// <summary
        /// 变量显示值
        /// </summary>
        private string varCaption;
        public string VarCaption
        {
            get
            { return varCaption; }
            set
            {
                varCaption = value;
                if (FileNodeXe != null)
                    FileNodeXe.SetAttribute("caption", value);
            }
        }

        /// <summary>
        /// 变量单位
        /// </summary>
        private string varUnit;
        public string VarUnit
        {
            get
            { return varUnit; }
            set
            {
                varUnit = value;
                if (FileNodeXe != null)
                    FileNodeXe.SetAttribute("dw", value);
            }
        }
        /// <summary>
        ///变量值
        /// </summary>
        public string VarValue { get; set; }
        /// <summary>
        /// 变量的前辍；
        /// </summary>
        private string prev;
        public string Prev
        {
            get
            { return prev; }
            set
            {
                prev = value;

            }
        }
        //在调用类成员set方法时均对FileNodeXe中的对应Attribute进行赋值，但是遗漏了"varUnit"和"prev" @Deer
        //CaseNode方法中将filenode的引用传递给FileNodeXe，并提取相关属性赋值给类中成员 @Deer
        public void CaseNode(XmlNode filenode)
        {
            FileNodeXe = (XmlElement)filenode;
            varName = FileNodeXe.GetAttribute("id");
            varCaption = FileNodeXe.GetAttribute("caption");
            varUnit = FileNodeXe.GetAttribute("unit");
        }

    }
}
