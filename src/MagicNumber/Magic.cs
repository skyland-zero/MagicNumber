using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MagicNumber
{
    public class Magic : IMagic
    {
        private Dictionary<string, List<MagicNumberRecord>> _signatures = new Dictionary<string, List<MagicNumberRecord>>();

        public Magic()
        {
            _signatures = FileSignature.Default;
        }

        /// <summary>
        /// 流与扩展名是否匹配
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="ext"></param>
        /// <returns></returns>
        public bool Match(Stream stream, string ext)
        {
            if (stream == null || string.IsNullOrEmpty(ext))
                return false;

            ext = ext.ToLower();
            if (!_signatures.TryGetValue(ext, out var records))
                return false;
            if (records.Any(x => x.Offset > 0))
            {
                //判断Offset=0的数据
                var offsetZeros = records.Where(a => a.Offset == 0);
                if (offsetZeros != null && offsetZeros.Any())
                {
                    using (var reader = new BinaryReader(stream))
                    {
                        var headerBytes = reader.ReadBytes(offsetZeros.Max(m => m.Size));
                        var temp = offsetZeros.Any(signature => headerBytes.Take(signature.Size).SequenceEqual(signature.Number));
                        if (temp) return true;
                    }
                }
                //判断Offset>0的数据
                var offsetOverZero = records.Where(a => a.Offset > 0).OrderBy(a => a.Offset);
                foreach (var item in offsetOverZero)
                {
                    if (item.Offset > stream.Length) continue;
                    stream.Position = item.Offset;
                    using (var reader = new BinaryReader(stream))
                    {
                        var headerBytes = reader.ReadBytes(item.Size);
                        if (headerBytes.SequenceEqual(item.Number)) return true;
                    }
                }
            }
            else
            {
                using (var reader = new BinaryReader(stream))
                {
                    var headerBytes = reader.ReadBytes(records.Max(m => m.Size));
                    return records.Any(signature => headerBytes.Take(signature.Size).SequenceEqual(signature.Number));
                }
            }
            return false;
        }

    }
}
