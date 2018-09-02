using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraTreeList.Nodes;

using DevExpress.XtraTreeList;


using System.IO;
using System.Collections;

namespace Scheme
{
    public partial class Frmsceme : Form
    {
        /// <summary>
        /// 方案的顶级路径
        /// </summary>
        private string schemeTopPath = "";
        string fileUrl = null;
        public Frmsceme()
        {
            InitializeComponent();
            schemeTopPath = Application.StartupPath + @"\category";
        }


        /// <summary>
        /// 遍历文件夹
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="ParentNode"></param>
        void TraverseFolder(TreeListNode ParentNode)
        {
            //overwrite the logic of function  
            if (ParentNode.Tag == null) return;
            string path = ParentNode.Tag.ToString();
            DirectoryInfo DirInfo = new DirectoryInfo(path); //文件夹信息
            if (!DirInfo.Exists) return;
            foreach (DirectoryInfo childFolder in DirInfo.GetDirectories())//获取文件夹的子文件夹
            {
                TreeListNode ChildNode = this.treeClass.AppendNode(null, ParentNode);
                ChildNode.SetValue(this.treeClass.Columns["FolderName"], childFolder.Name);
                TraverseFolder(ChildNode);
            }
        }

        public Frmsceme(string spath)
        {
            schemeTopPath = spath;


        }

        private void button2_Click(object sender, EventArgs e)
        {
            //remove the focused folder
            TreeListNode treeListNode = this.treeClass.FocusedNode;
            treeListNode.Remove();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //add folders
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = true;
            //set the default folder path
            //folderBrowserDialog.SelectedPath = schemeTopPath;
            folderBrowserDialog.ShowDialog();
            string path = folderBrowserDialog.SelectedPath;
            if (path != null)
            {
                DirectoryInfo info = new DirectoryInfo(path);
                TreeListNode node = this.treeClass.AppendNode(null, null);
                node.Tag = path;
                node.SetValue(this.treeClass.Columns["FolderName"], info.Name);
                TraverseFolder(node);
            }
        }

        /// <summary>
        /// 遍历文件
        /// </summary>
        /// <param name="filepath"></param>
        private void TraverseFile(string filepath)
        {
            //this.clearAll();
            //this.currentpackeol = null;
            DirectoryInfo TheFolder = new DirectoryInfo(filepath);
            if (!TheFolder.Exists) return;
            foreach (FileInfo childFile in TheFolder.GetFiles())
            {
                TreeListNode fileNode = this.treeFile.AppendNode(null, null);
                fileNode.Tag = childFile.FullName;
                //add node tag to bulid assosication between the filenode and the filepath
                fileNode.SetValue(this.treeFile.Columns["FileName"], childFile.Name);
            }

        }
        /// <summary>
        /// 获取选中文件夹的路径
        /// </summary>
        /// <param name="node"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        private string getfolder(TreeListNode node, string folder)
        {
            //useless function
            if (node.ParentNode == null)
            {
                return this.schemeTopPath + folder;
            }
            else
            {
                string newfolder = node.GetValue(this.treeClass.Columns["FolderName"]).ToString() + "\\" + folder;
                return getfolder(node.ParentNode, newfolder);
            }
        }

        private void Frmsceme_Load(object sender, EventArgs e)
        {
            //initial operation
            //maybe useless
            ToolHeight();
            TreeListNode FileNode = this.treeClass.AppendNode(null, null);
            FileNode.SetValue(this.treeClass.Columns["FolderName"], "scheme");
            TraverseFolder(FileNode);
            treeClass.FocusedNode = FileNode;
            TraverseFile(getfolder(FileNode, ""));
        }

        private void ToolHeight()
        {
            scemePanel.Height = toolboxControl1.Height - 39;
        }
        private void Frmsceme_Resize(object sender, EventArgs e)
        {
            ToolHeight();
        }

        private void tabNNarmalTest_Paint(object sender, PaintEventArgs e)
        {

        }

        private void contextMenuStripResult_Opening(object sender, CancelEventArgs e)
        {

        }

        private void buttonResult_Click(object sender, EventArgs e)
        {


        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void treeClass_AfterExpand(object sender, NodeEventArgs e)
        {

        }

        private void treeClass_RowStateImageClick(object sender, RowClickEventArgs e)
        {

        }

        private void treeClass_RowSelectImageClick(object sender, RowClickEventArgs e)
        {

        }

        private void treeClass_CustomDrawNodeButton(object sender, CustomDrawNodeButtonEventArgs e)
        {

        }

        private void treeClass_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            treeFile.ClearNodes();
            try
            {
                TreeListNode node = treeClass.FocusedNode;
                if (node.Tag == null) return;
                string path = node.Tag.ToString();
                treeFile.AppendNode("begin", null);
                TraverseFile(path);
                treeFile.AppendNode("end", null);
            }
            catch (System.NullReferenceException)
            {
                Console.WriteLine("System.NullReferenceException has occured!");
            }
        }

        private void treeFile_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            try
            {
                TreeListNode fnode = treeFile.FocusedNode;
                if (fnode.Tag == null) return;
                fileUrl = fnode.Tag.ToString();
            }
            catch (System.NullReferenceException)
            {
                Console.WriteLine("System.NullReferenceException has occured!");
                return;
            }
            
        }
    }
}
