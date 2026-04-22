using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Luban;
using UnityEngine;

namespace FrameworkCore.Futures
{

    public class LuBanData
    {

        public Dictionary<string, ByteBuf> BytesData { get; set; } = new Dictionary<string, ByteBuf>();

        public byte[] Bytes { get; set; }

        public LuBanData(byte[] bytes)
        {
            Debug.Log("LuBanData: " + bytes.Length);
            Bytes = bytes;
            // 对Bytes进行解码，得到实际的数据
            // 名字长度 + 名字 + 数据长度 + 数据
            if (Bytes == null || Bytes.Length == 0)
                return;
            using (MemoryStream ms = new MemoryStream(Bytes))
            using (BinaryReader br = new BinaryReader(ms))
            {
                while (ms.Position < ms.Length)
                {
                    Debug.Log($"LuBanData start: {Bytes.Length}");
                    string name = br.ReadString();
                    Debug.Log($"LuBanData name: {name}");
                    int dataLength = br.Read7BitEncodedInt32();
                    Debug.Log($"LuBanData dataLength: {dataLength}");
                    byte[] data = br.ReadBytes(dataLength);
                    Debug.Log($"LuBanData name: {name}, dataLength: {dataLength}");
                    Debug.Log($"ms.Position: {ms.Position} ms.Length: {ms.Length}");
                    BytesData[name] = new ByteBuf(data);
                }
                LoadData = new Func<string, ByteBuf>((string name) => getData(name));
            }
        }

        public Func<string, ByteBuf> LoadData;

        private ByteBuf getData(string name)
        {
            if (BytesData.ContainsKey(name))
            {
                return BytesData[name];
            }
            return null;
        }
    }
}
