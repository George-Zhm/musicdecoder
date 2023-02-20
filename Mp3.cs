using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

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
        private Myfilestream mp3FileStream;
        private byte[] rawbytes;
        long rawlength;
        private List<ID3v1Tag> id3v1Tags;
        private List<ID3v2Tag> id3v2Tags;
        private List<Mp3Frame> mp3Frames;



        public Mp3(string filepath)
        {
            mp3FileStream = new Myfilestream(filepath, FileMode.Open, FileAccess.Read);
            //reveive complete file
            rawbytes = new byte[mp3FileStream.Length];
            rawlength = mp3FileStream.ReadAll(rawbytes,0,mp3FileStream.Length);
            //
            Mp3Decoder();
            mp3FileStream.Close();
        }

        public void Mp3Decoder()
        {
            int streamoffset = 0; 
            FrameType frameType = FrameType.INVALID;
            while(streamoffset + 11 < mp3FileStream.Length)
            {
                if((rawbytes[streamoffset]==0x49)&&(rawbytes[streamoffset+1]==0x44)&&(rawbytes[streamoffset+2]==0x33))
                {
                    frameType = FrameType.ID3V2TAG;
                }
                else if(false)
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
                        var id3v2Tag = ID3v2Decoder(rawbytes,streamoffset);
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

        private ID3v2Tag? ID3v2Decoder(byte[] streambytes, long index)
        {   
            long headindex = index;
            Byte[] headArray = new byte[10];

            Array.ConstrainedCopy(streambytes,headindex,headArray,0,10);

            int ver;
            int revision;
            BitArray flag;;
            int size;

            ver = (int)streambytes[index+3];
            revision = (int)streambytes[index+4];
            flag = new BitArray(streambytes[index+5]);
            size = (((int)streambytes[index+6]&0x7F)<<21)+(((int)streambytes[index+7]&0x7F)<<14)+
            (((int)streambytes[index+8]&0x7F)<<7)+((int)streambytes[index+9]&0x7F);
            ID3v2Tag newID3v2Tag = new ID3v2Tag(ver,revision,flag,size);

            long contentindex = index+10;

            return null;
        }

        private Mp3Frame? Mp3FrameDecoder()
        {
            return null;
        }

        class ID3v2Tag
        {
            public int tagLength{set;get;}
            string header{set;get;} = string.Empty;
            int ver{set;get;}
            int revision{set;get;}
            BitArray flag{set;get;}
            int size{set;get;}


            public ID3v2Tag(int v,int r,BitArray f,int s)
            {
                header = "ID3";
                ver = v;
                revision = r;
                flag = f;
                size = s;
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

    class Myfilestream : FileStream
    {

        public Myfilestream(string path, FileMode mode, FileAccess access) : base(path, mode, access)
        {
        }

        public long ReadAll(byte[] buffer, int offset, long count)
        {   
            long bufferSize = 0;
            int tempsize = 0;
            byte[] tempbuffer = new byte[1024];
            while((tempsize = Read(tempbuffer,0,1024))>0)
            {
                if(tempsize<1024)
                {
                    byte[] endbuffer = new byte[tempsize];
                    Array.Copy(tempbuffer,endbuffer,tempsize);
                    endbuffer.CopyTo(buffer,bufferSize);
                }
                else
                {
                    tempbuffer.CopyTo(buffer,bufferSize);
                }
                bufferSize+=tempsize;
            }
            return bufferSize;
        }
    }
}