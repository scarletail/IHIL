using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Scheme
{
    public class TSchem
    {
        [XmlElement("setCanlist")]
        public SetCanlist setCanlist{ get; set; }
        [XmlElement("setEthList")]
        public setEthList SetEthList { get; set; }
        [XmlElement("stepList")]
        public stepList StepList { get; set; }
        public TSchem()
        {
            setCanlist = new SetCanlist();
            SetEthList = new setEthList();
            StepList = new stepList();
        }
    }
    public class TSetCan
    {
        [XmlAttribute("Check")]
        public string Check { get; set; }
        [XmlAttribute("AgreeMentFile")]
        public string AgreeMentFile { get; set; }
        [XmlAttribute("Baut")]
        public string Baut { get; set; }
        [XmlAttribute("Alias")]
        public string Alias { get; set; }
        public TSetCan()
        {
            Check = "";
            AgreeMentFile = "";
            Baut = "";
            Alias = "";
        }
    }
    public class SetCanlist
    {
        [XmlElement("TSetCan")]
        public List<TSetCan> TSetCans { get; set; }
        public SetCanlist()
        {
            TSetCans = new List<TSetCan>();
        }
    }
    public class TSetEth
    {
        [XmlAttribute("Check")]
        public string Check { get; set; }
        [XmlAttribute("IP")]
        public string IP { get; set; }
        [XmlAttribute("Port")]
        public string Port { get; set; }
        [XmlAttribute("AgreeMentFile")]
        public string AgreeMentFile { get; set; }
        [XmlAttribute("Alias")]
        public string Alias { get; set; }
        public TSetEth()
        {
            Check = "";
            IP = "";
            Port = "";
            AgreeMentFile = "";
            Alias = "";
        }
    }
    public class setEthList
    {
        [XmlElement("TSetEth")]
        public List<TSetEth> TSetEths { get; set; }
        public setEthList()
        {
            TSetEths = new List<TSetEth>();
        }
    }
    public class subCondition
    {
        [XmlAttribute("IsParent")]
        public string IsParent { get; set; }
        [XmlAttribute("VarName")]
        public string VarName { get; set; }
        [XmlAttribute("VarCaption")]
        public string VarCaption { get; set; }
        [XmlAttribute("Con")]
        public string Con { get; set; }
        [XmlAttribute("ConValue")]
        public string ConValue { get; set; }
        [XmlAttribute("Unit")]
        public string Unit { get; set; }
        public subCondition()
        {
            IsParent = "";
            VarName = "";
            VarCaption = "";
            Con = "";
            ConValue = "";
            Unit = "";
        }
    }
    public class TCondition
    {
        [XmlAttribute("IsParent")]
        public string IsParent { get; set; }
        [XmlAttribute("VarName")]
        public string VarName { get; set; }
        [XmlAttribute("VarCaption")]
        public string VarCaption { get; set; }
        [XmlAttribute("Con")]
        public string Con { get; set; }
        [XmlAttribute("ConValue")]
        public string ConValue { get; set; }
        [XmlAttribute("Unit")]
        public string Unit { get; set; }
        [XmlElement("subCondition")]
        public List<subCondition> subConditions { get; set; }
        public TCondition()
        {
            IsParent = "";
            VarName = "";
            VarCaption = "";
            Con = "";
            ConValue = "";
            Unit = "";
            subConditions = new List<subCondition>();
        }
    }
    public class judgelist
    {
        [XmlElement("TCondition")]
        public List<TCondition> tconditions { get; set; }
        public judgelist()
        {
            tconditions = new List<TCondition>();
        }
    }

    public class savelist
    {
        [XmlElement("TCondition")]
        public List<TCondition> TConditions { get; set; }
        public savelist()
        {
            TConditions = new List<TCondition>();
        }
    }
    public class initlist
    {
        [XmlElement("TCondition")]
        public List<TCondition> TConditions { get; set; }
        public initlist()
        {
            TConditions = new List<TCondition>();
        }
    }
    public class setlist
    {
        [XmlElement("TCondition")]
        public List<TCondition> TConditions { get; set; }
        public setlist()
        {
            TConditions = new List<TCondition>();
        }
    }
    public class TCMD
    {
        [XmlElement("initlist")]
        public initlist Initlist { get; set; }
        [XmlElement("setlist")]
        public setlist Setlist { get; set; }
        [XmlElement("judgelist")]
        public judgelist Judgelist { get; set; }
        [XmlElement("savelist")]
        public savelist Savelist { get; set; }
        [XmlAttribute("Kind")]
        public string Kind { get; set; }
        [XmlAttribute("Cmd")]
        public string Cmd{ get; set; }
        [XmlAttribute("WaitTime")]
        public string WaitTime{ get; set; }
        public TCMD()
        {
            Initlist = new initlist();
            Setlist = new setlist();
            Judgelist = new judgelist();
            Savelist = new savelist();
            Kind = "";
            Cmd = "";
            WaitTime = "";
        }
    }
    public class cmdList
    {
        [XmlElement("TCMD")]
        public List<TCMD> TCMDs { get; set; }
        public cmdList()
        {
            TCMDs = new List<TCMD>();
        }
    }
    public class TStep
    {
        [XmlAttribute("testid")]
        public string testid { get; set; }
        [XmlAttribute("title")]
        public string title{ get; set; }
        [XmlAttribute("kind")]
        public string kind { get; set; }
        [XmlAttribute("Check")]
        public string Check { get; set; }
        [XmlElement("cmdList")]
        public cmdList CmdList { get; set; }
        public TStep()
        {
            testid = "";
            title = "";
            kind = "";
            Check = "";
            CmdList = new cmdList();
        }
    }
    public class stepList
    {
        [XmlElement("TStep")]
        public List<TStep> TSteps { get; set; }
        public stepList()
        {
            TSteps = new List<TStep>();
        }
    }
    public class test
    {
        public string testid { get; set; }
        public string title { get; set; }
        public test()
        {
            testid = "";
            title = "";
        }
    }
}
