using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.unionpay.acp.sdk;
using System.Text;
using System.IO;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.X509;

namespace upacp_demo_app.demo.api_05_app
{
    public partial class VerifyAppData : System.Web.UI.Page
    {

        private AsymmetricKeyParameter appVerifyPubKey = null;

        private bool ValidateAppResponse(string jsonData, Encoding encoding)
        {
            //��ȡǩ��
            Dictionary<string, object> data = SDKUtil.JsonToDictionary(jsonData);

            string dataString = (string)data["data"];
            string signString = (string)data["sign"];

            byte[] signByte = Convert.FromBase64String(signString);
            byte[] dataByte = encoding.GetBytes(dataString);
            IDigest digest = DigestUtilities.GetDigest("SHA1");
            digest.BlockUpdate(dataByte, 0, dataByte.Length);
            byte[] dataDigest = DigestUtilities.DoFinal(digest);

            string digestString = BitConverter.ToString(dataDigest).Replace("-", "").ToLower();

            if (appVerifyPubKey == null)
            {
                using (FileStream fileStream = new FileStream("d:/certs/acp_test_app_verify_sign.cer", FileMode.Open))//TODO: ����ǲ��Ի�����֤�飬�л�������Ҫ������֤�顣
                { 
                    X509Certificate certificate = new X509CertificateParser().ReadCertificate(fileStream);
                    this.appVerifyPubKey = certificate.GetPublicKey();
                }
            }
            byte[] digestByte = encoding.GetBytes(digestString);

            ISigner verifier = SignerUtilities.GetSigner("SHA1WithRSA");
            verifier.Init(false, this.appVerifyPubKey);

            verifier.BlockUpdate(digestByte,0, digestByte.Length);
            return verifier.VerifySignature(signByte);  
        }

        protected void Page_Load(object sender, EventArgs e)
        {
                        
            /**
             * �Կؼ����̻�APP���ص�Ӧ����Ϣ��ǩ��ǰ����ֱ�Ӱ�string�͵�json��post����
             */
            StreamReader reader = new StreamReader(Request.InputStream);
            string data = reader.ReadToEnd();
            Response.Write(ValidateAppResponse(data, System.Text.Encoding.UTF8) ? "true" : "false");
        }

    }
}
