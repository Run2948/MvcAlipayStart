using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.unionpay.acp.sdk;

namespace upacp_demo_wtz_token.demo.api_03_token
{
    public partial class Form_6_2_ApplyToken : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /**
             * ��Ҫ����������ʱ����ϸ�Ķ�ע�ͣ�
             * 
             * ��Ʒ������תtoken��Ʒ<br>
             * ���ף�����token�ţ���̨����<br>
             * ���ڣ� 2015-11<br>
             * �汾�� 1.0.0
             * ��Ȩ�� �й�����<br>
             * ˵�������´���ֻ��Ϊ�˷����̻����Զ��ṩ���������룬�̻����Ը����Լ���Ҫ�����ռ����ĵ���д���ô�������ο������ṩ�������ܹ淶�Եȷ���ı���<br>
             * ����˵��:���ݿ�ͨ������׵�orderId����,��̨ͬ��Ӧ��ȷ�����׳ɹ���
             */

            Dictionary<string, string> param = new Dictionary<string, string>();

            //  ������Ϣ��Ҫ��д
            param["orderId"] = Request.Form["orderId"].ToString();//�̻������ţ���д��ͨ��֧�����׵�orderId���˴�Ĭ��ȡdemo��ʾҳ�洫�ݵĲ���
            param["merId"] = Request.Form["merId"].ToString();//�̻����룬��ĳ��Լ��Ĳ����̻��ţ��˴�Ĭ��ȡdemo��ʾҳ�洫�ݵĲ���
            param["txnTime"] = Request.Form["txnTime"].ToString();//��������ʱ�䣬��д��ͨ��֧�����׵�txnTime���˴�Ĭ��ȡdemo��ʾҳ�洫�ݵĲ���
            param["tokenPayData"] = "{trId=62000000001&tokenType=01}"; //���Ի����̶�trId=62000000001&tokenType=01������������ҵ����䡣���Ի�����Ϊ�����̻���ʹ��ͬһ��trId������ͬһ������ȡ��token�Ŷ���ͬ����һ�˷������token���߽��token���󶼻ᵼ��ԭtoken��ʧЧ������֮ǰ�ɹ���ͻȻ����3900002����ʱ���ȳ������¿�ͨһ�¡�
            
            // ���󷽱�����
            // ͸���ֶΣ���ѯ��֪ͨ�������ļ��о���ԭ�����֣�������Ҫ�����ò��޸��Լ�ϣ��͸�������ݡ�
            // ���ֲ��������ַ�ʱ����Ӱ��������밴���潨��ķ�ʽ��д��
            // 1. �����ȷ�����ݲ������&={}[]"'�ȷ���ʱ������ֱ����д���ݣ�����ķ������¡�
            //param["reqReserved"] = "͸����Ϣ1|͸����Ϣ2|͸����Ϣ3";
            // 2. ���ݿ��ܳ���&={}[]"'����ʱ��
            // 1) �����Ҫ�����ļ�������ʾ���ɽ��ַ��滻��ȫ�ǣ����������������ַ����Լ�д���룬�˴�����ʾ����
            // 2) ��������ļ�û����ʾҪ�󣬿���һ��base64�����£���
            //    ע��������ݳ��ȣ�ʵ�ʴ�������ݳ��Ȳ��ܳ���1024λ��
            //    ��ѯ��֪ͨ�Ƚӿڽ���ʱʹ��System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(reqReserved))��base64���ٶ�����������������
            //param["reqReserved"] = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("�����ʽ����Ϣ������"));

            //������Ϣ�������������Ҫ�Ķ�
             param["version"] = SDKConfig.Version;//�汾��
            param["encoding"] = "UTF-8";//���뷽ʽ
            param["certId"] = CertUtil.GetSignCertId();//ǩ��֤��ID
            param["signMethod"] =SDKConfig.SignMethod;//ǩ������
            param["txnType"] = "79";//��������
            param["txnSubType"] = "05";//��������
            param["bizType"] = "000902";//ҵ������
            param["accessType"] = "0";//��������
            param["channelType"] = "07";//��������
            param["encryptCertId"] = CertUtil.GetEncryptCertId();//����֤��ID

            AcpService.Sign(param, System.Text.Encoding.UTF8);  // ǩ��
            string url = SDKConfig.SingleQueryUrl;

            Dictionary<String, String> rspData = AcpService.Post(param, url, System.Text.Encoding.UTF8);

            Response.Write(DemoUtil.GetPrintResult(url, param, rspData));

            if (rspData.Count != 0)
            {

                if (AcpService.Validate(rspData, System.Text.Encoding.UTF8))
                {
                    Response.Write("�̻�����֤���ر���ǩ���ɹ���<br>\n");
                    string respcode = rspData["respCode"]; //����Ӧ�����Ҳ���ô˷�����ȡ
                    if ("00" == respcode)
                    {
                        //����token�ɹ�
                        //TODO
                        Response.Write("����token�ɹ���<br>\n");
                        string tokenPayDataStr = rspData["tokenPayData"];
                        Dictionary<string, string> tokenPayData = SDKUtil.parseQString(tokenPayDataStr.Substring(1, tokenPayDataStr.Length - 2), System.Text.Encoding.UTF8);
                        if (tokenPayData.ContainsKey("token"))
                        {
                            string token = tokenPayData["token"]; //tokenPayData����������ɲο��˷�ʽ��ȡ  
                        }
                        foreach (KeyValuePair<string, string> pair in tokenPayData)
                        {
                            Response.Write(pair.Key + "=" + pair.Value + "<br>\n");
                        }
                    }
                    else
                    {
                        //����Ӧ��������ʧ�ܴ���
                        //TODO
                        Response.Write("ʧ�ܣ�" + rspData["respMsg"] + "��<br>\n");
                    }
                }
                else
                {
                    Response.Write("�̻�����֤���ر���ǩ��ʧ��\n");
                }
            }
            else
            {
                Response.Write("����ʧ��\n");
            }
        }
    }
}