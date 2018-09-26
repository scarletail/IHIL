using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iinterface
{
    public interface iXmlFunction
    {
        void LoadXml(string path, ref TSchem schem);
        void SaveXml(string path, TSchem schem);
    }
}
