namespace GsdsV2.Utils
{
    public class PasswordEncoder
    {
        public static string passwordEncrypt(string passwordText)
        {
            var a = passwordText.ToArray();
            byte[] b = new byte[a.Length];
            byte[] c = System.Text.Encoding.UTF8.GetBytes(a);
            c.CopyTo(b, 0);
            string d = BitConverter.ToString(b);
            d = d.Replace("-", "");
            d = @"0x" + d;

            return d;
        }

        public static string passwordDecrypt(string hex)
        {
            byte[] stringBytes = Enumerable.Range(2, hex.Length - 2)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();

            string decodedString = System.Text.Encoding.UTF8.GetString(stringBytes);
            return decodedString;
        }
    }
}
