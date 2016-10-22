using System.Collections.Generic;

namespace VideoFilesOrganizer.BL
{
    public class VideoFile
    {
        public string Filename { get; private set; }
        public IDictionary<VideoFilePropertyType, string> Properties { get; private set; }

        public VideoFile()
        {
            Properties = new Dictionary<VideoFilePropertyType, string>();
        }

        public void AddProperty(VideoFilePropertyType propType, string propValue)
        {
            Properties.Add(propType, propValue);

            if (propType == VideoFilePropertyType.Filename)
            {
                Filename = propValue;
            }
        }
    }
}
