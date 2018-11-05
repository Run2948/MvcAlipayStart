using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.unionpay.acp.sdk;

namespace upacp_demo_wtz_token.demo.api_03_token
{
    public partial class Form_6_5_UpdateToken : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /**
             * ��Ҫ����������ʱ����ϸ�Ķ�ע�ͣ�
             * 
             * ��Ʒ������תtoken��Ʒ<br>
             * ���ף�����token�ţ���̨���ף���֪ͨ<br>
             * ���ڣ� 2015-09<br>
             * �汾�� 1.0.0
             * ��Ȩ�� �й�����<br>
             * ˵�������´���ֻ��Ϊ�˷����̻����Զ��ṩ���������룬�̻����Ը����Լ���Ҫ�����ռ����ĵ���д���ô�������ο������ṩ�������ܹ淶�Եȷ���ı���<br>
             * ����˵��: ͬ��Ӧ��ȷ�����׳ɹ�����֧���̻��࿪ͨ�ķ�ʽ���Ƽ�ֱ��ʹ�ÿ�ͨ�ӿ�������token������������ӿڸ��¡�
             *        
             */

            Dictionary<string, string> param = new Dictionary<string, string>();

            //  ������Ϣ��Ҫ��д
            param["orderId"] = Request.Form["orderId"].ToString();//�̻������ţ�8-32λ������ĸ���ɰ��Լ���������������Ͷ�����֤�룬����д��ȡ��֤��ʱһ����orderId���˴�Ĭ��ȡdemo��ʾҳ�洫�ݵĲ���
            param["merId"] = Request.Form["merId"].ToString();//�̻����룬��ĳ��Լ��Ĳ����̻��ţ��˴�Ĭ��ȡdemo��ʾҳ�洫�ݵĲ���
            param["txnTime"] = Request.Form["txnTime"].ToString();//��������ʱ�䣬ȡϵͳʱ�䣬�����Ͷ�����֤�룬����д��ȡ��֤��ʱһ����orderId���˴�Ĭ��ȡdemo��ʾҳ�洫�ݵĲ���
            param["tokenPayData"] = "{trId=62000000001&token=" + Request.Form["token"].ToString() + "&tokenType=01}"; //token�Ŵӿ�ͨ�Ϳ�ͨ��ѯ��ڻ�ȡ��trId�Ϳ�ͨ�ӿ�ʱ���͵���ͬ

            //֧������Ϣ��д
            //���ǿ� ���ͣ��ֻ��š�CVN2����Ч�ڣ���֤�뿴ҵ�����ã�Ĭ����Ҫ������֤�룩��
            //��ǿ� ���ͣ��ֻ��ţ�ѡ�ͣ�֤������+֤���š���������֤�뿴ҵ�����ã�Ĭ����Ҫ������֤�룩��
            Dictionary<string, string> customerInfo = new Dictionary<string, string>();
            customerInfo["phoneNo"] = "18100000000"; //�ֻ���
            //customerInfo["certifTp"] = "01"; //֤�����ͣ�01-���֤
            //customerInfo["certifId"] = "510265790128303"; //֤���ţ�15λ���֤��У��β�ţ�18λ��У��β�ţ��������ǰ��д��У�����
            //customerInfo["customerNm"] = "����"; //����
            customerInfo["cvn2"] = "248"; //cvn2
            customerInfo["expired"] = "1912"; //��Ч�ڣ�YYMM��ʽ���ֿ��˿���ӡ����MMYY�ģ���ע��������õ�һ��
            customerInfo["smsCode"] = "111111"; //������֤�룬���Ի���������ʵ�յ����ţ��̶���111111������123456��654321�̶���ʧ�ܣ�����̶��ɹ����˽ӿڻ�ȡ��֤��ӿ�ͬ��ͨ�Ļ�ȡ��֤��ӿڡ�

            //param["customerInfo"] = AcpService.GetCustomerInfo(customerInfo, System.Text.Encoding.UTF8); //�ֿ��������Ϣ���ɹ淶�밴�˷�ʽ��д
            param["customerInfo"] = AcpService.GetCustomerInfoWithEncrypt(customerInfo, System.Text.Encoding.UTF8); //�ֿ��������Ϣ���¹淶�밴�˷�ʽ��д

            //������Ϣ�������������Ҫ�Ķ�
             param["version"] = SDKConfig.Version;//�汾��
            param["encoding"] = "UTF-8";//���뷽ʽ
            param["signMethod"] =SDKConfig.SignMethod;//ǩ������
            param["txnType"] = "79";//��������
            param["txnSubType"] = "03";//��������
            param["bizType"] = "000902";//ҵ������
            param["accessType"] = "0";//��������
            param["channelType"] = "07";//��������
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
                        //���±�ǳɹ�
                        //TODO
                        Response.Write("���±�ǳɹ���<br>\n");
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