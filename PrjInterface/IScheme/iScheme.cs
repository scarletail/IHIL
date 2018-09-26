using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
//如果是项目相关的，区别iinterface ,这个是不区分系统的。
namespace HilInterface
{

    public interface ISchemeManage
    {

        string SchemeFile { get; set; }
        
        void Save(string FileName);
        IScheme iScheme { get; }


    }
    [XmlInclude(typeof(IScheme))]
    
    //方案
    public interface IScheme
    {
        ISetCanList SetCanList { get;  }

        ISetEthList SetEthList { get;  }

        IStepList StepList { get;  }

    }
    // cany设置表
    [XmlInclude(typeof(ISetCanList))]
    public interface ISetCanList
    {
        int Count();
        ISetCan  Item(int index) ;//{ get; set; }


    }
    // 
    [XmlInclude(typeof(ISetCan))]
    public interface ISetCan
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        bool  Check { get; set; }
        /// <summary>
        /// 协议文件
        /// </summary>
        string AgreeMentFile { get; set; }
        /// <summary>
        /// 波特率
        /// </summary>
        int  Baut { get; set; }

        string Alias { get; set; }
    }

    [XmlInclude(typeof(ISetEthList))]
    public interface ISetEthList
    {
        int Count();
        ISetEth Item(int index);
    }
    [XmlInclude(typeof(ISetEth))]

    public interface ISetEth
    {
        string IP { get; set; }
        int  Port { get; set; }
        string AgreeMentFile { get; set; }

        string Alias { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        bool Check { get; set; }
    }
    [XmlInclude(typeof(ICondition))]


    public interface ICondition
    {
       // <conitem value="0" isparent="0" text="脉冲前DTC(2)" blmc="CAN2_bcm_dtc_fault_detected" dw="DTCs" con="=" defaultVal=""/>
        //与条件
        bool IsParent { get; set; }
        //变量ID
        string VarName { get; set; }
        // 变量说明
        string VarCaption { get; set; }
        //符号
        string Con { get; set; }
        //t条件值
        double ConValue { get; set; }
        //单位 
        string Unit { get; set; }


    }
    [XmlInclude(typeof(IConditionList))]
    public interface IConditionList
    {
        int Count();
        ICondition Item(int index);

    }

    [XmlInclude(typeof(ICmd))]

    public interface ICmd
    {

        IConditionList InitList { get; }
        IConditionList SetList { get ; }

        IConditionList JudgeList { get; }

        IConditionList SaveList { get; }


        //类型或关键字
        string Kind { get; set; }
       //指令关键字
        string Cmd { get; set; }
        //等待时间
        int WaitTime { get; set; }

        
    }

    [XmlInclude(typeof(ICmdList))]

    public interface ICmdList
    {
        int Count();
        ICmd Item(int index);

    }

    [XmlInclude(typeof(IStep))]

    public interface IStep
    {
        ICmdList CmdList { get;  }

        string testid { get; set; }

        string title { get; set; }

        string kind { get; set; }

         bool Check { get; set; }

    }

    [XmlInclude(typeof(IStepList ))]

    public interface IStepList
    {
        int Count();
        IStep Step(int index);


    }


}
