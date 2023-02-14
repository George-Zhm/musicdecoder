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

        class Mp3Head
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

                if(existence)
                {

                }
                else
                {
                    Console.WriteLine("TAG:ID3 does not exist");
                }
            }

            private void Mp3HeadDecoder(FileStream fs)
            {
                // streamoffset;s
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
                    Console.WriteLine(BitConverter.ToString(music_bytestream));
                    string str = System.Text.Encoding.Default.GetString(music_bytestream);
                    if(r<10)
                    {
                        existence = false;
                        return;
                    }
                    if(str.StartsWith("ID3"))
                    {
                        existence = true;
                        head = new TAGHead(music_bytestream);
                    }
                }
            }

            class TAGHead
            {
                string header{set;get;} = string.Empty;
                int ver{set;get;}
                int revision{set;get;}
                byte flag{set;get;}
                int size{set;get;}

                public TAGHead(byte[] bytes)
                {
                    header = "ID3";
                    ver = (int)bytes[3];
                    revision = (int)bytes[4];
                    flag = bytes[5];
                    size = (((int)bytes[6]&0x7F)<<21)+(((int)bytes[7]&0x7F)<<14)+(((int)bytes[8]&0x7F)<<7)+((int)bytes[9]&0x7F);
                    Console.WriteLine("TAG:");
                    Console.WriteLine("header:"+header);
                    Console.WriteLine("ver:"+ver);
                    Console.WriteLine("revision:"+revision);
                    Console.WriteLine("flag:"+flag);
                    Console.WriteLine("size:"+size);
                }
            }

            class TAGFrame
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