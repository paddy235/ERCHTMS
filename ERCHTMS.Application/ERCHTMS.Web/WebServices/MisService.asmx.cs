using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Services;
namespace ERCHTMS.Web.WebServices
{
    /// <summary>
    /// MisService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class MisService : System.Web.Services.WebService
    {
        ERCHTMS.Busines.BaseManage.DepartmentBLL deptBll = new Busines.BaseManage.DepartmentBLL();
        [WebMethod(Description="新增人员信息")]
        public string createEmployee(SyncUserInfo userInfo)
        {
            try
            {
                string str = ReturnCode(0);
                DataTable table = deptBll.GetDataTable(string.Format("select *from [base_user] where account='{0}'", userInfo.uid));
                if (table.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(userInfo.displayName))
                    {
                        str = this.updateEmployee(userInfo.uid, userInfo.displayName);
                    }
                    if (str != this.ReturnCode(0))
                    {
                        return str;
                    }
                    if (!string.IsNullOrEmpty(userInfo.email))
                    {
                        str = this.updateEmployeEmail(userInfo.uid, userInfo.email);
                    }
                    if (str != this.ReturnCode(0))
                    {
                        return str;
                    }
                    //if (!string.IsNullOrEmpty(userInfo.password))
                    //{
                    //    str = this.updateEmployeePassword(userInfo.uid, userInfo.password);
                    //}
                    str = this.enableOrNotEmployee(userInfo, 1);
                }
                else
                {
                    try
                    {
                         ERCHTMS.Entity.BaseManage.UserEntity user = new Entity.BaseManage.UserEntity();
                         user.UserId = Guid.NewGuid().ToString();
                         user.RealName = userInfo.displayName;
                         user.Account = userInfo.uid;
                         user.Email = userInfo.email;
                         user.Password = "123456";
                         new ERCHTMS.Busines.BaseManage.UserBLL().SaveForm("",user);
                         return this.ReturnCode(0);
                    }
                    catch(Exception ex)
                    {
                        return this.ReturnCode(11);
                    }
                }
                return str;
            }
            catch (Exception)
            {
                return this.ReturnCode(11);
            }
        }
        [WebMethod(Description = "删除人员信息")]
        public string deleteEmployee(string employeeLoginName)
        {
            try
            {
                int count = deptBll.ExecuteSql(string.Format("delete from base_user where account='{0}'", employeeLoginName));
                if (count>0)
                {
                    return ReturnCode(0);
                }
                else
                {
                   return ReturnCode(3);
                }
            }
            catch (Exception)
            {
                return ReturnCode(3);
            }
           
        }


        public string enableOrNotEmployee(SyncUserInfo userInfo, int active)
        {
            try
            {
                int count=deptBll.ExecuteSql(string.Format("update base_user set enabledmark={1} where account='{0}'", userInfo.uid, active));
                if (count>0)
                {
                    return this.ReturnCode(0);
                }
                return this.ReturnCode(3);
            }
            catch (Exception)
            {
                return this.ReturnCode(3);
            }
        }

        public string updateEmployee(string employeeLoginName, string employeeName)
        {
            try
            {
                int count = deptBll.ExecuteSql(string.Format("update base_user set realname='{1}' where account='{0}'", employeeLoginName, employeeName));
                if (count > 0)
                {
                    return this.ReturnCode(0);
                }
                return this.ReturnCode(3);
            }
            catch (Exception)
            {
                return this.ReturnCode(3);
            }
        }
        public string updateEmployeEmail(string employeeLoginName, string employeeEmail)
        {
            try
            {
                int count = deptBll.ExecuteSql(string.Format("update base_user set email='{1}' where account='{0}'", employeeLoginName, employeeEmail));
                if (count > 0)
                {
                    return this.ReturnCode(0);
                }
                return this.ReturnCode(3);
            }
            catch (Exception)
            {
                return this.ReturnCode(3);
            }
        }
        public string updateEmployeePassword(string employeeLoginName, string employeePassword)
        {
            try
            {
               
                return this.ReturnCode(3);
            }
            catch (Exception)
            {
                return this.ReturnCode(3);
            }
        }
        public string ReturnCode(int iNo)
        {
            string code = "";
            string desc = "";
            switch (iNo)
            {
                case 0:
                    code = "S000A000";
                    desc = "处理成功";
                    break;

                case 1:
                    code = "E0003000";
                    desc = "令牌错误";
                    break;

                case 2:
                    code = "E0003001";
                    desc = "处理失败，请联系应用系统管理员";
                    break;

                case 3:
                    code = "E0003002";
                    desc = "用户不存在";
                    break;

                case 4:
                    code = "E0003003";
                    desc = "报文无法识别";
                    break;

                case 5:
                    code = "E0003004";
                    desc = "必填数据不能为空";
                    break;

                case 6:
                    code = "E0003005";
                    desc = "数据长度超出限制";
                    break;

                case 7:
                    code = "";
                    desc = "无返回值，请查看请求数据、网络状况、应用系统服务是否正常!";
                    break;

                case 8:
                    code = "E0003006";
                    desc = "操作类型错误";
                    break;

                case 9:
                    code = "E0003007";
                    desc = "应用系统逻辑错误";
                    break;

                case 10:
                    code = "E0003008";
                    desc = "未知的程序异常";
                    break;

                case 11:
                    code = "E0003001";
                    desc = "用户已经存在";
                    break;

                case 12:
                    code = "E0003011";
                    desc = "获取远端数据出错";
                    break;

                case 13:
                    code = "E0003012";
                    desc = "用户创建成功";
                    break;

                default:
                    code = "";
                    desc = "无返回值，请查看请求数据、网络状况、应用系统服务是否正常!";
                    break;
            }
            return SyncXmlUtils.returnXml(code, desc);
        }

    }
    public class SyncUserInfo
    {
        // Fields
        public string uid{set;get;}
        public string password { set; get; }
        public string displayName { set; get; }
        public string email { set; get; }

    }
    public class SyncXmlUtils
    {
        // Fields
        private const string key = "20111219";
        private const string iv = "12345678";

        // Methods
       
        public static string matchRequestData(string soapResponse, string patternStr)
        {
            string str = "<" + patternStr + ">";
            string str2 = "</" + patternStr + ">";
            int index = soapResponse.IndexOf("<tem:requestData>");
            int num2 = soapResponse.IndexOf("</tem:requestData>");
            int startIndex = index + "<tem:requestData>".Length;
            string decryptStr = soapResponse.Substring(startIndex, num2 - startIndex);
            Encoding encoding1 = Encoding.UTF8;
            decryptStr = EncoderUtil.DecryptDESWithJava(decryptStr);
            int num4 = decryptStr.IndexOf(str);
            int num5 = decryptStr.IndexOf(str2);
            string str4 = "";
            if (num4 != -1)
            {
                str4 = decryptStr.Substring(num4 + str.Length, (num5 - num4) - str.Length);
            }
            return str4;

        }
        public static string matchResponse(string soapResponse, string patternStr)
        {
            string str = "<" + patternStr + ">";
            string str2 = "</" + patternStr + ">";
            int index = soapResponse.IndexOf(str);
            int length = soapResponse.IndexOf(str2);
            string str3 = "";
            if (index != -1)
            {
                str3 = soapResponse.Substring(index + str.Length, length);
            }
            return str3;
        }
        public static string returnXml(string code, string desc)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:tem=\"http://tempuri.org/\">");
            builder.Append("<soapenv:Body>");
            builder.Append("<tmp:RESPONSE>");
            builder.Append("<RESULT_CODE>" + code + "</RESULT_CODE>");
            builder.Append("<RESULT_DESC>" + desc + "</RESULT_DESC>");
            builder.Append("<RESULT_DATA></RESULT_DATA>");
            builder.Append("</tmp:RESPONSE>");
            builder.Append("</soapenv:Body>");
            builder.Append("</soapenv:Envelope>");
            return builder.ToString();
        }
        public static string tagBegin(string begStr)
        {
            return ("<" + begStr + ">");
        }
        public static string tagEnd(string endStr)
        {
            return ("</" + endStr + ">");
        }
    }

    public class EncoderUtil
    {
        // Fields
        private static byte[] key;
        private static byte[] iv;

        // Methods
        public static string DecryptDESWithJava(string DecryptStr)
        {
            try
            {
                byte[] buffer = Convert.FromBase64String(DecryptStr);
                byte[] rgbKey = getcsharpkey();
                byte[] iV = IV;
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider
                {
                    Mode = CipherMode.ECB
                };
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(rgbKey, iV), CryptoStreamMode.Write);
                stream2.Write(buffer, 0, buffer.Length);
                stream2.FlushFinalBlock();
                return Encoding.UTF8.GetString(stream.ToArray());
            }
            catch (Exception)
            {
                return DecryptStr;
            }
        }


        public static byte[] Des3DecodeCBC(byte[] data)
        {
            try
            {
                MemoryStream stream = new MemoryStream(data);
                TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider
                {
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.PKCS7
                };
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(key, iv), CryptoStreamMode.Read);
                byte[] buffer = new byte[data.Length];
                stream2.Read(buffer, 0, buffer.Length);
                return buffer;
            }
            catch (CryptographicException exception)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", exception.Message);
                return null;
            }
        }


        public static byte[] Des3DecodeECB(byte[] data)
        {
            try
            {
                MemoryStream stream = new MemoryStream(data);
                TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider
                {
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(key, iv), CryptoStreamMode.Read);
                byte[] buffer = new byte[data.Length];
                stream2.Read(buffer, 0, buffer.Length);
                return buffer;
            }
            catch (CryptographicException exception)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", exception.Message);
                return null;
            }
        }
        public static byte[] Des3EncodeCBC(byte[] data)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider
                {
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.PKCS7
                };
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(key, iv), CryptoStreamMode.Write);
                stream2.Write(data, 0, data.Length);
                stream2.FlushFinalBlock();
                byte[] buffer = stream.ToArray();
                stream2.Close();
                stream.Close();
                return buffer;
            }
            catch (CryptographicException exception)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", exception.Message);
                return null;
            }
        }
        public static byte[] Des3EncodeECB(byte[] data)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider
                {
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(key, iv), CryptoStreamMode.Write);
                stream2.Write(data, 0, data.Length);
                stream2.FlushFinalBlock();
                byte[] buffer = stream.ToArray();
                stream2.Close();
                stream.Close();
                return buffer;
            }
            catch (CryptographicException exception)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", exception.Message);
                return null;
            }
        }
        public static byte[] getcsharpkey()
        {
            sbyte[] array = new sbyte[] { -36, -63, 0x31, 0x25, -56, -32, 0x67, -85 };
            return Array.ConvertAll<sbyte, byte>(array, a => (byte)a);
        }
        public static string MD5(string encypStr)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(encypStr);
            bytes = provider.ComputeHash(bytes);
            StringBuilder builder = new StringBuilder();
            foreach (byte num in bytes)
            {
                builder.Append(num.ToString("x2").ToUpper());
            }
            return builder.ToString();
        }
        // Properties
        private static string CsharpKey
        {
            get
            {
                byte[] bytes = getcsharpkey();
                return Encoding.UTF8.GetString(bytes);
            }
        }
        private static byte[] IV { get; set; }
    }





}
