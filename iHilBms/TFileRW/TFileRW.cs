using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using iinterface;
using Model;
using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace TFileRW
{
    [Export("TFileRW", typeof(iFileRW))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class TFileRW: iFileRW
    {
        /// <summary>
        /// 遍历文件夹
        /// </summary>
        /// <param name="ParentNode">文件夹父节点</param>
        /// <param name="treeClass">引用传递treeClass控件句柄</param>
        public void TraverseFolder(TreeListNode ParentNode, ref TreeList treeClass)
        {
            if (ParentNode.Tag == null) return;
            string path = ParentNode.Tag.ToString();
            DirectoryInfo DirInfo = new DirectoryInfo(path);
            if (!DirInfo.Exists) return;
            foreach (DirectoryInfo childFolder in DirInfo.GetDirectories())
            {
                TreeListNode ChildNode = treeClass.AppendNode(null, ParentNode);
                ChildNode.SetValue(treeClass.Columns["FolderName"], childFolder.Name);
                ChildNode.Tag = childFolder.FullName;
                TraverseFolder(ChildNode, ref treeClass);
            }
        }
        /// <summary>
        /// 移除当前获得焦点的结点对应的文件夹
        /// </summary>
        /// <param name="treeClass">文件夹列表控件的引用传递</param>
        public void RemoveFolder(ref TreeList treeClass)
        {
            if (treeClass.FocusedNode == null)
            {
                MessageBox.Show("There're no folder in the list", "Warning");
                return;
            }
            treeClass.FocusedNode.Remove();
        }
        /// <summary>
        /// 遍历文件夹下的文件
        /// </summary>
        /// <param name="treeClass">文件夹列表控件的引用传递</param>
        /// <param name="treeFile">文件列表控件的引用传递</param>
        public void TraverseFile(ref TreeList treeClass, ref TreeList treeFile)
        {
            if (treeFile.AllNodesCount != 0) treeFile.ClearNodes();
            if (treeClass.FocusedNode == null) return;
            try
            {
                string filepath = treeClass.FocusedNode.Tag.ToString();
                DirectoryInfo TheFolder = new DirectoryInfo(filepath);
                if (!TheFolder.Exists) return;
                if (treeFile.AllNodesCount != 0) treeFile.ClearNodes();
                foreach (FileInfo childFile in TheFolder.GetFiles())
                {
                    TreeListNode fileNode = treeFile.AppendNode(null, null);
                    fileNode.Tag = childFile.FullName;
                    fileNode.SetValue(treeFile.Columns["FileName"], childFile.Name);
                }
                treeFile.Columns["FileName"].SortOrder = SortOrder.Ascending;
            }
            catch (NullReferenceException)
            {
                return;
            }
        }
        /// <summary>
        /// 导入文件夹
        /// </summary>
        /// <param name="treeClass">文件夹列表控件的引用传递</param>
        public void ImportFolder(ref TreeList treeClass)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = true;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string path = folderBrowserDialog.SelectedPath;
                DirectoryInfo info = new DirectoryInfo(path);
                TreeListNode node = treeClass.AppendNode(null, null);
                node.Tag = path;
                node.SetValue(treeClass.Columns["FolderName"], info.Name);
                //TraverseFolder(node);
                TraverseFolder(node, ref treeClass);
                treeClass.Columns["FolderName"].SortOrder = SortOrder.Ascending;
            }
        }
        /// <summary>
        /// 删除文件结点及文件
        /// </summary>
        /// <param name="treeFile">文件列表控件的引用传递</param>
        public void DeleteFile(ref TreeList treeFile)
        {
            try
            {
                TreeListNode deleteNode = treeFile.FocusedNode;
                string deleteFile = deleteNode.Tag.ToString();
                if (File.Exists(deleteFile))
                {
                    File.Delete(deleteFile);
                    treeFile.DeleteNode(deleteNode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
                return;
            }
        }
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="schem">存储测试相关信息的类</param>
        /// <param name="path">文件保存路径</param>
        public void SaveFile(TSchem schem, string path)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(schem.GetType());
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = Encoding.GetEncoding("gb2312");
                settings.Indent = true;
                XmlWriter writer = XmlWriter.Create(path, settings);
                ns.Add("", "");
                xs.Serialize(writer, schem, ns);
                writer.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }
        /// <summary>
        /// 编辑框关闭之后调用此方法完成后续工作
        /// </summary>
        /// <param name="hiddenEditorType">编辑类型"addFile"或"renameFile"</param>
        /// <param name="treeFile">文件列表控件的引用传递</param>
        /// <param name="treeClass">文件夹列表控件的引用传递</param>
        public void FileNameEditorHidden(ref string hiddenEditorType, ref TreeList treeFile, ref TreeList treeClass)
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
                    XmlDeclaration declaration = xmlDoc.CreateXmlDeclaration("1.0", "gb2312", "yes");
                    xmlDoc.AppendChild(declaration);
                    XmlElement RootElement = xmlDoc.CreateElement("chHIL");
                    xmlDoc.AppendChild(RootElement);
                    xmlDoc.Save(fullname);
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("file name cannot be empty!", "Warning!");
                    currentNode.Remove();
                }
                catch (IOException)
                {
                    MessageBox.Show("Failed to create the file!", "Error");
                    currentNode.Remove();
                }
                finally
                {
                    hiddenEditorType = null;
                }
                return;
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
                    if (sourceFile == targetFile) return;
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
                hiddenEditorType = null;
                return;
            }
        }
        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <returns>文件信息</returns>
        public FileInfo GetFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            FileInfo info;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string path = Path.GetFullPath(dialog.FileName);
                    info = new FileInfo(path);
                }
                catch (ArgumentException)
                {
                    info = null;
                }
            }
            else
            {
                info = null;
            }
            return info;
        }
    }
}
