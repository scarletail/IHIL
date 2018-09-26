﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.ComponentModel.Composition;
using iinterface;
using System.IO;

//实现了ICANVariables中定义的成员与方法，其中VarUnit、VarValue、Prev未找到相关定义 @Deer

namespace instance
{
    [Export("TCANVarible", typeof(iCANVariables))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class TCANVarible : TCommunicationBase, iCANVariables
    {
        private string varName;
        private string varCaption;
        private int startBit;
        private int bitLength;
        private int model;

        /// <summary>
        /// 起始位
        /// </summary>
        public int StartBit
        {
            get
            { return startBit; }
            set
            {
                startBit = value;
                if (FileNodeXe != null)
                {
                    (FileNodeXe).SetAttribute("startbit", value.ToString());
                }
            }
        }
        /// <summary>
        /// 位长度
        /// </summary>
        public int BitLength
        {
            get
            { return bitLength; }
            set
            {
                bitLength = value;
                if (FileNodeXe != null)
                {
                    (FileNodeXe).SetAttribute("bitlength", value.ToString());
                }
            }
        }

        /// <summary>
        /// Intel/1 OR  Motorola/0模式
        /// </summary>
        public int Model
        {
            get
            {
                return model;
            }
            set
            {
                model = value;
                if (FileNodeXe != null)
                {
                    (FileNodeXe).SetAttribute("bytefrombl", value.ToString());
                }
            }
        }
        // { get; set; }

        //其他的还有...
        //已补全 @Deer
        public string VarName {
            get {
                return varName;
            }
            set {
                varName = value;
                if (FileNodeXe != null)
                {
                    FileNodeXe.SetAttribute("bl",value.ToString());
                }
            }
        }
        public string VarCaption {
            get {
                return varCaption;
            }
            set {
                VarCaption = value;
                if (FileNodeXe != null)
                {
                    FileNodeXe.SetAttribute("caption", value.ToString());
                }
            }
        }
        //系统自动生成 @Deer
        public string VarUnit { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string VarValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Prev { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        //CaseNode方法中已将VarUnit,VarValue,Prev的实例化加入 @Deer
        //该方法中定义的成员与0904.hil中格式不符，无法确定最终格式 @Deer
        public void CaseNode(XmlNode iVarible)
        {
            base.CaseNode(iVarible);
            //fileNode = iVarible;
            // XmlElement  FileNodeXe= (XmlElement)iVarible;
            startBit = Convert.ToInt16(FileNodeXe.GetAttribute("startbit"));
            bitLength = Convert.ToInt16(FileNodeXe.GetAttribute("bitlength"));
            varName = FileNodeXe.GetAttribute("bl");
            varCaption = FileNodeXe.GetAttribute("caption");
            model = Convert.ToInt16(FileNodeXe.GetAttribute("bytefrombl"));          //  
            VarUnit = FileNodeXe.GetAttribute("varunit");
            VarValue = FileNodeXe.GetAttribute("varvalue");
            Prev = FileNodeXe.GetAttribute("prev");
        }

    }



    [Export("TCANID", typeof(iCANID))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    /// <summary>
    /// CAN的报文
    /// </summary>
    public class TCANID : iCANID
    {
        private List<TCANVarible> CanBlList = new List<TCANVarible>();
        private XmlNode fileNode;
        private string canid;
        /// <summary>
        /// 报文ID 为十六进制
        /// </summary>
        public string CANID
        {
            get
            { return canid; }
            set
            {
                canid = value;
                if (fileNode != null)
                    ((XmlElement)fileNode).SetAttribute("id", value);
            }
        }
        private string canName;
        /// <summary>
        /// 报文名称
        /// </summary>
        public string CANName
        {
            get
            { return canName; }
            set
            {
                canName = value;
                if (fileNode != null)
                    ((XmlElement)fileNode).SetAttribute("caption", value);
            }
        }
        /// <summary>
        /// 报文备注
        /// </summary>
        public string CANMemo { get; set; }


        public iCANVariables FindByIndex(int idx)
        {
            return CanBlList[idx];

        }
        /// <summary>
        /// 根据变量名称获取对应的变量
        /// </summary>
        /// <param name="sName"></param>
        /// <returns></returns>
        public iCANVariables FindByName(string sName)
        {

            return CanBlList.Find(t => ((iCANVariables)t).VarName == sName);
        }
        //接口TComunicationBase中Casenode方法实现 @Deer
        //Casenode方法类似于将ICANNode内容赋值给类中成员，并让ICANNode的引用传递给filenode @Deer
        //难道是封装(encapsulation)的缩写?case也有包装的意思 @Deer
        public void CaseNode(XmlNode ICANNode)
        {
            fileNode = ICANNode;
            XmlElement FileNodeXe = (XmlElement)ICANNode;
            canid = FileNodeXe.GetAttribute("id");
            canName = FileNodeXe.GetAttribute("caption");
            foreach (XmlNode iCanVar in ICANNode.ChildNodes)
            {
                TCANVarible newCanV = new TCANVarible();
                CanBlList.Add(newCanV);
                newCanV.CaseNode(iCanVar);
            }

        }
    }

    //TSendCAN尚未确定对应的功能实现 @Deer
    public class TSendCAN
    {
        private XmlNode fileNode;
        private string cANID;
        private string sendVAlue;
        private int waitTime;
        public string CANID
        {
            get
            {
                return cANID;
            }
            set
            {
                cANID = value;
                if (fileNode != null)
                {
                    ((XmlElement)fileNode).SetAttribute("id", value);
                }
            }
        }
        public string SendValue
        {
            get { return sendVAlue; }
            set
            {
                sendVAlue = value;
                if (fileNode != null)
                {
                    ((XmlElement)fileNode).SetAttribute("cmd", value);
                }
            }
        }
        public int WaitTime
        {
            get
            {
                return waitTime;
            }
            set
            {
                waitTime = value;
                if (fileNode != null)
                    ((XmlElement)fileNode).SetAttribute("time", value.ToString());
            }
        }
        public void CaseNode(XmlNode iNode)
        {
            fileNode = iNode;
            XmlElement FileNodeXe = (XmlElement)fileNode;
            cANID = FileNodeXe.GetAttribute("id");
            sendVAlue = FileNodeXe.GetAttribute("cmd");
            waitTime = Convert.ToInt32(FileNodeXe.GetAttribute("time"));
        }
    }

    public class TBMS
    {
        private List<TCANID> CanBwList = new List<TCANID>();
        private List<TSendCAN> SendCanList = new List<TSendCAN>();
        private string fileName;
        private XmlDocument xdoc = new XmlDocument();
        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                fileName = value;
                if (File.Exists(value))
                {
                    xdoc.Load(fileName);
                    //0904.hil文件中尚未定义与sendbms和bms相关的XMLNode,BMS相关报文应该是采用另外的格式 @Deer
                    Case(xdoc.OwnerDocument.SelectSingleNode("bms"));
                    CaseSend(xdoc.OwnerDocument.SelectSingleNode("sendbms"));
                }
            }
        }
        /// <summary>
        /// 采集CAN的个数
        /// </summary>
        public int CANCount
        {
            get
            { return CanBwList.Count; }
        }
        /// <summary>
        /// 发送CAN的个数
        /// </summary>
        public int SendCanCout
        {
            get
            {
                return SendCanList.Count;
            }
        }

        private void Case(XmlNode aNode)
        {
            ///采集
            ///定时发送
            XmlNode apNode = aNode;
            foreach (XmlNode inode in apNode.ChildNodes)
            {
                TCANID xCANID = new TCANID();
                xCANID.CaseNode(inode);
                CanBwList.Add(xCANID);
            }
        }
        private void CaseSend(XmlNode aNode)
        {
            foreach (XmlNode inode in aNode.ChildNodes)
            {
                TSendCAN xCANID = new TSendCAN();
                xCANID.CaseNode(inode);
                SendCanList.Add(xCANID);
            }
        }


        //接口作为参数返回，返回的实际上是继承了接口的对象 @Deer
        public iCANID FindCANID(string sStringID)
        {
            return null;
        }

        public iCANVariables FindCANVar(string iVARname)
        {
            return null;
        }

    }

}
