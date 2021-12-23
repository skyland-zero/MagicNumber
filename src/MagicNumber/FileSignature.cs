using System.Collections.Generic;

namespace MagicNumber
{
    public class FileSignature
    {

        public static readonly Dictionary<string, List<MagicNumberRecord>> Default = new Dictionary<string, List<MagicNumberRecord>>
        {
            {
                "jpeg", new List<MagicNumberRecord> {
                    new MagicNumberRecord(){ Number = new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 }, Offset = 0, Size = 4 },
                    new MagicNumberRecord(){ Number = new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 }, Offset = 0, Size = 4 },
                    new MagicNumberRecord(){ Number = new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 }, Offset = 0, Size = 4 }
                }
            },
            {
                "jpg", new List<MagicNumberRecord> {
                    new MagicNumberRecord(){ Number = new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 }, Offset = 0, Size = 4 },
                    new MagicNumberRecord(){ Number = new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 }, Offset = 0, Size = 4 },
                    new MagicNumberRecord(){ Number = new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 }, Offset = 0, Size = 4 }
                }
            }
        };
    }
}
