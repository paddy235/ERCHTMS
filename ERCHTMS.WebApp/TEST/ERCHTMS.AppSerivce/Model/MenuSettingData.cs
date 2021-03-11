using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERCHTMS.AppSerivce.Model
{
    public class MenuSettingData
    {
        public MenuSettingData()
        {
            this.HasChild = false;
            Child = new List<ChildMenu>();
        }
        public string Id { get; set; }
        public int? Sort { get; set; }
        public string Remark { get; set; }
        public string Name { get; set; }
        public List<ChildMenu> Child { get; set; }
        public bool HasChild { get; set; }
        public void AddChild(ChildMenu menu) {
            this.HasChild = true;
            Child.Add(menu);
        }
        /// <summary>
        /// 按菜单默认排序来添加
        /// </summary>
        /// <param name="menu"></param>
        public void AddChild(List<MenuConfigEntity> menu)
        {
            List<ChildMenu> children = new List<ChildMenu>();
            foreach (var item in menu)
            {
                ChildMenu child = new ChildMenu()
                {
                    ModuleId = item.ModuleId,
                    MenuIcon =new DataItemDetailBLL().GetItemValue("imgUrl") +  item.MenuIcon,
                    ModuleName = item.ModuleName,
                    ParentId = item.ParentId,
                    ParentName = item.ParentName,
                    Remark = item.Remark,
                    Sort = item.Sort,
                    ModuleCode = item.ModuleCode
                };
                children.Add(child);
            }
            if (children.Count>0)
            {
                this.HasChild = true;
                this.Child.AddRange(children.OrderBy(x => x.Sort).ToList());
            }
        }
       /// <summary>
       /// 按设置排序来添加
       /// </summary>
       /// <param name="appMenu"></param>
       /// <param name="list"></param>
        public void AddChild(List<MenuConfigEntity> appMenu, List<AppSettingAssociationEntity> list)
        {
            List<ChildMenu> children = new List<ChildMenu>();
            foreach (var item in appMenu)
            {
                var associationEntity = list.FirstOrDefault(p => p.ModuleId == item.ModuleId);
                ChildMenu child = new ChildMenu()
                {
                    ModuleId = item.ModuleId,
                    MenuIcon = new DataItemDetailBLL().GetItemValue("imgUrl") + item.MenuIcon,
                    ModuleName = item.ModuleName,
                    ParentId = item.ParentId,
                    ParentName = item.ParentName,
                    Remark = item.Remark,
                    Sort = associationEntity==null ? null : associationEntity.Sort,
                    ModuleCode = item.ModuleCode
                };
                children.Add(child);
            }
            if (children.Count > 0)
            {
                this.HasChild = true;
                this.Child.AddRange(children.OrderBy(x => x.Sort).ToList());
            }
        }
    }

    public class ChildMenu
    {
        public int? Sort { get; set; }
        public string ModuleName { get; set; }
        public string ModuleId { get; set; }
        public string Remark { get; set; }
        public string ParentId { get; set; }
        public string ParentName { get; set; }
        public string MenuIcon { get; set; }

        public string ModuleCode { get; set; }

    }
}