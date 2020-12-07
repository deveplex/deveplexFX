using System;
using System.Collections.Generic;
using System.Linq;

//namespace System.Configuration.Repository
//{
//    //#region Test Data
//    //internal class User
//    //{
//    //    public int ID { get; set; }
//    //    public string SUID { get; set; }
//    //    public string Account { get; set; }
//    //    public string UserName { get; set; }
//    //    public string Password { get; set; }
//    //    public List<KeyValuePair<int, string>> Roles { get; set; }
//    //}
//    //internal class UserRepository
//    //{
//    //    private User[] usersForTest = new[]{
//    //    new User{ ID = 1, Account = "Administrator", UserName = "超级管理员", Password = "administrator", Roles = new List<KeyValuePair<int, string>>{new KeyValuePair<int, string>(2325,"Administrator")}},
//    //    new User{ ID = 2, Account = "Admin", UserName = "超级管理员", Password = "admin", Roles = new List<KeyValuePair<int, string>>{new KeyValuePair<int, string>(2325,"Administrator")}},
//    //    new User{ ID = 3, Account = "Super", UserName = "超级管理员", Password = "super", Roles = new List<KeyValuePair<int, string>>{new KeyValuePair<int, string>(2325,"Administrator")}},
//    //    new User{ ID = 4, Account = "Manager", UserName = "管理员", Password = "manager", Roles = new List<KeyValuePair<int, string>>{new KeyValuePair<int, string>(3689,"Manager")}},
//    //    new User{ ID = 5, Account = "Master", UserName = "管理员", Password = "master", Roles = new List<KeyValuePair<int, string>>{new KeyValuePair<int, string>(3689,"Manager")}},
//    //    new User{ ID = 6, Account = "Server", UserName = "管理员", Password = "master", Roles = new List<KeyValuePair<int, string>>{new KeyValuePair<int, string>(3689,"Manager")}},
//    //    new User{ ID = 7, Account = "Service", UserName = "管理员", Password = "master", Roles = new List<KeyValuePair<int, string>>{new KeyValuePair<int, string>(3689,"Manager")}},
//    //    new User{ ID = 8, Account = "VIP", UserName = "VIP会员", Password = "vip", Roles = new List<KeyValuePair<int, string>>{new KeyValuePair<int, string>(5454,"VIP")}},
//    //    new User{ ID = 9, Account = "Power", UserName = "VIP会员", Password = "power", Roles = new List<KeyValuePair<int, string>>{new KeyValuePair<int, string>(5454,"VIP")}},
//    //    new User{ ID = 10, Account = "Employee", UserName = "会员", Password = "employee", Roles = new List<KeyValuePair<int, string>>{new KeyValuePair<int, string>(9487,"Employee")}},
//    //    new User{ ID = 11, Account = "User", UserName = "会员", Password = "user", Roles = new List<KeyValuePair<int, string>>{new KeyValuePair<int, string>(9487,"Employee")}},
//    //    };

//    //    public bool CheckAccount(string account)
//    //    {
//    //        bool ret = false;
//    //        if (account == null)
//    //            return ret;

//    //        if (ret = usersForTest.FirstOrDefault(u => u.Account.ToLower() == account.ToLower()) != null)
//    //            return ret;

//    //        //Match Match = Regex.Match("", "^[.]{0,3}" + account + "[.]{0,5}$", RegexOptions.IgnoreCase);

//    //        return ret;
//    //    }

//    //    public List<KeyValuePair<int, string>> GetRoles(string id)
//    //    {
//    //        return usersForTest
//    //            .Where(u => u.ID.ToString() == id)
//    //            .Select(u => u.Roles)
//    //            .FirstOrDefault();
//    //    }
//    //}

//    //public static class MemberShip
//    //{
//    //    private static UserRepository repository = new UserRepository();

//    //    public static bool CheckAccount(string account)
//    //    {
//    //        bool ret = false;
//    //        if (account == null)
//    //            return ret;

//    //        if (ret = repository.CheckAccount(account))
//    //            return ret;

//    //        string connectionString = ConfigurationManager.ConnectionStrings["RSDACDB"].ConnectionString;
//    //        SqlHelper sqlHelper = new SqlHelper(connectionString);
//    //        SqlParameter errorValue = new SqlParameter("@ErrorValue", SqlDbType.Int, 4) { Direction = ParameterDirection.ReturnValue };
//    //        sqlHelper.ExecuteNonQuery("sp_CheckAccount", CommandType.StoredProcedure, new SqlParameter[] {
//    //            errorValue,
//    //            new SqlParameter("@UserIdentity", SqlDbType.NVarChar){ Value = account },
//    //            });
//    //        if (ret = int.Parse(errorValue.Value.ToString()) == 0)
//    //            return ret;
//    //        connectionString = ConfigurationManager.ConnectionStrings["UADACDB"].ConnectionString;
//    //        sqlHelper = new SqlHelper(connectionString);
//    //        SqlParameter errorValue1 = new SqlParameter("@ErrorValue", SqlDbType.Int, 4) { Direction = ParameterDirection.ReturnValue };
//    //        sqlHelper.ExecuteNonQuery("sp_GetAccountInformationByIdetity", CommandType.StoredProcedure, new SqlParameter[] {
//    //            errorValue1,
//    //            new SqlParameter("@UserIdentity", SqlDbType.NVarChar){ Value = account },
//    //            });
//    //        if (ret = int.Parse(errorValue1.Value.ToString()) == 0)
//    //            return ret;

//    //        return ret;
//    //    }

//    //    public static DataSet CheckAccountExists(string account)
//    //    {
//    //        if (account == null)
//    //            return null;

//    //        DataSet ds = new DataSet();
//    //        string connectionString = ConfigurationManager.ConnectionStrings["UADACDB"].ConnectionString;
//    //        SqlHelper sqlHelper = new SqlHelper(connectionString);
//    //        SqlParameter errorValue = new SqlParameter("@ErrorValue", SqlDbType.Int, 4) { Direction = ParameterDirection.ReturnValue };
//    //        DataTable dt = sqlHelper.ExecuteDataTable("sp_GetAccountInformationByIdetity", CommandType.StoredProcedure, new SqlParameter[] {
//    //            errorValue,
//    //            new SqlParameter("@UserIdentity", SqlDbType.NVarChar){ Value = account },
//    //            });
//    //        if (int.Parse(errorValue.Value.ToString()) == 0)
//    //            ds.Tables.Add(dt);

//    //        return ds;
//    //    }

//    //    public static DataSet RegisterUser(string modeljson, int type)
//    //    {
//    //        JArray model = JsonConvert.DeserializeObject(modeljson) as JArray;
//    //        if (model == null)
//    //        {
//    //            model = new JArray { JObject.Parse(modeljson) };
//    //        }
//    //        var m = (from j in model.Children()
//    //                select new
//    //                {
//    //                    Account = (string)j["Account"],
//    //                    Password = (string)j["Password"],
//    //                    Email = (string)j["Email"],
//    //                    Creater = int.Parse((string)j["Creater"]),
//    //                }).FirstOrDefault();

//    //        //zggltz.Web.Models.RegisterAccountMDL m = model as zggltz.Web.Models.RegisterAccountMDL;
//    //        if (m == null)
//    //            return null;

//    //        if (repository.CheckAccount(m.Account))
//    //            return null;

//    //        string account = Deveplex.Security.Cryptography.MD5.Encrypt(m.Account);
//    //        string password = Deveplex.Security.Cryptography.MD5.Encrypt(m.Password);
//    //        string encryptstr = Deveplex.Security.Cryptography.MD5.Encrypt(account + password);

//    //        string connectionString = ConfigurationManager.ConnectionStrings["RSDACDB"].ConnectionString;
//    //        SqlHelper sqlHelper = new SqlHelper(connectionString);
//    //        SqlParameter errorValue = new SqlParameter("@ErrorValue", SqlDbType.Int, 4) { Direction = ParameterDirection.ReturnValue };
//    //        DataTable dt = sqlHelper.ExecuteDataTable("sp_CreateRegisterActivityRecord", CommandType.StoredProcedure, new SqlParameter[] {
//    //                errorValue,
//    //                new SqlParameter("@Account", SqlDbType.NVarChar){ Value = m.Account },
//    //                new SqlParameter("@Password", SqlDbType.NVarChar){ Value = encryptstr },
//    //                new SqlParameter("@Creater", SqlDbType.Int){ Value = m.Creater },
//    //                new SqlParameter("@Val", SqlDbType.NVarChar){ Value = m.Email },
//    //                new SqlParameter("@Type",SqlDbType.Int){Value = type},
//    //            });
//    //        if (dt == null || dt.Rows.Count <= 0)
//    //        {
//    //            return null;
//    //        }
//    //        DataSet ds = new DataSet();
//    //        ds.Tables.Add(dt);
//    //        return ds;
//    //    }

//    //    public static bool CreateUser(int rid, string code)
//    //    {
//    //        if (code == null)
//    //            return false;

//    //        string connectionString = ConfigurationManager.ConnectionStrings["RSDACDB"].ConnectionString;
//    //        SqlHelper sqlHelper = new SqlHelper(connectionString);
//    //        SqlParameter errorValue = new SqlParameter("@ErrorValue", SqlDbType.Int, 4) { Direction = ParameterDirection.ReturnValue };
//    //        DataTable dt = sqlHelper.ExecuteDataTable("sp_ActivateRegisterRecord", CommandType.StoredProcedure, new SqlParameter[]{
//    //                errorValue,
//    //                new SqlParameter("@RecordID", SqlDbType.Int){ Value = rid },
//    //                new SqlParameter("@Code", SqlDbType.NVarChar){ Value = code },
//    //            });
//    //        if (dt == null || dt.Rows.Count <= 0)
//    //        {
//    //            return false;
//    //        }

//    //        string uaconnectionString = ConfigurationManager.ConnectionStrings["UADACDB"].ConnectionString;
//    //        SqlHelper sqluaHelper = new SqlHelper(uaconnectionString);
//    //        SqlParameter errorValue1 = new SqlParameter("@ErrorValue", SqlDbType.Int, 4) { Direction = ParameterDirection.ReturnValue };
//    //        DataTable uadt = sqluaHelper.ExecuteDataTable("sp_CreateAccount", CommandType.StoredProcedure, new SqlParameter[]{
//    //                    errorValue1,
//    //                    new SqlParameter("@UserIdentity", SqlDbType.NVarChar){ Value = dt.Rows[0]["Account"].ToString() },
//    //                    new SqlParameter("@Password", SqlDbType.NVarChar){ Value = dt.Rows[0]["Password"].ToString() },
//    //                    new SqlParameter("@Creater", SqlDbType.Int){ Value = int.Parse(dt.Rows[0]["Creater"].ToString()) },
//    //                });
//    //        if (uadt == null || uadt.Rows.Count <= 0)
//    //        {
//    //            return false;
//    //        }

//    //        int type = int.Parse(dt.Rows[0]["Type"].ToString());
//    //        if (type == 1)
//    //        {
//    //        }
//    //        else
//    //        {
//    //            SqlParameter errorValue2 = new SqlParameter("@ErrorValue", SqlDbType.Int, 4) { Direction = ParameterDirection.ReturnValue };
//    //            uadt = sqluaHelper.ExecuteDataTable("sp_UpdateSecurityProfile", CommandType.StoredProcedure, new SqlParameter[]{
//    //                    errorValue2,
//    //                    new SqlParameter("@SAID",SqlDbType.UniqueIdentifier){ Value = Guid.Parse(uadt.Rows[0]["AccountID"].ToString()) },
//    //                    new SqlParameter("@Email",SqlDbType.NVarChar){ Value = dt.Rows[0]["Val"].ToString() },
//    //                });
//    //        }

//    //        return true;
//    //    }

//    //    public static string ValidateUser(string userName, string psw)
//    //    {
//    //        var jss = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, ContractResolver = new CamelCasePropertyNamesContractResolver() };
//    //        string json = JsonConvert.SerializeObject(false, Formatting.Indented, jss);

//    //        if (userName == null || psw == null)
//    //            return json;

//    //        //User user = usersForTest.FirstOrDefault(u => u.Account.ToLower() == account.ToLower() && u.Password == password);
//    //        //if (user == null)
//    //        //    return null;

//    //        string account = Deveplex.Security.Cryptography.MD5.Encrypt(userName);
//    //        string password = Deveplex.Security.Cryptography.MD5.Encrypt(psw);
//    //        string encryptstr = Deveplex.Security.Cryptography.MD5.Encrypt(account + password);

//    //        string uaconnectionString = ConfigurationManager.ConnectionStrings["UADACDB"].ConnectionString;
//    //        SqlHelper sqluaHelper = new SqlHelper(uaconnectionString);
//    //        SqlParameter errorValue = new SqlParameter("@ErrorValue", SqlDbType.Int, 4) { Direction = ParameterDirection.ReturnValue };
//    //        DataTable uadt = sqluaHelper.ExecuteDataTable("sp_Login", CommandType.StoredProcedure, new SqlParameter[]{
//    //                    errorValue,
//    //                    new SqlParameter("@UserIdentity", SqlDbType.NVarChar){ Value = userName },
//    //                    new SqlParameter("@Password", SqlDbType.NVarChar){ Value = encryptstr },
//    //                });
//    //        if (uadt == null || uadt.Rows.Count <= 0)
//    //        {
//    //            return json;
//    //        }

//    //        var q = from a in uadt.AsEnumerable()
//    //                where a.Field<string>("Account").ToLower() == userName.ToLower()
//    //                select new
//    //                {
//    //                    AccountID = a.Field<Guid>("AccountID"),
//    //                    Account = a.Field<string>("Account"),
//    //                    UserName = a.Field<string>("UserName"),
//    //                    Creater = a.Field<int>("Creater"),
//    //                    IsRealName = a.Field<bool>("IsRealName"),
//    //                    Roles = new List<KeyValuePair<int, string>>() { }
//    //                };

//    //        //zggltz.Web.Models.AuthorizationContext context = new zggltz.Web.Models.AuthorizationContext();
//    //        //context.ID = uadt.Rows[0]["AccountID"].ToString();
//    //        //context.Account = uadt.Rows[0]["Account"].ToString();
//    //        //context.UserName = uadt.Rows[0]["UserName"].ToString();
//    //        //context.Creater = int.Parse(uadt.Rows[0]["Creater"].ToString());
//    //        //context.IsRealName = bool.Parse(uadt.Rows[0]["IsRealName"].ToString());
//    //        //context.Roles = new List<KeyValuePair<int, string>>() { };//repository.GetRoles(context.ID);

//    //        json = JsonConvert.SerializeObject(q, Formatting.Indented);
//    //        return json;
//    //    }

//    //    public static bool SendMail(string stmpUserName, string stmpPass, string strFromName, string strTo, string strToName, string strSubject, string strBody, bool bodyformat, string sfile = null)
//    //    {
//    //        //NetSectionGroup.GetSectionGroup(WebConfigurationManager.OpenWebConfiguration("~/web.config")).MailSettings.Smtp
//    //        SmtpSection smtp = ConfigurationManager.GetSection("system.net/mailSettings/smtp") as SmtpSection;
//    //        if (smtp == null)
//    //            return false;

//    //        MailAddress from = new MailAddress(smtp.From, strFromName);
//    //        MailAddress to = new MailAddress(strTo, strToName);

//    //        MailMessage objMail = new MailMessage(from, to);

//    //        if (!string.IsNullOrWhiteSpace(sfile))
//    //        {
//    //            objMail.Attachments.Add(new Attachment(sfile));
//    //        }

//    //        objMail.Priority = MailPriority.Normal;

//    //        objMail.Subject = strSubject;
//    //        objMail.SubjectEncoding = System.Text.Encoding.UTF8;
//    //        objMail.BodyEncoding = System.Text.Encoding.UTF8;
//    //        objMail.IsBodyHtml = bodyformat;
//    //        objMail.Body = strBody;

//    //        SmtpClient client = new SmtpClient(smtp.Network.Host, smtp.Network.Port);
//    //        client.DeliveryMethod = smtp.DeliveryMethod;
//    //        client.UseDefaultCredentials = false;
//    //        client.Credentials = new NetworkCredential((stmpUserName == null) ? smtp.Network.UserName : stmpUserName, (stmpPass == null) ? smtp.Network.Password : stmpPass);
//    //        try
//    //        {
//    //            client.Send(objMail);
//    //        }
//    //        catch (Exception ex)
//    //        {
//    //            return false;
//    //        }
//    //        finally
//    //        {
//    //            ////释放资源
//    //            objMail.Dispose();
//    //        }
//    //        return true;
//    //    }

//    //    public static List<string> GetActionRoles(string action, string controller)
//    //    {
//    //        //XElement rootElement = XElement.Load(HttpContext.Current.Server.MapPath("/") + "ActionRoles.xml");
//    //        //XElement controllerElement = findElementByAttribute(rootElement, "Controller", controller);
//    //        //if (controllerElement != null)
//    //        //{
//    //        //    XElement actionElement = findElementByAttribute(controllerElement, "Action", action);
//    //        //    if (actionElement != null)
//    //        //    {
//    //        //        return actionElement.Value;
//    //        //    }
//    //        //}
//    //        List<string> list = new List<string>();
//    //        list.Add("Administrator");
//    //        return list;
//    //    }

//    //    public static List<string> GetActionUsers(string action, string controller)
//    //    {
//    //        //XElement rootElement = XElement.Load(HttpContext.Current.Server.MapPath("/") + "ActionRoles.xml");
//    //        //XElement controllerElement = findElementByAttribute(rootElement, "Controller", controller);
//    //        //if (controllerElement != null)
//    //        //{
//    //        //    XElement actionElement = findElementByAttribute(controllerElement, "Action", action);
//    //        //    if (actionElement != null)
//    //        //    {
//    //        //        return actionElement.Value;
//    //        //    }
//    //        //}
//    //        List<string> list = new List<string>();
//    //        list.Add("admin");
//    //        return list;
//    //    }
//    //    public static List<string> GetActionRolesPermission(string action, string controller)
//    //    {
//    //        List<string> list = new List<string>();
//    //        list.Add("admin");
//    //        return list;
//    //    }
//    //    public static List<string> GetActionUsersPermission(string action, string controller)
//    //    {
//    //        List<string> list = new List<string>();
//    //        list.Add("admin");
//    //        return list;
//    //    }
//    //}
//    //#endregion //Test Data

//    //public class ValidateCode
//    //{
//    //    public string CreateValidateCode(int length)
//    //    {
//    //        length = (length <= 0) ? 5 : length;

//    //        string allChar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,a,b,c,d,e,f,g,h,i,j,k,m,n,o,p,q,s,t,u,w,x,y,z";
//    //        string[] allCharArray = allChar.Split(',');
//    //        string validateCode = "";
//    //        int temp = -1;
//    //        Random rand = new Random();
//    //        for (int i = 0; i < length; i++)
//    //        {
//    //            if (temp != -1)
//    //            {
//    //                rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
//    //            }
//    //            int t = rand.Next(35);
//    //            if (temp == t)
//    //            {
//    //                return CreateValidateCode(length);
//    //            }
//    //            temp = t;
//    //            validateCode += allCharArray[t];
//    //        }
//    //        return validateCode;
//    //    }

//    //    public byte[] CreateValidateCodeImage(string validateCode)
//    //    {
//    //        if (validateCode == null || validateCode.Trim() == string.Empty)
//    //            return new byte[1];
//    //        System.Drawing.Bitmap image = new System.Drawing.Bitmap(1, 1);
//    //        Graphics g = Graphics.FromImage(image);
//    //        Font font = new System.Drawing.Font("Arial", 12, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic));
//    //        SizeF sim = g.MeasureString("W", font);
//    //        g.Dispose();
//    //        image = new System.Drawing.Bitmap(validateCode.Length * (int)sim.Width, (int)sim.Height);
//    //        g = Graphics.FromImage(image);
//    //        try
//    //        {
//    //            Random random = new Random();
//    //            g.Clear(Color.White);
//    //            for (int i = 0; i < 5; i++)
//    //            {
//    //                int x1 = random.Next(image.Width);
//    //                int x2 = random.Next(image.Width);
//    //                int y1 = random.Next(image.Height);
//    //                int y2 = random.Next(image.Height);
//    //                g.DrawLine(new Pen(Color.FromArgb(random.Next())), x1, y1, x2, y2);
//    //            }
//    //            System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
//    //            float xx = image.Width / validateCode.Length;
//    //            for (int i = 0; i < validateCode.Length; i++)
//    //            {
//    //                float x = 0;
//    //                float y = 0;
//    //                g.DrawString(new string(validateCode[i], 1), font, brush, i * xx, 0);
//    //            }
//    //            for (int i = 0; i < 20; i++)
//    //            {
//    //                int x = random.Next(image.Width);
//    //                int y = random.Next(image.Height);
//    //                image.SetPixel(x, y, Color.FromArgb(random.Next()));

//    //            }
//    //            System.IO.MemoryStream ms = new System.IO.MemoryStream();
//    //            image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
//    //            return ms.ToArray();
//    //        }
//    //        finally
//    //        {
//    //            g.Dispose();
//    //            image.Dispose();
//    //        }
//    //    }
//    //}

//    ////public class GetRoles
//    ////{
//    ////    public static XElement findElementByAttribute(XElement xElement, string tagName, string attribute)
//    ////    {
//    ////        return xElement.Elements(tagName).FirstOrDefault(x => x.Attribute("name").Value.Equals(attribute, StringComparison.OrdinalIgnoreCase));
//    ////    }
//    ////}
//}
