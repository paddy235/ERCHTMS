using BSFramework.Cache.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Code
{
    public class CacheHelper
    {
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="sclist"></param>
        /// <param name="keyname"></param>
        public static void SetChache(object sclist, string keyname)
        {
            CacheFactory.Cache().WriteCache(sclist, keyname, DateTime.Now.AddHours(1));
        }

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="keyname"></param>
        /// <returns></returns>
        public static List<SaftyCheckModel> GetChache(string keyname)
        {
            return CacheFactory.Cache().GetCache<List<SaftyCheckModel>>(keyname);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyname"></param>
        public static void RemoveChache(string keyname)
        {
            CacheFactory.Cache().RemoveCache(keyname);
        }
    }
}
