using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoFilesOrganizer.BL
{
    public class VideoFileRenamer
    {
        public static void Rename(IEnumerable<VideoFile> files, string outputDir)
        {
            var filesToRename = CreateRenamingFileList(files, outputDir);

            ResolveDuplicates(filesToRename);

            filesToRename.ToList().ForEach(file => File.Move(file.OriginalFileName, file.TemporaryFileName));
            filesToRename.ToList().ForEach(file => File.Move(file.TemporaryFileName, file.NewFileName));
        }

        private static IEnumerable<RenamingFile> CreateRenamingFileList(IEnumerable<VideoFile> files, string outputDir)
        {
            var filesToRename = new List<RenamingFile>();

            foreach(var file in files)
            {
                var fileToRename = new RenamingFile()
                {
                    OriginalFileName = file.Properties[VideoFilePropertyType.Path],
                    TemporaryFileName = Path.Combine(outputDir, string.Format("{0}{1}", Guid.NewGuid().ToString(), file.Properties[VideoFilePropertyType.FileExtension])),
                    NewFileName = Path.Combine(outputDir,
                        string.Format("Video-{0:yyyy-MM-dd-HH-mm-ss}{1}",
                            DateTime.Parse(file.Properties[VideoFilePropertyType.DateModified]),
                            file.Properties[VideoFilePropertyType.FileExtension]))
                };

                filesToRename.Add(fileToRename);
            }

            return filesToRename;
        }

        private static void ResolveDuplicates(IEnumerable<RenamingFile> files)
        {
            var temp = new Dictionary<string, string>();

            files.ToList().ForEach(file =>
            {
                while (temp.ContainsKey(file.NewFileName))
                {
                    file.NewFileName = GetAlternativeName(file.NewFileName);
                }

                temp.Add(file.NewFileName, file.NewFileName);
            });
        }

        private static string GetAlternativeName(string originaName)
        {
            var path = Path.GetDirectoryName(originaName);
            var fileName = Path.GetFileNameWithoutExtension(originaName);
            var extension = Path.GetExtension(originaName);

            var parts = fileName.Split('-');
            var suffix = ' ';

            if (parts.Length == 8)
            {
                suffix = parts[7][0];
            }

            suffix = IncrementSuffix(suffix);

            return Path.Combine(path,
                string.Format("{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}{8}",
                    parts[0],
                    parts[1],
                    parts[2],
                    parts[3],
                    parts[4],
                    parts[5],
                    parts[6],
                    suffix,
                    extension));
        }

        private static char IncrementSuffix(char suffix)
        {
            if (suffix == ' ')
            {
                return 'a';
            }
            else
            {
                return (char)(suffix + 1);
            }
        }

        private class RenamingFile
        {
            public string _TemporaryFileName;

            public string OriginalFileName { get; set; }
            public string TemporaryFileName
            {
                get
                {
                    return _TemporaryFileName;
                }
                set
                {
                    _TemporaryFileName = value;
                }
            }
            public string NewFileName { get; set; }
        }
    }
}
