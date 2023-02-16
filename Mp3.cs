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
        private byte[] rawbytes;
        private List<ID3v1Tag> id3v1Tags;
        private List<ID3v2Tag> id3v2Tags;
        private List<Mp3Frame> mp3Frames;



        public Mp3(string filepath)
        {
            mp3FileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            //reveive complete file
            mp3FileStream.Read(rawbytes,0,mp3FileStream.Length);
            //
            Mp3Decoder();
            mp3FileStream.Close();
        }

        public void Mp3Decoder()
        {
            int streamoffset = 0;
            byte[] filterbytes = new byte[10];
   
            FrameType frameType = FrameType.INVALID;
            while(streamoffset + 11 < mp3FileStream.Length)
            {
                int r = mp3FileStream.Read(filterbytes,streamoffset,10);
                string str = System.Text.Encoding.Default.GetString(filterbytes);
                if(str.StartsWith("ID3"))
                {
                    frameType = FrameType.ID3V2TAG;
                }
                else if(str.StartsWith("TAG"))
                {
                    frameType = FrameType.ID3V1TAG;
                }
                else
                {
                    break;
                }

                switch (frameType)
                {
                    case FrameType.ID3V1TAG:
                        break;
                    case FrameType.ID3V2TAG:
                    {
                        var id3v2Tag = ID3v2Decoder(mp3FileStream,streamoffset);
                        if(id3v2Tag != null)
                        {
                            id3v2Tags.Add(id3v2Tag);
                            streamoffset += id3v2Tag.tagLength;
                        }
                        else
                        {
                            streamoffset++;
                        }
                    }
                    break;
                    case FrameType.APEV2TAG:

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

        private ID3v1Tag? ID3v1Decoder(FileStream fs, int offset)
        {
            return null;
        }

        private ID3v2Tag? ID3v2Decoder(FileStream fs, int offset)
        {
            ID3v2Tag newID3v2Tag = new ID3v2Tag(headbytes,contentbytes);
            return null;
        }

        private Mp3Frame? Mp3FrameDecoder()
        {
            return null;
        }

        class ID3v2Tag:TAG
        {
            string header{set;get;} = string.Empty;
            int ver{set;get;}
            int revision{set;get;}
            byte flag{set;get;}
            int size{set;get;}
            public ID3v2Tag(byte[] headbytes,byte[] contentbytes)
            {
                head = headbytes;
                content = contentbytes;
                header = "ID3";
                ver = (int)headbytes[3];
                revision = (int)headbytes[4];
                flag = headbytes[5];
                size = (((int)headbytes[6]&0x7F)<<21)+(((int)headbytes[7]&0x7F)<<14)+(((int)headbytes[8]&0x7F)<<7)+((int)headbytes[9]&0x7F);
                tagLength = size+10;
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

        private class Mp3Frame
        {

        }


    }

    class TAG
    {
        public int tagLength;
        public byte[]? head;
        public byte[]? content;
    }
}