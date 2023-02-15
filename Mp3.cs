using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace musicdecoder
{
    enum FrameType
    {
        ID3V1TAG,
        ID3V2TAG,
        APEV2TAG,
        FRAME,
        INVALID
    }

    public class Mp3
    {
        private FileStream mp3FileStream;

        private List<Mp3Tag> mp3Tags;
        private List<Mp3Frame> mp3Frames;



        public Mp3(string filepath)
        {
            mp3FileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            Mp3Decoder();
            mp3FileStream.Close();
        }

        public void Mp3Decoder()
        {
            int streamoffset = 0;
            while(streamoffset < mp3FileStream.Length)
            {
                FrameType frameType = Tagseeker();
                switch (frameType)
                {
                    
                    case FrameType.ID3V1TAG:
                    case FrameType.ID3V2TAG:
                    case FrameType.APEV2TAG:
                        var mp3Tag = Mp3TagDecoder(frameType);
                        if(mp3Tag != null)
                        {
                            mp3Tags.Add(mp3Tag);
                        }
                        break;
                    case FrameType.FRAME:
                        var mp3Frame = Mp3FrameDecoder();
                        if(mp3Frame != null)
                        {   
                            mp3Frames.Add(mp3Frame);
                        }
                        break;
                    default:
                        break;

                }
            }
        }

        private FrameType Tagseeker()
        {
            return FrameType.INVALID;
        } 

        private Mp3Tag? Mp3TagDecoder(FrameType type)
        {
            return null;
        }

        private Mp3Frame? Mp3FrameDecoder()
        {
            return null;
        }



        class Mp3Tag
        {
            string tagtype = string.Empty;
            ID3v2Tag? iD3v2Tag;
            ID3v1Tag? iD3v1Tag;
            APEV2Tag? apev2Tag;

            public Mp3Tag()
            {

            }

            private void Mp3HeadDecoder(FileStream fs)
            {
                // streamoffset;
                int  n = (int)fs.Length;
                if(n<10)
                {
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
                        return;
                    }
                    if(str.StartsWith("ID3"))
                    {
                        head = new TAGHead(music_bytestream);
                    }
                }
            }

            class ID3v2Tag
            {
                string header{set;get;} = string.Empty;
                int ver{set;get;}
                int revision{set;get;}
                byte flag{set;get;}
                int size{set;get;}

                public ID3v2Tag(byte[] bytes)
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

            class ID3v1Tag
            {

            }

            class APEV2Tag
            {

            }

        }

        private class Mp3Frame
        {

        }


    }
}