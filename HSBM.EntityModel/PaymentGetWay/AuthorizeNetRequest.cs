using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.PaymentGetWay
{
    public class AuthorizeNetRequest
    {

        public const string TEST_URL = "https://test.authorize.net/gateway/transact.dll";
        public const string LIVE_URL = "https://secure2.authorize.net/gateway/transact.dll";
        public AuthorizeNetRequest()
        {
            this.FormUrl = TEST_URL;
            this.ApiLogin = "7R6pbBg4Z";
            this.TransactionKey = "2r4798474H6haFUR";

            //    this.FormUrl = LIVE_URL;
            //    this.ApiLogin = "7R6pbBg4Z";
            //    this.TransactionKey = "2r4798474H6haFUR";

            this.ReturnUrl = "http://2faa29c4.ngrok.io/widget/Payment";
        }

        public string FormUrl { get; set; }
        public bool IsAuthorizeNet { get; set; }
        public string ApiLogin { get; set; }
        public string TransactionKey { get; set; }
        public string FingerPrint
        {
            get;
            set;
        }
        public int Stamp
        {
            get;
            set;

        }
        public string Seq
        {
            get;
            set;
        }
        public string ReturnUrl { get; set; }
        public bool RelayResponse
        {
            get;
            set;
        }
        public string OrderID
        {
            get;
            set;
        }
        public decimal Amount { get; set; }
        public int TId { get; set; }
        public string ShowForm
        {
            get;
            set;
        }

        /// Generates the HMAC-encrypted hash to send along with the SIM form
        /// </summary>
        /// <param name="transactionKey">The merchant's transaction key</param>
        /// <param name="login">The merchant's Authorize.NET API Login</param>
        /// <param name="amount">The amount of the transaction</param>
        /// <param name="sequence">The sequence</param>
        /// <param name="timeStamp">The timeStamp</param>
        /// <returns>string</returns>
        public string GenerateFingerprint(string transactionKey, string login, decimal amount, string sequence, string timeStamp)
        {
            var result = "";
            var keyString = string.Format("{0}^{1}^{2}^{3}^", login, sequence, timeStamp.ToString(), amount.ToString());
            result = EncryptHMAC(transactionKey, keyString);
            return result;
        }

        /// <summary>
        /// Encrypts the key/value pair supplied using HMAC-MD5
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <returns></returns>
        public string EncryptHMAC(string key, string value)
        {
            // The first two lines take the input values and convert them from strings to Byte arrays
            byte[] HMACkey = (new System.Text.ASCIIEncoding()).GetBytes(key);
            byte[] HMACdata = (new System.Text.ASCIIEncoding()).GetBytes(value);

            // create a HMACMD5 object with the key set
            HMACMD5 myhmacMD5 = new HMACMD5(HMACkey);

            //calculate the hash (returns a byte array)
            byte[] HMAChash = myhmacMD5.ComputeHash(HMACdata);

            //loop through the byte array and add append each piece to a string to obtain a hash string
            string fingerprint = "";
            for (int i = 0; i < HMAChash.Length; i++)
            {
                fingerprint += HMAChash[i].ToString("x").PadLeft(2, '0');
            }

            return fingerprint;
        }

        /// <summary>
        /// Generates a 4-place sequence number randomly
        /// </summary>
        /// <returns></returns>
        public string GenerateSequence()
        {
            CryptoRandom random = new CryptoRandom();
            return (random.Next(0, 10000000)).ToString();

        }

        /// <summary>
        /// Generates a timestamp in seconds from 1970
        /// </summary>
        public int GenerateTimestamp()
        {
            return ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);
        }

    }
}
