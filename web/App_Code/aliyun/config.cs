using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// config 的摘要说明
/// </summary>
public class config
{
    public config()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    // 应用ID,您的APPID
    public static string app_id = "2017110109666747";

    // 支付宝网关
    public static string gatewayUrl = "https://openapi.alipay.com/gateway.do";

    // 商户私钥，您的原始格式RSA私钥
    //public static string private_key = GetCurrentPath() + "aop-sandbox-RSA-private-c#.pem";
    public static string private_key = "MIIEpAIBAAKCAQEA5jVApWxTQ4gsq3u9TNXEui3EQS2M2I+wCoGAVKRNyNcVv51dXk7D5EpIfMH1XG41L5D3E03de5gUNoR6/q0uOzVxejlbDzpI2QmNg/Ar1v2lLN6Y4rxKa0Xp192eHWozILvaCDAJgoRwvEoEvFQZc74r6gc0OeTNeOGmWlDHg0gF6mDanCSAF2yPVk6HpXiRkRdVTPZ7oNGZfkYI1c/IxyZybPjLuKQ4UN38Zoj1QZXT0KxihLCVClmfkyfWbkmEXWUsIdyIAwAHqKyF4qMWAPSPq8Q+gtd8ipcOOjKYLXSsD9Dzu8P4h+J7OTeoK6Ir1cSjPA59uwHgNPvcjtKOawIDAQABAoIBAGdXiIm6MzahrsvcZIJVhFa/rNZFiHC/MBvHPsDAcVqzk0PNdNidKzsUTa4Ts/2mS6Hqxb+YuNB+2LQQjNY/D/3sY+CmVsZjN8iWjTypWETO4JNf6en+9LlrTSpmhlBWGFWdfqcmSV6Z7bHY2H9ikXpv6G7KhRtoJwUY5wCsOqQnmunduxyc1J1cvb4y05Sovx4RFpooWLTrApVG61Dp6zVnJ/CtdVy6Y/Vepm5QRXg9qvZsIOtV0xdcflDxG1bFjGcTyxx3+b3pTASiS1g5ROiPQzJl37VJcHdL5gLRt7Psxu6QJf0I6y5kfTNwbs/KYMn+HkT1UvC+XOYo4U3Ht0ECgYEA9EifeKsb7KLiLAzWbDTOdCCnKZXN95K/LRiGJc09ZeDxybfhfsj7BVEKBCocYeY74TS4pFH5Yma0IYe8q0ji7UW679ZW6KuchCZKEp/rLpd5txO9L+qIq4ghoUhlguNvkZwVUtF6lQb1xXOoaZDNLyeCHpbGr5i5XL2MbQgkZTkCgYEA8T/OFA8+7sSMYSxL7C18T9lXqU2Qs2pPnf5ueEi+aki4vLAILUc8Ubw/Efv5Z1KNiZkSNEP9jhCg4iKI9dLv34254Rmv9mAZQVNpKwQG54AjIuzyDO56adZkC73oR5rNE0C/8JOPGo5CIM0z5LLEsVJqZ2Gwtk72kLkiT0W5FMMCgYAFk4LQZleE11MCXmimn632yKgIMvs4o4jppp2pTz4PjUr3p9Ll6gCQ5oCsB6oOMgs74NA2MAjZTi7edRLBdjB36heSUfo55XD1M0qNkkj0D1Ef9Ltk1J26DKl33Qc0LazmTgHOmimKLVZ+41z8y+ljIiu+NwLiD0Jr84e5FW5/QQKBgQCXg8MhxhF4U+j2fqOQSWDxpUbxBc4DYJlwWQN4d/dfOR0NJGF+TmcLznauDNqukaJi8MgGG017k+X3IEl0Wm5csN41CbUBv4kdBg3e/kB31Ho8zSOYC47GOefLmBHyJr71gI0LwyD1RLMzdq1IzR3LYD+Dfk9FsIIakquBrgB3kQKBgQDUExO6Ig2Oz6ndRzQU1+AxWj5iFEmxYzZovhtso6ZdZjLSGsv9cSw3FBPsK50WqJ7dgSHNlJOnu7eFXMz4lAG3Wp/ihxNs0qNPMn+oOvdJg4QTGpim1QDYFzVkMXYfdlw6iyB1l0dWJQBiQ962mWtT8EeXoMllFWBFxnfCLZNv9g==";

    // 支付宝公钥,查看地址：https://openhome.alipay.com/platform/keyManage.htm 对应APPID下的支付宝公钥。
    //public static string alipay_public_key = GetCurrentPath() + "public-key.pem";
    public static string alipay_public_key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA5jVApWxTQ4gsq3u9TNXEui3EQS2M2I+wCoGAVKRNyNcVv51dXk7D5EpIfMH1XG41L5D3E03de5gUNoR6/q0uOzVxejlbDzpI2QmNg/Ar1v2lLN6Y4rxKa0Xp192eHWozILvaCDAJgoRwvEoEvFQZc74r6gc0OeTNeOGmWlDHg0gF6mDanCSAF2yPVk6HpXiRkRdVTPZ7oNGZfkYI1c/IxyZybPjLuKQ4UN38Zoj1QZXT0KxihLCVClmfkyfWbkmEXWUsIdyIAwAHqKyF4qMWAPSPq8Q+gtd8ipcOOjKYLXSsD9Dzu8P4h+J7OTeoK6Ir1cSjPA59uwHgNPvcjtKOawIDAQAB";

    // 签名方式
    public static string sign_type = "RSA2";

    // 编码格式
    public static string charset = "UTF-8";

    private static string GetCurrentPath()
    {
        string basePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
        return basePath + "/aliyun/";
    }
}