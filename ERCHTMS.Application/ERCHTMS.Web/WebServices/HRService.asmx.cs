using BSFramework.Util;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Services;
using BSFramework.Util.Attributes;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage;

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
    public class HRService : System.Web.Services.WebService
    {
        ERCHTMS.Busines.BaseManage.DepartmentBLL deptBll = new Busines.BaseManage.DepartmentBLL();
        [WebMethod(Description = "华润电力统一接口")]
        public string synchronism(string username, string password, string requestData)
        {
            if (!isAuth(username, password, requestData))
            {
                return this.ReturnCode(1);
            }
            try
            {
                string text2 = SyncXmlUtils.matchRequestData(requestData, "operation-type");
                SyncUserInfo userInfo = SyncUser.parseXMLToUser(requestData);
                string text3;
                if ((text3 = text2) != null)
                {
                    if (text3 == "CREATE")
                    {
                        return this.createEmployee(userInfo);
                    }
                    if (text3 == "DELETE")
                    {
                        return this.enableOrNotEmployee(userInfo, 0);
                    }
                    if (text3 == "DISABLE")
                    {
                        return this.enableOrNotEmployee(userInfo, 0);
                    }
                    if (text3 == "ENABLE")
                    {
                        return this.enableOrNotEmployee(userInfo, 1);
                    }
                    if (text3 == "EDIT")
                    {
                        return this.editAccount(userInfo);
                    }
                }
                return this.ReturnCode(3);
            }
            catch (Exception ex)
            {
                LdapDataLog(Newtonsoft.Json.JsonConvert.SerializeObject(ex), 4);
                return this.ReturnCode(10);
            }
        }
        //[WebMethod(Description="新增人员信息")]
        public string createEmployee(SyncUserInfo userInfo)
        {
            try
            {
                string str = ReturnCode(0);
                DataTable table = deptBll.GetDataTable(string.Format("select userid from base_user where upper(account)='{0}'", userInfo.uid.ToUpper()));
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
                    if (!string.IsNullOrEmpty(userInfo.password))
                    {
                        str = this.updateEmployeePassword(userInfo.uid, userInfo.password);
                    }
                    str = this.enableOrNotEmployee(userInfo, 1);

                    str = this.UpdateAccountType(userInfo.uid.ToUpper());

                }
                else
                {
                    try
                    {
                        string enCode = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("hrdl", "FactoryEncode");//查询电厂编码
                        string orgId = "";//电厂ID
                        string orgCode = "";//电厂编码
                        string deptId = "";//部门Id
                        string deptCode = "";//部门编码
                        if (!string.IsNullOrEmpty(enCode))
                        {
                            DepartmentEntity dept = deptBll.GetEntityByCode(enCode);//获取电厂信息
                            if (dept != null)
                            {
                                orgId = dept.DepartmentId;
                                orgCode = enCode;
                                //判断是否存在临时人员的部门节点,不存在则新建
                                DataTable dtDept = deptBll.GetDataTable(string.Format("select DepartmentId,encode from BASE_DEPARTMENT where parentid='{0}' and fullname='临时人员' and Nature='部门'", orgId));
                                if (dtDept.Rows.Count > 0)
                                {
                                    deptId = dtDept.Rows[0][0].ToString();
                                    deptCode = dtDept.Rows[0][1].ToString();

                                }
                                else
                                {
                                    //新建临时人员的部门节点
                                    dept = new DepartmentEntity
                                    {
                                        FullName = "临时人员",
                                        ParentId = orgId,
                                        Nature = "部门",
                                        Description = "临时人员",
                                        IsOrg = 0,
                                        Industry = "电力"
                                    };
                                    deptBll.SaveForm("", dept);
                                    deptId = dept.DepartmentId;
                                    deptCode = dept.EnCode;

                                }
                                dtDept.Dispose();
                            }
                        }
                        //新增的人员直接挂在临时人员的部门节点，后期由电厂管理员手动调整人完善员信息
                        ERCHTMS.Entity.BaseManage.UserEntity user = new Entity.BaseManage.UserEntity();
                        user.UserId = Guid.NewGuid().ToString();
                        user.RealName = userInfo.displayName;
                        user.Account = userInfo.uid;
                        user.Email = userInfo.email;
                        user.Password = userInfo.password;
                        user.DepartmentId = deptId;
                        user.DepartmentCode = deptCode;
                        user.OrganizeCode = orgCode;
                        user.OrganizeId = orgId;
                        user.RoleId = "2a878044-06e9-4fe4-89f0-ba7bd5a1bde6";
                        user.RoleName = "普通用户";
                        user.IsPresence = "1";
                        user.AccountType = "1";
                        user.Mobile = userInfo.mobile;
                        user.EnCode = userInfo.empno;
                        //user.WeChat = userInfo.deptId;
                        //user.OICQ = userInfo.password;
                        new ERCHTMS.Busines.BaseManage.UserBLL().SaveForm("", user,1);
                        return this.ReturnCode(0);
                    }
                    catch (Exception ex)
                    {
                        LdapDataLog(Newtonsoft.Json.JsonConvert.SerializeObject(ex),4);
                        return this.ReturnCode(11);
                    }
                }
                return str;
            }
            catch (Exception ex)
            {
                LdapDataLog(Newtonsoft.Json.JsonConvert.SerializeObject(ex), 4);
                return this.ReturnCode(10);
            }
        }

        /// <summary>
        /// 如果用户在本地已存在,修改用户的来源为LDAP
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        private string UpdateAccountType(string uid)
        {
            try
            {
                int count = deptBll.ExecuteSql(string.Format("update base_user set ACCOUNTTYPE='1' where upper(account)='{0}'", uid.ToUpper()));
                if (count > 0)
                {
                    return this.ReturnCode(0);
                }
                return this.ReturnCode(3);
            }
            catch (Exception ex)
            {
                LdapDataLog(Newtonsoft.Json.JsonConvert.SerializeObject(ex), 4);
                return this.ReturnCode(3);
            }
        }
        //[WebMethod(Description = "删除人员信息")]
        public string deleteEmployee(string employeeLoginName)
        {
            try
            {
                int count = deptBll.ExecuteSql(string.Format("delete from base_user where upper(account)='{0}'", employeeLoginName.ToUpper()));
                if (count > 0)
                {
                    return ReturnCode(0);
                }
                else
                {
                    return ReturnCode(3);
                }
            }
            catch (Exception ex)
            {
                LdapDataLog(Newtonsoft.Json.JsonConvert.SerializeObject(ex), 4);
                return ReturnCode(3);
            }

        }


        public string enableOrNotEmployee(SyncUserInfo userInfo, int active)
        {
            try
            {
                int count = deptBll.ExecuteSql(string.Format("update base_user set enabledmark={1} where upper(account)='{0}'", userInfo.uid.ToUpper(), active));
                if (count > 0)
                {
                    return this.ReturnCode(0);
                }
                return this.ReturnCode(3);
            }
            catch (Exception ex)
            {
                LdapDataLog(Newtonsoft.Json.JsonConvert.SerializeObject(ex), 4);
                return this.ReturnCode(3);
            }
        }
        public string editAccount(SyncUserInfo userInfo)
        {
            string text = this.ReturnCode(0);
            DataTable dataTable = deptBll.GetDataTable(string.Format("select userid from base_user where upper(account)='{0}'", userInfo.uid.ToUpper()));
            if (dataTable.Rows.Count > 0)
            {
                if (userInfo.displayName != "")
                {
                    text = this.updateEmployee(userInfo.uid, userInfo.displayName);
                }
                if (text != this.ReturnCode(0))
                {
                    return text;
                }
                if (userInfo.email != "")
                {
                    text = this.updateEmployeEmail(userInfo.uid, userInfo.email);
                }
                if (text != this.ReturnCode(0))
                {
                    return text;
                }
                if (userInfo.password != "")
                {
                    text = this.updateEmployeePassword(userInfo.uid, userInfo.password);
                }
            }
            else
            {
                text = this.ReturnCode(3);
            }
            return text;
        }
        public string updateEmployee(string employeeLoginName, string employeeName)
        {
            try
            {
                int count = deptBll.ExecuteSql(string.Format("update base_user set realname='{1}' where upper(account)='{0}'", employeeLoginName.ToUpper(), employeeName));
                if (count > 0)
                {
                    return this.ReturnCode(0);
                }
                return this.ReturnCode(3);
            }
            catch (Exception ex)
            {
                LdapDataLog(Newtonsoft.Json.JsonConvert.SerializeObject(ex), 4);
                return this.ReturnCode(3);
            }
        }
        public string updateEmployeEmail(string employeeLoginName, string employeeEmail)
        {
            try
            {
                int count = deptBll.ExecuteSql(string.Format("update base_user set email='{1}' where upper(account)='{0}'", employeeLoginName.ToUpper(), employeeEmail));
                if (count > 0)
                {
                    return this.ReturnCode(0);
                }
                return this.ReturnCode(3);
            }
            catch (Exception ex)
            {
                LdapDataLog(Newtonsoft.Json.JsonConvert.SerializeObject(ex), 4);
                return this.ReturnCode(3);
            }
        }
        public string updateEmployeePassword(string employeeLoginName, string employeePassword)
        {
            string result;
            try
            {
               int count = deptBll.ExecuteSql(string.Format("update base_user set Password='{0}' where upper(account)='{1}'", employeePassword, employeeLoginName.ToUpper()));
                if (count > 0)
                {
                    result = this.ReturnCode(0);
                }
                else
                {
                    result = this.ReturnCode(3);
                }
            }
            catch (Exception ex)
            {
                LdapDataLog(Newtonsoft.Json.JsonConvert.SerializeObject(ex), 4);
                result = this.ReturnCode(3);
            }
            return result;
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

            if (iNo != 0)
            {
                if (iNo != 13)
                {
                    LdapDataLog(desc, 4);
                }
                else
                {
                    LdapDataLog(desc, 3);
                }
             
            }

            return SyncXmlUtils.returnXml(code, desc);
        }
        public static string getWSUser(string soapStr)
        {
            return SyncXmlUtils.matchResponse(soapStr, "tem:username");
        }

        // Token: 0x06000030 RID: 48 RVA: 0x000031D0 File Offset: 0x000013D0
        public static string getWSPwd(string soapStr)
        {
            return SyncXmlUtils.matchResponse(soapStr, "tem:password");
        }
        public bool isAuth(string username, string password, string requestdate)
        {
            string pwd = BSFramework.Util.Md5Helper.MD5(soapWsPwd, 32);
            if (username.Equals(soapWsUser) && password.Equals(pwd))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string soapWsUser = "HRUser";

        // Token: 0x0400002A RID: 42
        public string soapWsPwd = "#ASdWE183ZX^";


        /// <summary>
        /// LADP数据同步日志
        /// </summary>
        /// <param name="strMessage"></param>
        public void LdapDataLog(string strMessage,int intType)
        {
            LogEntity logEntity = new LogEntity();
            logEntity.CategoryId = intType;
            logEntity.OperateTypeId = ((int)OperationType.LdapSync).ToString();
            logEntity.OperateType = EnumAttribute.GetDescription(OperationType.LdapSync);
            logEntity.OperateAccount = "HRUser"; 
            if (null != OperatorProvider.Provider.Current())
            {
                //logEntity.OperateUserId = OperatorProvider.Provider.Current().UserId;
                logEntity.OperateUserId = "HRUser"; 
            }
            logEntity.ExecuteResult = -1;
            logEntity.ExecuteResultJson = strMessage;
            logEntity.Module = "同步LADP用户";
            logEntity.ModuleId = "10";
            logEntity.WriteLog();
        }
    }
    public class SyncUserInfo
    {
        // Token: 0x06000038 RID: 56 RVA: 0x000031FA File Offset: 0x000013FA
        public string getUid()
        {
            return this.uid;
        }

        // Token: 0x06000039 RID: 57 RVA: 0x00003202 File Offset: 0x00001402
        public void setUid(string uid)
        {
            this.uid = uid;
        }

        // Token: 0x0600003A RID: 58 RVA: 0x0000320B File Offset: 0x0000140B
        public string getPassword()
        {
            return this.password;
        }

        // Token: 0x0600003B RID: 59 RVA: 0x00003213 File Offset: 0x00001413
        public void setPassword(string password)
        {
            this.password = password;
        }

        // Token: 0x0600003C RID: 60 RVA: 0x0000321C File Offset: 0x0000141C
        public string getDisplayName()
        {
            return this.displayName;
        }

        // Token: 0x0600003D RID: 61 RVA: 0x00003224 File Offset: 0x00001424
        public void setDisplayName(string displayName)
        {
            this.displayName = displayName;
        }

        // Token: 0x0600003E RID: 62 RVA: 0x0000322D File Offset: 0x0000142D
        public string getEmail()
        {
            return this.email;
        }

        // Token: 0x0600003F RID: 63 RVA: 0x00003235 File Offset: 0x00001435
        public void setEmail(string email)
        {
            this.email = email;
        }
        public string getEmpno()
        {
            return this.empno;
        }

        // Token: 0x0600003F RID: 63 RVA: 0x00003235 File Offset: 0x00001435
        public void setEmpno(string empno)
        {
            this.empno = empno;
        }
        public string getDeptId()
        {
            return this.deptId;
        }

        // Token: 0x0600003F RID: 63 RVA: 0x00003235 File Offset: 0x00001435
        public void setDeptId(string deptId)
        {
            this.deptId = deptId;
        }
        public string getMobile()
        {
            return this.mobile;
        }

        // Token: 0x0600003F RID: 63 RVA: 0x00003235 File Offset: 0x00001435
        public void setMobile(string mobile)
        {
            this.mobile = mobile;
        }
        // Fields
        public string uid { set; get; } //账号
        public string password { set; get; } //密码
        public string displayName { set; get; }//显示名称
        public string email { set; get; }//邮箱
        public string empno { set; get; }//工号
        public string deptId { set; get; }//部门编号
        public string mobile { set; get; } //手机号

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

    // Token: 0x02000006 RID: 6
    public abstract class SyncUser
    {
        // Token: 0x0600002B RID: 43
        public static bool isAuth(string requestdate)
        {
            string pwd = BSFramework.Util.Md5Helper.MD5(soapWsPwd, 32);
            if (getWSUser(requestdate).Equals(soapWsUser) && getWSPwd(requestdate).Equals(pwd))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Token: 0x0600002C RID: 44 RVA: 0x00002F9C File Offset: 0x0000119

        // Token: 0x0600002D RID: 45 RVA: 0x00003044 File Offset: 0x00001244
        public static SyncUserInfo parseXMLToUser(string soapStr)
        {
            Console.Write("parseXml start .....");
            SyncUserInfo syncUserInfo = new SyncUserInfo();
            try
            {
                Type type = syncUserInfo.GetType();
                object obj = Activator.CreateInstance(type);
                foreach (MemberInfo memberInfo in type.GetMethods())
                {
                    if (memberInfo.Name.StartsWith("get") && !memberInfo.Name.Contains("_"))
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.Append(memberInfo.Name.Substring(3, 1).ToLower());
                        stringBuilder.Append(memberInfo.Name.Substring(4));
                        string patternStr = stringBuilder.ToString();
                        string text = SyncXmlUtils.matchRequestData(soapStr, patternStr);
                        StringBuilder stringBuilder2 = new StringBuilder();
                        stringBuilder2.Append("set");
                        stringBuilder2.Append(memberInfo.Name.Substring(3, 1).ToUpper());
                        stringBuilder2.Append(memberInfo.Name.Substring(4));
                        object[] array = new object[]
					{
						text
					};
                        type.GetMethod(stringBuilder2.ToString()).Invoke(obj, array);
                    }
                }
                return (SyncUserInfo)obj;
            }
            catch (Exception)
            {

            }
            Console.Write("parseXml end .....");
            return syncUserInfo;
        }

        // Token: 0x0600002E RID: 46 RVA: 0x00003198 File Offset: 0x00001398
        public string getOpType(string soapStr)
        {
            return SyncXmlUtils.matchRequestData(soapStr, "operation-type");
        }

        // Token: 0x0600002F RID: 47 RVA: 0x000031B4 File Offset: 0x000013B4
        public static string getWSUser(string soapStr)
        {
            return SyncXmlUtils.matchResponse(soapStr, "tem:username");
        }

        // Token: 0x06000030 RID: 48 RVA: 0x000031D0 File Offset: 0x000013D0
        public static string getWSPwd(string soapStr)
        {
            return SyncXmlUtils.matchResponse(soapStr, "tem:password");
        }

        // Token: 0x06000036 RID: 54 RVA: 0x000031EA File Offset: 0x000013EA
        public SyncUser()
        {
        }

        // Token: 0x04000029 RID: 41
        public static string soapWsUser = "HRUser";

        // Token: 0x0400002A RID: 42
        public static string soapWsPwd = "#ASdWE183ZX^";
    }

}
