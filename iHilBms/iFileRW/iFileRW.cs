using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using Model;
using System.IO;

namespace iinterface
{
    public interface iFileRW
    {
        void TraverseFolder(TreeListNode ParentNode, ref TreeList treeClass);
        void RemoveFolder(ref TreeList treeClass);
        void TraverseFile(ref TreeList treeClass, ref TreeList treeFile);
        void ImportFolder(ref TreeList treeClass);
        void DeleteFile(ref TreeList treeFile);
        void SaveFile(TSchem schem, string path);
        void FileNameEditorHidden(ref string hiddenEditorType, ref TreeList treeFile, ref TreeList treeClass);
        FileInfo GetFile();
    }
}
