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
        public SetCanlist setCanlist;
        [XmlElement("setEthList")]
        public setEthList SetEthList;
        [XmlElement("stepList")]
        public stepList StepList;
    }
    public class TSetCan
    {
        [XmlAttribute("Check")]
        public string Check;
        [XmlAttribute("AgreeMentFile")]
        public string AgreeMentFile;
        [XmlAttribute("Baut")]
        public string Baut;
        [XmlAttribute("Alias")]
        public string Alias;
    }
    public class SetCanlist
    {
        [XmlElement("TSetCan")]
        public TSetCan[] TSetCans;
    }
    public class TSetEth
    {
        [XmlAttribute("Check")]
        public string Check;
        [XmlAttribute("IP")]
        public string IP;
        [XmlAttribute("Port")]
        public string Port;
        [XmlAttribute("AgreeMentFile")]
        public string AgreeMentFile;
        [XmlAttribute("Alias")]
        public string Alias;
    }
    public class setEthList
    {
        [XmlElement("TSetEth")]
        public TSetEth[] TSetEths;
    }
    public class subCondition
    {
        [XmlAttribute("IsParent")]
        public string IsParent;
        [XmlAttribute("VarName")]
        public string VarName;
        [XmlAttribute("VarCaption")]
        public string VarCaption;
        [XmlAttribute("Con")]
        public string Con;
        [XmlAttribute("ConValue")]
        public string ConValue;
        [XmlAttribute("Unit")]
        public string Unit;
    }
    public class TCondition
    {
        [XmlAttribute("IsParent")]
        public string IsParent;
        [XmlAttribute("VarName")]
        public string VarName;
        [XmlAttribute("VarCaption")]
        public string VarCaption;
        [XmlAttribute("Con")]
        public string Con;
        [XmlAttribute("ConValue")]
        public string ConValue;
        [XmlAttribute("Unit")]
        public string Unit;
        [XmlElement("subCondition")]
        public subCondition[] subConditions;
    }
    public class judgelist
    {
        [XmlElement("TCondition")]
        public TCondition tcondition;
    }
    public class savelist
    {
        [XmlElement("TCondition")]
        public TCondition[] TConditions;
    }
    public class initlist
    {
        [XmlElement("TCondition")]
        public TCondition[] TConditions;
    }
    public class setlist
    {
        [XmlElement("TCondition")]
        public TCondition[] TConditions;
    }
    public class TCMD
    {
        [XmlElement("initlist")]
        public initlist Initlist;
        [XmlElement("setlist")]
        public setlist Setlist;
        [XmlElement("judgelist")]
        public judgelist Judgelist;
        [XmlElement("savelist")]
        public savelist Savelist;
        [XmlAttribute("Kind")]
        public string Kind;
        [XmlAttribute("Cmd")]
        public string Cmd{ get; set; }
        [XmlAttribute("WaitTime")]
        public string WaitTime{ get; set; }
    }
    public class cmdList
    {
        [XmlElement("TCMD")]
        public TCMD[] TCMDs;
    }
    public class TStep
    {
        [XmlAttribute("testid")]
        public string testid { get; set; }
        [XmlAttribute("title")]
        public string title{ get; set; }
        [XmlAttribute("kind")]
        public string kind;
        [XmlAttribute("Check")]
        public string Check;
        [XmlElement("cmdList")]
        public cmdList CmdList;
    }
    public class stepList
    {
        [XmlElement("TStep")]
        public TStep[] TSteps;
    }
    public class test
    {
        public string testid { get; set; }
        public string title { get; set; }
    }
}
