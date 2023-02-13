using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace musicdecoder
{

    public class Mp3
    {
        private FileStream mp3FileStream;

        private Mp3Head? mp3Head;
        private List<Mp3Frame> mp3Frames;
        private Mp3Tail? mp3Tail;

        protected static int streamoffset = 0;

        public Mp3(string filepath)
        {
            mp3FileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            Mp3Decoder();
        }

        public void Mp3Decoder()
        {
            mp3Head = new Mp3Head(mp3FileStream);
 
        }



        private void Mp3FrameDecoder()
        {

        }

        private void Mp3TailDecoder()
        {

        }

        private class Mp3Head
        {
            bool existence = false;
            TAGHead head;

            public Mp3Head(FileStream fileStream)
            {
                if(fileStream!=null)
                {
                    Mp3HeadDecoder(fileStream);
                }
                else
                {
                    existence = false;
                }
            }

            private void Mp3HeadDecoder(FileStream fs)
            {
                streamoffset;
                int  n = (int)fs.Length;
                if(n<10)
                {
                    existence = false;
                    return;
                }
                else
                {
                    byte[] music_bytestream = new byte[10];
                    int r = fs.Read(music_bytestream,0,10);
                }
            }

            private class TAGHead
            {
                string? header{set;get;}
                char? ver{set;get;}
                char? revision{set;get;}
                char? flag{set;get;}
                int size{set;get;}
            }

            private class TAGFrame
            {

            }
        }

        private class Mp3Frame
        {

        }

        private class Mp3Tail
        {

        }

    }
}