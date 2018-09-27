using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;

namespace MEF
{
    public class MEF
    {
    }



    public class CreateNewDllInstance
    {
        //static string tag = "";//"Afternoon""Morning1"
        string path = "";//OutPut
        public CreateNewDllInstance(string _path)
        {
            path =  _path;//路径
            Compose(); //组装 
        }

        CompositionContainer container;
        /// <summary>  
        /// 通过容器对象将宿主和部件组装到一起。   
        /// </summary>  
        private void Compose()
        {
            //设置目录(Catalog),为了在目录所有程序集(*.dll)搜寻通过[Export]注册的导出部件 @Deer
            //目录采用一次性扫描，不会随着后续文件变动而更改，如需更改请使用"catalog.Refresh()" @Deer
            var catalog = new DirectoryCatalog(path); 
            //将目录传入容器(container) @Deer
            container = new CompositionContainer(catalog);
            //声明当前组件不会成为导出组件 @Deer
            container.SatisfyImportsOnce(this);
        }

        //返回一个dll的实例中的具体一个对象
        public T CreateByContainer<T>(string classname)
        {
            return container.GetExportedValue<T>(classname);
            //通过 var classname = container.GetExportedValue<T>(classname)的形式来调用 @Deer
            //classname即为注册了[Export]的导出部件
            //示例：

            //定义接口：
            //public interface iFunction
            //{
            //    int Add(int a, int b)
            //}

            //定义类：
            //[Export("TAdd"),typeof(iFunction)]
            //[PartCreationPolicy(CreationPolicy.NonShared)]
            //public class TAdd : iFunction
            //{
            //  public int Add(int a, int b)
            //    {
            //        return a + b;
            //    }
            //}

            //将以上代码生成dll文件

            //var catalog = new DirectoryCatalog(DllDirectory);
            //container = new CompositionContainer(catalog);
            //var classname = container.GetExportedValue<iFunction>("TAdd");
            //Console.WriteLine(classname.Add(1,2));
            //Console.ReadKey();
        }
    }

}
