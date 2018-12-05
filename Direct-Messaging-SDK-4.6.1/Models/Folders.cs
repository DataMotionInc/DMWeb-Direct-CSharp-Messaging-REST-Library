using System.Collections.Generic;

namespace DMWeb_REST.Models
{
    public class Folders
    {
        /// <summary>
        /// Structure for Folder object
        /// </summary>
        public class Create
        {
            public int FolderId { get; set; }
            public string FolderName { get; set; }
            public int FolderType { get; set; }
            public string FolderTypeDescription { get; set; }
            public bool IsSystemFolder { get; set; }
            public int TotalMessages { get; set; }
            public int TotalSize { get; set; }
        }

        public class FolderResponse
        {
            public int FolderId { get; set; }
        }

        public class ListFolders
        {
            public List<Create> Folders = new List<Create>();
        }
    }
}
