using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AhpilyServer.Tool
{
    /// <summary>
    /// 建立和数据库的连接
    /// </summary>
    public class ConnHelper
    {
        public const string CONNECTIONSTRING = "datasource=127.0.0.1;port=3306;database=tank;user=root;pwd=1234;";

        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <returns></returns>
        public static MySqlConnection Connect()
        {
            MySqlConnection conn = new MySqlConnection(CONNECTIONSTRING);
            try
            {
                conn.Open();
                return conn;
            }
            catch (Exception e)
            {
                Console.WriteLine("连接数据的时候出现了异常: " + e.Message);
                return null;
            }
        }

        /// <summary>
        /// 关闭数据库
        /// </summary>
        /// <param name="conn"></param>
        public static void CLose(MySqlConnection conn)
        {
            try
            {
                if (conn != null)
                    conn.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("连接不能为空");
            }
        }
    }
}
