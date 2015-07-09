using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FntFontToWPK
{
    public static class Exporter
    {
        private const Int32 signNumber = 4935767;

        public static void Export(string destinationFilePath, string serviceName, string fntFilePath, params string[] textureFilePaths)
        {
            using (BinaryWriter streamWriter = new BinaryWriter(new FileStream(destinationFilePath, FileMode.OpenOrCreate)))
            {
                #region Write Wave Packet Header
                //Write Sign 4935767
                streamWriter.Write((Int32)signNumber);

                //Write Service Name
                streamWriter.Write(serviceName);

                //Write Gzip flags
                streamWriter.Write(false);
                #endregion

                #region Write all attach files
                //Tạo danh sách các file cần ghi
                List<string> fileList = new List<string>();
                //Ghi file fnt trước tiên
                fileList.Add(fntFilePath);
                //Sau đó ghi các file
                fileList.AddRange(textureFilePaths); 

                foreach (var fileName in fileList)
                {
                    //Ghi header cho từng file
                    streamWriter.Write(System.IO.Path.GetFileName(fileName));

                    //Đọc nội dung file vào buffer
                    const int bufferSize = 1024;

                    MemoryStream fileBuffer = new MemoryStream();
                    byte[] buffer = new byte[bufferSize];

                    using (var fileStream = new FileStream(fileName, FileMode.Open))
                    {
                        while (true)
                        {
                            int readCount = fileStream.Read(buffer, 0, bufferSize);

                            fileBuffer.Write(buffer, 0, readCount);

                            if (readCount < bufferSize)
                                break;
                        }
                    }

                    //Ghi nội dung buffer vào đích
                    var fileBytes = fileBuffer.ToArray();

                    fileBuffer.Close();

                    streamWriter.Write((Int32)fileBytes.Length);
                    streamWriter.Write(fileBytes);
                }
                #endregion

            }
        }
    }
}
