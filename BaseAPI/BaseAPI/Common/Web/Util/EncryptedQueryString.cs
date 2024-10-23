using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BaseAPI.Common.Web.Util
{
    public class EncryptedQueryString : Dictionary<string, string>
    {
        protected byte[] _keyBytes = { 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18 };
        protected string _keyString;
        protected string _checksumKey = "__$$";

        public EncryptedQueryString()
        {
            _keyString = $"{AppServices.Config.EncryptionKey}MindAllSet".Substring(0, 8);
        }

        public EncryptedQueryString(string encryptedData)
        {
            _keyString = $"{AppServices.Config.EncryptionKey}MindAllSet".Substring(0, 8);
            string data = Decrypt(encryptedData);

            string checksum = null;
            string[] args = data.Split('&');

            foreach (string arg in args)
            {
                int i = arg.IndexOf('=');
                if (i != -1)
                {
                    string key = arg.Substring(0, i);
                    string value = arg.Substring(i + 1);
                    if (key == _checksumKey)
                    {
                        checksum = value;
                    }
                    else
                    {
                        if (!base.ContainsKey(HttpUtility.UrlDecode(key)))
                            base.Add(HttpUtility.UrlDecode(key), HttpUtility.UrlDecode(value));
                    }
                }
            }
        }

        public override string ToString()
        {
            StringBuilder content = new StringBuilder();

            foreach (string key in base.Keys)
            {
                if (content.Length > 0)
                    content.Append('&');
                content.AppendFormat("{0}={1}", HttpUtility.UrlEncode(key),
                HttpUtility.UrlEncode(base[key]));
            }

            if (content.Length > 0)
                content.Append('&');
            content.AppendFormat("{0}={1}", _checksumKey, ComputeChecksum());

            return Encrypt(content.ToString());
        }

        protected string ComputeChecksum()
        {
            int checksum = 0;

            foreach (KeyValuePair<string, string> pair in this)
            {
                checksum += pair.Key.Sum(c => c - '0');
                checksum += pair.Value.Sum(c => c - '0');
            }

            return checksum.ToString("X");
        }

        public string Encrypt(string text)
        {
            try
            {
                byte[] keyData = Encoding.UTF8.GetBytes(_keyString.Substring(0, 8));
#pragma warning disable SYSLIB0021 // Type or member is obsolete
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] textData = Encoding.UTF8.GetBytes(text);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms,
                des.CreateEncryptor(keyData, _keyBytes), CryptoStreamMode.Write);
                cs.Write(textData, 0, textData.Length);
                cs.FlushFinalBlock();
                return GetString(ms.ToArray());
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }

        public string Decrypt(string text)
        {
            try
            {
                byte[] keyData = Encoding.UTF8.GetBytes(_keyString.Substring(0, 8));
#pragma warning disable SYSLIB0021 // Type or member is obsolete
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
#pragma warning restore SYSLIB0021 // Type or member is obsolete
                byte[] textData = GetBytes(text);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms,
                des.CreateDecryptor(keyData, _keyBytes), CryptoStreamMode.Write);
                cs.Write(textData, 0, textData.Length);
                cs.FlushFinalBlock();
                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public Dictionary<string, string> DecryptToDictionary(string encryptedData)
        {
            string d = Decrypt(encryptedData);

            if (d.Equals(string.Empty))
                return null;

            return ParseQueryString(Decrypt(encryptedData));
        }

        public Dictionary<string, string> ParseQueryString(string requestQueryString)
        {
            Dictionary<string, string> rc = new Dictionary<string, string>();
            string[] ar1 = requestQueryString.Split(new char[] { '&', '?' });
            foreach (string row in ar1)
            {
                if (string.IsNullOrEmpty(row)) continue;
                int index = row.IndexOf('=');
                if (index < 0) continue;
                rc[Uri.UnescapeDataString(row.Substring(0, index))] = Uri.UnescapeDataString(row.Substring(index + 1)); // use Unescape only parts          
            }
            return rc;
        }

        protected string GetString(byte[] data)
        {
            StringBuilder results = new StringBuilder();

            foreach (byte b in data)
                results.Append(b.ToString("X2"));

            return results.ToString();
        }

        protected byte[] GetBytes(string data)
        {
            byte[] results = new byte[data.Length / 2];

            for (int i = 0; i < data.Length; i += 2)
                results[i / 2] = Convert.ToByte(data.Substring(i, 2), 16);

            return results;
        }
    }
}
