using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using PaySharp.Alipay;
using PaySharp.Core;
using PaySharp.Core.Mvc;

namespace AlipayDemo
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var containerBuilder = new ContainerBuilder();

            containerBuilder.Register(a =>
            {
                var gateways = new Gateways();
                //gateways.RegisterAlipay();

                /** 支付宝 - 沙箱环境使用说明
                 * https://alipay.open.taobao.com/doc2/detail.htm?treeId=200&articleId=105311&docType=1
                 * 1. 生成并上传RSA2(SHA256)的应用公钥；配置RSA2(SHA256)的应用公钥后，不需要配置RSA(SHA1)密钥；
                 * 2. 编写代码时，请将
                 *   a.请求网关修改为：https://openapi.alipaydev.com/gateway.do 此处新版本应该修改为 https://openapi.alipaydev.com
                 *   b.appid切换为沙箱的appid
                 *   c.签名方式使用RSA2
                 *   d.应用私钥使用第1步生成的RSA2(SHA256)的私钥(请根据开发语言进行选择原始或pkcs8格式)
                 *   e.支付宝公钥切换为第1步配置后应用公钥后，点击查看支付宝公钥看到的公钥
                 */
                var alipayMerchant = new PaySharp.Alipay.Merchant
                {
                    AppId = "2016092800616303",
                    NotifyUrl = "http://localhost:63988/Notify",
                    ReturnUrl = "http://localhost:63988/Notify",
                    AlipayPublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA5jQk+Jhg20hHmqYf+5HOsRHGYr2UdwmQJMHKNfl2nXvvkRvaY16O9/Ws9veCL6AaWHEzSS5TT3TTnykDC7uG5x7GXVSDlA0wpxWIME9ieglR6+gFKx2Df9mu7p9rcpgraZFidOQmsMGmvFyZf7TJbtKAaj6mCzsHj2dGaApgTokcyg8OFz9rwf+MhMI9tlNJ9+82KtkGhS8MlfSmMZyIUJhQRVhT+G9uGM7A0hQ9hn+q/s3mjQ9XTzWprvXII6Mu64CFV/DHh4jesILUTpB32d4NKhgw2LNmgvaa7+Ym6ETwoB+8kKAM6g+I6saMutFWihme2B1WIX7xv/pJHLG9QQIDAQAB",
                    Privatekey = "MIIEpAIBAAKCAQEA4TxEpvAVVvl42s5iAdr/kSj0rCiESP1DTSpIBOAIuu8PC8nkNnXMEnlKqsKOAWrEYIEqkRSfpEHIW08NzDPEfCm1IlNVRzz7UII8zM+MljVO8ppWlxNShgU7aEgZaBkx1ucgkisUmrnGnHOXR7dEkUGfwbi7kzQD0kPXM7rzBB7CuItAwTemMuA6BiVlUKIZIoljP4pkM/BilK8k+Ag//msFcH+G5hjdF7gE6idLHQOiTLuZmjOdHW6QWy0qFEfbeX4GXC1ERY5njcL7zlLbMwio0Hj+dg7FKbQdiSxLRl39PiRrGydzQHCYJ/y6x6PI4ctZI8RE9c8Lh9XAOOAktQIDAQABAoIBAQCNJCMxKUl2Eya0lpe76ew0nqGUMF+VDX/bHx+6TlmqKpwXGxCzP+X9vZwYnYo3QRyGDOsLtwzC9aYD8eoHiHkcBVbIh8fsuD4UGUjYX0cN6aHrTOPuD+GqsiSkGGozXXZp4LP8ZJqoyrm22Ih4HFQPYjwmPQjuGa47WN/GPuSCM0eFQC5BFlhnCwBDDOY6pciEPe0tDCSDHwBNRtcFgqXtJZGTiAP1hMxslb4V2PrN0XyyC52tpRGE6TCTq6gNt1xaBySjTvIG7CAz4EWOjf9LEqr5ggn6EuRKp470hOkhg1EUaMTqtvO4zAilkoFYRMwrbxn3NoRk1r9w83VlDbcRAoGBAPHwsudjw+ixbEVD8jt4XUiJdZ10gwpGsmdP7ExA3ohuNyXHebBZXpnFdq9j56RpKE/dFJlHBTalKdPlS18xuB6KA9UCd73LMTIgqihnmYlSYb6G2D2UGJtfqWPVEr9NZGNwlA76dtyx7MU/bmxx9NLUFvyVEcCIK8gVRQtvtmOTAoGBAO5TDg+AeyLt+HYHqNH6s/O3uywhMXr+f+lKDXKurMZmhAU0UhC2NqXaVTcISV8ZT/lGL0srcaDUHAJe6oVvPoOnHwqcvCZFHbTj2xrFOj5MKAMSN7vLnXUUvvCu3UuaJbF+AxlbYscuGH9xhmiZHgP9htkKKzOYbibZjPORKpOXAoGAXHvCJ6l/Tgfkd2XNxuXv4raI+zN6lAcKU2u9zDhP2J8o/YwO/FZtTyKoh8sM1VBNVJoSMbVwTL8+Cf3wnecHlsTzSg2zcB2oJJ1P7joL+u4+5vHs3z0pWttqiPr+O5p98XUrY75iiWKKO6xPrayyBZWFY/An5Q0oj0dyKTj+43kCgYEAh7pta460kjQNKMs77jplegvNYViWIYIHvwkZl5K7e1KvJXeitTnQ7avmlCz3/F0iGslJ7fmUARBL013TGqM8ayYmm5if3vvF61tJUXm5rfkZYIZjj2RrDF8AT3qHNaNYjDlD8pEFNIKgGRTCg5eQbJ1aywjribrqIN4NVDn8kYkCgYBvyCnGZVrSfg9fBm2M+ttgcXEDAM3jebiZi20VzHcwfP15OhEbfOIWOj0c54PyZU0sF0/uMvRNBFwbLMfk6oVP2KE/JVnxMt2kOYJnUMhC5Oefz+GiaRYUTamyGmJAVxwyeUwAGfPiaqhAMDbbcyMlwnF1OCS0jmUK66aZOcAOmQ=="
                };

                gateways.Add(new AlipayGateway(alipayMerchant)
                {
                    GatewayUrl = "https://openapi.alipaydev.com"
                });

                return gateways;

            }).As<IGateways>().InstancePerRequest();

            containerBuilder.RegisterControllers(typeof(MvcApplication).Assembly);

            var container = containerBuilder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
