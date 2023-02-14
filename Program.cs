using musicdecoder;
Console.WriteLine("Hello, World!");
string musicfile = "y860.mp3";
if(File.Exists(musicfile))
{
    // FileStream fs = new FileStream(musicfile,FileMode.Open,FileAccess.Read);
    // int  n = (int)fs.Length;
    // byte[] music_bytestream = new byte[n];
    // int r = fs.Read(music_bytestream,0,n);
    // Console.WriteLine(BitConverter.ToString(music_bytestream));
    Mp3 mymp3 = new Mp3(musicfile);
    
}