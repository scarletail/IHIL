using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scheme
{

    class MenuUtils
    {

        public void MoveRowUp(Object o, ref GridView gridview)
        {
            int i = gridview.GetSelectedRows()[0];
            BindingSource bs = (BindingSource)gridview.DataSource;
            List<Object> list = (List<Object>)bs.DataSource;
            Object temp = list[i - 1];
            list[i - 1] = list[i];
            list[i] = temp;
        }
        public void MoveRowDn(Object o, ref GridView gridview)
        {
            int i = gridview.GetSelectedRows()[0];
            BindingSource bs = (BindingSource)gridview.DataSource;
            List<Object> list = (List<Object>)bs.DataSource;
            Object temp = list[i + 1];
            list[i + 1] = list[i];
            list[i] = temp;
        }
    }
}
