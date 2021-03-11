using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace ERCHTMS.Service.MatterManage
{
    public class WeightBridgeService
    {
        public WeightBridgeService()
        { }

        public DataTable GetBridgData(string carNo)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = null;
            try
            {
                string conStr = "server=192.168.0.253;database=KJAutoData;uid=sa;pwd=123";
                string sql = string.Format(@"select top 1 [车号] as platenumber,[货名] as producttype,[毛重] rough,[皮重] as tare,[净重] as netwneight,[毛重时间] as roughtime,[皮重时间] as taretime,[毛重司磅员] as roughusername,[皮重司磅员] tareusername from dbo.[称重信息]
where [车号]='{0}' order by [皮重时间] desc", carNo);
                conn = new SqlConnection(conStr);
                SqlCommand command = conn.CreateCommand();
                command.CommandText = sql;
                conn.Open();
                SqlDataAdapter sda = new SqlDataAdapter(command);
                sda.Fill(dt);
                command.Dispose();
                return dt;
            }
            catch (Exception ex)
            {
                return dt;
            }
            finally {
               if(conn.State!=ConnectionState.Closed)
                    conn.Close();               
            }           
        }
    }
}
