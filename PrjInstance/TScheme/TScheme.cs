using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HilInterface;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using XMLSerialize;
using System.ComponentModel.Composition;
namespace HILInstance
{


   [Export("TSchemeManage", typeof(ISchemeManage))]
   [PartCreationPolicy(CreationPolicy.NonShared)]

   //提供了部分属性以及序列化反序列化的方法 @Deer
    public class TSchemeManage:ISchemeManage
    {
        private string schemeFile;
        private TSchem Scheme ;//= new TSchem();

        public string SchemeFile
        {
            get
            {
                return schemeFile;
            }
            set 
            {
                schemeFile = value;
                Scheme = XMLSerialize.xmlSerialize.DeSerialization<TSchem>(schemeFile);  
            }


        }

        public void Save(string FileName)
        {
            if (FileName =="") 
                FileName = schemeFile;

            if (FileName != "")
                xmlSerialize.Serialization<TSchem>(Scheme, FileName);
        }
        public IScheme iScheme { get { return Scheme; } }
    }

    //方案

    [Export("TSchem", typeof(IScheme))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Serializable]
    public class TSchem:IScheme
    {
        public TSchem() { }
       // private string schemeFile;
        //private XmlDocument SchemeXml = new XmlDocument();
        
        public TSetCanList setCanlist = new TSetCanList();
       
        public TSetEthList setEthList = new TSetEthList();
        
        public TStepList stepList = new TStepList();
        public ISetCanList SetCanList { get { return setCanlist; } }

        public ISetEthList SetEthList { get {return setEthList;} }

        public IStepList StepList { get { return stepList; } }



    }
    [Export("TSetCanList", typeof(ISetCanList))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Serializable]

    // cany设置表
    public class TSetCanList: ISetCanList
    {
        public TSetCanList() { }
        [XmlElementAttribute(ElementName = "TSetCan", IsNullable = false)]
        public List<TSetCan> setCanList=new List<TSetCan> ();
        public int Count() { 
            return setCanList.Count(); }
        
        public ISetCan Item(int index)//{ get; set; }
        {
            return setCanList[index];
        }

    }

    [Export("TSetCan", typeof(ISetCan))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Serializable]
    // 
    public class TSetCan:ISetCan
    {

      //  <TSetCan Check="-1" AgreeMentFile="20151215-hm1-IP34_-WX-EP_BMS_HSCAN6-V04.bm" Baut="2" Alias=""/>
        public TSetCan() { }
         
        [XmlAttribute]
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Check { get; set; }
        [XmlAttribute]
        /// <summary>
        /// 协议文件
        /// </summary>
        public string AgreeMentFile { get; set; }
        [XmlAttribute]
        /// <summary>
        /// 波特率
        /// </summary>
        public int Baut { get; set; }
        [XmlAttribute]
        public string Alias { get; set; }
    }
    [Export("TSetEthList", typeof(ISetEthList))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Serializable]

    public class TSetEthList: ISetEthList
    {
        public TSetEthList() { }
         [XmlElementAttribute(ElementName = "TSetEth", IsNullable = false)]
        public List<TSetEth> setEthList = new List<TSetEth>();
        public int Count()
        {
            { return setEthList.Count(); }
        }
        public ISetEth Item(int index)
        {
            return setEthList[index];
        }
    }
    [Export("TSetEth", typeof(ISetEth))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Serializable]
    public class TSetEth: ISetEth
    {
        public TSetEth() { }
        [XmlAttribute]
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Check { get; set; }

        [XmlAttribute]
        public string IP { get; set; }
         [XmlAttribute]
        public int Port { get; set; }
         [XmlAttribute]
         public string AgreeMentFile { get; set; }
         [XmlAttribute]
        public string  Alias { get; set; }
    }

    [Export("TSubCondition", typeof(ICondition))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Serializable]
    public class TSubCondition:ICondition
    {

        public TSubCondition() { }
        [XmlAttribute]
        //与条件
        public bool IsParent { get; set; }
        [XmlAttribute]
        //变量ID
        public string VarName { get; set; }
        [XmlAttribute]
        // 变量说明
        public string VarCaption { get; set; }
        [XmlAttribute]
        //符号
        public string Con { get; set; }
        [XmlAttribute]
        //t条件值
        public double ConValue { get; set; }
        [XmlAttribute]
        //单位 
        public string Unit { get; set; }

        [XmlIgnore]
        public double CalcValue { get; set; }
    }


    [Export("TCondition", typeof(ICondition))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Serializable]
    public class TCondition :ICondition
    {
        public TCondition() { }
        [XmlElementAttribute(ElementName = "subCondition", IsNullable = false)]
        public List<TSubCondition> subCondition = new List<TSubCondition>();
        //public List<TCondition> conditionList = new List<TCondition>();
        // <conitem value="0" isparent="0" text="脉冲前DTC(2)" blmc="CAN2_bcm_dtc_fault_detected" dw="DTCs" con="=" defaultVal=""/>
         [XmlAttribute]
        //与条件
        public bool IsParent { get; set; }
         [XmlAttribute]
        //变量ID
        public string VarName { get; set; }
         [XmlAttribute]
        // 变量说明
        public string VarCaption { get; set; }
         [XmlAttribute]
        //符号
        public string Con { get; set; }
         [XmlAttribute]
        //t条件值
        public double ConValue { get; set; }
         [XmlAttribute]
        //单位 
        public string Unit { get; set; }
         [XmlIgnore]
         public double CalcValue { get; set; }

    }
    [Export("TConditionList", typeof(IConditionList))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Serializable]
    public class TConditionList: IConditionList
    {
        public TConditionList() { }
       [XmlElementAttribute(ElementName = "TCondition", IsNullable = false)]
        public List<TCondition> conditionList = new List<TCondition>();
        public int Count()
        {
            return conditionList.Count();
        }
        public ICondition Item(int index)
        {
            return conditionList[index];
        }

    }
    [Export("TCMD", typeof(ICmd))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Serializable]
    public class TCMD: ICmd
    {
        public TConditionList initlist = new TConditionList();
        public TConditionList setlist = new TConditionList();
        public TConditionList judgelist = new TConditionList();
        public TConditionList savelist = new TConditionList();

        public IConditionList InitList { get { return initlist; } }
        public IConditionList SetList { get { return setlist; } }

        public IConditionList JudgeList { get { return judgelist; } }

        public IConditionList SaveList { get { return savelist; } }
        public TCMD() { }
        [XmlAttribute]
        //类型或关键字
        public string Kind { get; set; }
         [XmlAttribute]
        //指令关键字
        public string Cmd { get; set; }
         [XmlAttribute]
        //等待时间
        public int WaitTime { get; set; }


    }

    [Export("TCmdList", typeof(ICmdList))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Serializable]
    public class    TCmdList: ICmdList
    {
        public TCmdList() { }
        [XmlElementAttribute(ElementName = "TCMD", IsNullable = false)]
        public List<TCMD> cmdList = new List<TCMD>();
        public  int Count()
        {
            return cmdList.Count();
        }
        public ICmd Item(int index)
        {
            return cmdList[index];
        }

    }
    [Export("TStep", typeof(IStep))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Serializable]
    public class TStep: IStep
    {
        public TStep() { }
        public TCmdList cmdList = new TCmdList();
        public ICmdList CmdList { get { return cmdList; } }
        [XmlAttribute]

        public string testid { get; set; }
        [XmlAttribute]
        public string title { get; set; }
        [XmlAttribute]
        public string kind { get; set; }
        [XmlAttribute]
        public bool Check { get; set; }


    }
    [Export("TStepList", typeof(IStepList))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Serializable]
    public class TStepList: IStepList
    {
        public TStepList() { }
        [XmlElementAttribute(ElementName = "TStep", IsNullable = false)]
        public List<TStep> stepList = new List<TStep>();

        public int Count ()
        {
             return stepList.Count(); 
        }
        public IStep Step(int index)
        {
            return stepList[index];
        }


    }


}
