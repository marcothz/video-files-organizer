using System;
using System.Collections.Generic;
using Shell32;
using System.Reflection;

namespace VideoFilesOrganizer.BL
{
    public class VideoFileLoader
    {
        public static IEnumerable<VideoFile> GetVideoFilesWithProperties(string path)
        {
            var videoFiles = new List<VideoFile>();
            var shellType = Type.GetTypeFromProgID("Shell.Application");
            var shell = Activator.CreateInstance(shellType);
            var folder = (Folder)shellType.InvokeMember("NameSpace", BindingFlags.InvokeMethod, null, shell, new object[] { path });

            foreach (var folderItem in folder.Items())
            {
                var videoFile = new VideoFile();

                var perceivedType = folder.GetDetailsOf(folderItem, (int)VideoFilePropertyType.PerceivedType);

                if(perceivedType.ToLower() == "video")
                {
                    foreach (VideoFilePropertyType propType in Enum.GetValues(typeof(VideoFilePropertyType)))
                    {
                        var propValue = folder.GetDetailsOf(folderItem, (int)propType);

                        if (!string.IsNullOrEmpty(propValue))
                        {
                            videoFile.AddProperty(propType, propValue);
                        }
                    }

                    videoFiles.Add(videoFile);
                }
            }

            return videoFiles;
        }
    }
}
