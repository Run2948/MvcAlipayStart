using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.unionpay.acp.sdk;

namespace upacp_demo_wtz_token.demo.api_03_token
{
    public partial class Form_6_6_SmsConsume : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /**
             * ��Ҫ����������ʱ����ϸ�Ķ�ע�ͣ�
             * 
             * ��Ʒ������תtoken��Ʒ<br>
             * ���ף����Ѷ��ţ���̨���ף���֪ͨ<br>
             * ���ڣ� 2015-09<br>
             * �汾�� 1.0.0
             * ��Ȩ�� �й�����<br>
             * ˵�������´���ֻ��Ϊ�˷����̻����Զ��ṩ���������룬�̻����Ը����Լ���Ҫ�����ռ����ĵ���д���ô�������ο������ṩ�������ܹ淶�Եȷ���ı���<br>
             * ����˵��: ͬ��Ӧ��ȷ�����׳ɹ���
             *        ���  + token��  + �ֻ���phoneNo��customerInfo�򣩣�ͬ��Ӧ��ȷ�����׳ɹ���
             */

            Dictionary<string, string> param = new Dictionary<string, string>();

            //������Ϣ��Ҫ��д
            param["orderId"] = Request.Form["orderId"].ToString();//�̻������ţ�8-32λ������ĸ���ɰ��Լ�����������˴�Ĭ��ȡdemo��ʾҳ�洫�ݵĲ���
            param["merId"] = Request.Form["merId"].ToString();//�̻����룬��ĳ��Լ��Ĳ����̻��ţ��˴�Ĭ��ȡdemo��ʾҳ�洫�ݵĲ���
            param["txnTime"] = Request.Form["txnTime"].ToString();//��������ʱ�䣬ȡϵͳʱ�䣬�˴�Ĭ��ȡdemo��ʾҳ�洫�ݵĲ���
            param["txnAmt"] = Request.Form["txnAmt"].ToString();//���׽���λ�֣��˴�Ĭ��ȡdemo��ʾҳ�洫�ݵĲ���
            param["tokenPayData"] = "{trId=62000000001&token=" + Request.Form["token"].ToString() + "}"; //token�Ŵӿ�ͨ�Ϳ�ͨ��ѯ��ڻ�ȡ

           //������Ϣ�������������Ҫ�Ķ�
             param["version"] = SDKConfig.Version;//�汾��
            param["encoding"] = "UTF-8";//���뷽ʽ
            param["signMethod"] =SDKConfig.SignMethod;//ǩ������
            param["txnType"] = "77";//��������
            param["txnSubType"] = "02";//��������
            param["bizType"] = "000902";//ҵ������
            param["accessType"] = "0";//��������
            param["channelType"] = "07";//��������
            param["currencyCode"] = "156";//���ױ��֣������̻����
            param["encryptCertId"] = AcpService.GetEncryptCertId();//����֤��ID

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
                        //�ɹ�
                        //TODO
                        Response.Write("�ɹ���<br>\n");
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