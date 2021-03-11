using ERCHTMS.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERCHTMS.Web.Areas.SystemManage.Model
{
    public class ResponesMenuTree : MenuConfigEntity
    {
        private string _id;
        private string _text;
        public ResponesMenuTree()
        {
            _id = Id;
            _text = ModuleName;
        }
        /// <summary>
        /// 菜单树用
        /// </summary>
        public string id { get { return _id; } }
        /// <summary>
        /// 菜单树用
        /// </summary>
        public string text { get { return _text; } }
        /// <summary>
        /// 菜单树用
        /// </summary>
        public string value { get { return _id; } }
    }
}