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
using System.Xml;

namespace Scheme
{
    public partial class Frmsceme : Form
    {
        /// <summary>
        /// 方案的顶级路径
        /// </summary>
        private string schemeTopPath = "";
        string fileUrl = null;
        string hiddenEditorType = null;
        public Frmsceme()
        {
            InitializeComponent();
            schemeTopPath = Application.StartupPath + @"\category";
        }
        /// <summary>
        /// traverse the folders
        /// </summary>
        /// <param name="ParentNode"></param>
        void TraverseFolder(TreeListNode ParentNode)
        {
            if (ParentNode.Tag == null) return;
            string path = ParentNode.Tag.ToString();
            DirectoryInfo DirInfo = new DirectoryInfo(path);
            if (!DirInfo.Exists) return;
            foreach (DirectoryInfo childFolder in DirInfo.GetDirectories())
            {
                TreeListNode ChildNode = this.treeClass.AppendNode(null, ParentNode);
                ChildNode.SetValue(this.treeClass.Columns["FolderName"], childFolder.Name);
                ChildNode.Tag = childFolder.FullName;
                TraverseFolder(ChildNode);
            }
        }
        public Frmsceme(string spath)
        {
            schemeTopPath = spath;
        }
        /// <summary>
        /// Remove the folder
        /// while treeClass is empty it will throw a NullReferenceException
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //remove the focused folder
            TreeListNode treeListNode = this.treeClass.FocusedNode;
            try
            {
                treeListNode.Remove();
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("can't remove the folder because the list is empty!");
                return;
            }

        }
        /// <summary>
        /// Import a new folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //add folders
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = true;
            //set the default folder path
            folderBrowserDialog.SelectedPath = @"F:\GitSync\IHIL\test";
            folderBrowserDialog.ShowDialog();
            string path = folderBrowserDialog.SelectedPath;
            if (path != null)
            {
                DirectoryInfo info = new DirectoryInfo(path);
                TreeListNode node = treeClass.AppendNode(null, null);
                node.Tag = path;
                node.SetValue(treeClass.Columns["FolderName"], info.Name);
                TraverseFolder(node);
                treeClass.Columns["FolderName"].SortOrder = SortOrder.Ascending;
                //sorting completed!
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
                TreeListNode fileNode = treeFile.AppendNode(null, null);
                fileNode.Tag = childFile.FullName;
                //add node tag to bulid assosication between the filenode and the filepath
                fileNode.SetValue(treeFile.Columns["FileName"], childFile.Name);
            }

        }
        /// <summary>
        /// 获取选中文件夹的路径
        /// no use function
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
                string newfolder = node.GetValue(treeClass.Columns["FolderName"]).ToString() + "\\" + folder;
                return getfolder(node.ParentNode, newfolder);
            }
        }
        /// <summary>
        /// no use function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frmsceme_Load(object sender, EventArgs e)
        {
            //initial operation
            //maybe useless
            ToolHeight();
            TreeListNode FileNode = treeClass.AppendNode(null, null);
            FileNode.SetValue(treeClass.Columns["FolderName"], "scheme");
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
        /// <summary>
        /// sort the nodes by filename when fucused node is changing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeClass_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            treeFile.ClearNodes();
            if (treeClass.AllNodesCount == 0) return;
            try
            {
                TreeListNode node = treeClass.FocusedNode;
                if (node.Tag == null) return;
                string path = node.Tag.ToString();
                TraverseFile(path);
                treeFile.Columns["FileName"].SortOrder = SortOrder.Ascending;
            }
            catch (System.NullReferenceException)
            {
                Console.WriteLine("There is no node focused in the TreeClass!");
            }
        }
        private void treeFile_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            if (treeFile.AllNodesCount == 0) return;
            try
            {
                TreeListNode fnode = treeFile.FocusedNode;
                if (fnode.Tag == null) return;
                fileUrl = fnode.Tag.ToString();
            }
            catch (System.NullReferenceException)
            {
                Console.WriteLine("There is no node focused in the TreeFile!");
                return;
            }

        }
        /// <summary>
        /// create a new xml file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddclass_Click(object sender, EventArgs e)
        {
            try
            {
                if (treeClass.FocusedNode != null)
                {
                    string folderpath = treeClass.FocusedNode.Tag.ToString();
                    TreeListNode newFileNode = treeFile.AppendNode(null, null);
                    newFileNode.Selected = true;
                    hiddenEditorType = "addFile";
                    treeFile.ShowEditor();
                }

            }
            catch (NullReferenceException)
            {
                Console.WriteLine("There're some unreasonable problems!");
                return;
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("File created filed,no such folder!");
                return;
            }

        }
        /// <summary>
        /// add a listener to response the hidden of editor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeFile_HiddenEditor(object sender, EventArgs e)
        {
            if (hiddenEditorType == "addFile")
            {
                TreeListNode currentNode = treeFile.FocusedNode;
                TreeListNode parentNode = treeClass.FocusedNode;
                if (currentNode == null || parentNode == null) return;
                try
                {
                    string filename = currentNode.GetValue(treeFile.Columns["FileName"]).ToString();
                    string filepath = parentNode.Tag.ToString();
                    string fullname = filepath + '\\' + filename;
                    currentNode.Tag = fullname;
                    //Console.WriteLine(fullname);
                    XmlDocument xmlDoc = new XmlDocument();
                    XmlDeclaration declaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes");
                    xmlDoc.AppendChild(declaration);
                    XmlElement RootElement = xmlDoc.CreateElement("ROOT");
                    xmlDoc.AppendChild(RootElement);
                    xmlDoc.Save(fullname);
                }
                catch (NullReferenceException)
                {
                    Console.WriteLine("nothing can describe my mood~~~");
                    return;
                }
                catch (IOException)
                {
                    MessageBox.Show("Failed to create the file!");
                    treeFile.DeleteNode(currentNode);
                    return;
                }
            }
            if (hiddenEditorType == "renameFile")
            {
                TreeListNode node = treeFile.FocusedNode;
                if (node == null) return;
                string sourceFile = node.Tag.ToString();
                string targetFile = node.GetValue(treeFile.Columns["FileName"]).ToString();
                if (sourceFile != null)
                {
                    FileInfo sFile = new FileInfo(sourceFile);
                    DirectoryInfo dirInfo = sFile.Directory;
                    targetFile = dirInfo.FullName + "\\" + targetFile;
                    FileInfo tFile = new FileInfo(targetFile);
                    if (tFile.Exists)
                    {
                        MessageBox.Show("Target file already exists!");
                        node.SetValue(treeFile.Columns["FileName"], sFile.Name);
                    }
                    else
                    {
                        sFile.MoveTo(targetFile);
                    }
                }
            }
        }
        /// <summary>
        /// delete the node in the treeFile and delete the local file in the meantime
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelClass_Click(object sender, EventArgs e)
        {
            try
            {
                TreeListNode deleteNode = treeFile.FocusedNode;
                string deleteFile = deleteNode.Tag.ToString();
                treeFile.DeleteNode(deleteNode);
                if (File.Exists(deleteFile))
                {
                    File.Delete(deleteFile);
                }
            }
            catch (NullReferenceException)
            {
                return;
            }
            catch (IOException)
            {
                return;
            }
        }
        /// <summary>
        /// create a dialog
        /// and copy the current file to a new file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            TreeListNode node = treeClass.FocusedNode;
            TreeListNode fnode = treeFile.FocusedNode;
            string path = null;
            string name = null;
            if (node != null && fnode != null)
            {
                path = node.Tag.ToString();
                name = fnode.GetValue(treeFile.Columns["FileName"]).ToString();
            }
            dialog.OverwritePrompt = true;
            dialog.InitialDirectory = path;
            dialog.FileName = name;
            dialog.Filter = "All files|*.*|Xml file|*.xml";
            dialog.ShowDialog();
            string sourceFile = path + "//" + name;
            string targetFile = null;
            if (dialog.FileName != "")
            {
                //FileStream fs = (FileStream)dialog.OpenFile();
                targetFile = dialog.FileName;
                File.Copy(sourceFile, targetFile);
            }
        }
        /// <summary>
        /// rename the focused file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReName_Click(object sender, EventArgs e)
        {
            TreeListNode fileNode = treeFile.FocusedNode;
            if (fileNode == null) return;
            fileNode.Selected = true;
            hiddenEditorType = "renameFile";
            treeFile.ShowEditor();
        }
    }
}
