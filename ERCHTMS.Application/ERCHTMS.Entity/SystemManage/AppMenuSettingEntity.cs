using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.SystemManage
{
    [Table("BASE_APPMENUSETTING")]
   public  class AppMenuSettingEntity : BaseEntity
    {

        [Column("ID")]
        public string Id { get; set; }
        [Column("DEPTID")]
        public string DeptId { get; set; }
        [Column("DEPTCODE")]
        public string DeptCode { get; set; }
        [Column("DEPTNAME")]
        public string DeptName { get; set; }
        [Column("SORT")]
        public int? Sort { get; set; }
        [Column("REMARK")]
        public string Remark { get; set; }
        [Column("NAME")]
        public string Name { get; set; }
        [Column("THEMECODE")]
        public int? ThemeCode { get; set; }
        [Column("PLATFORMTYPE")]
        public int? PlatformType { get; set; }
        [Column("ICON")]
        public string Icon { get; set; }
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
        }
        public override void Modify(string keyValue)
        {
            this.Id = keyValue;
        }

        /// <summary>
        /// 复制实体中的所有的数据，并生产一个新的实体
        /// 会生成主键
        /// </summary>
        /// <returns></returns>
        public AppMenuSettingEntity Clone(string deptId,string dpetName,string deptCode)
        {
            AppMenuSettingEntity cloneEntity = new AppMenuSettingEntity()
            {
                Id = Guid.NewGuid().ToString(),
                DeptId = deptId,
                DeptCode = deptCode,
                DeptName = dpetName,
                Name = this.Name,
                PlatformType = this.PlatformType,
                Remark = this.Remark,
                Sort = this.Sort,
                ThemeCode = this.ThemeCode
            };
            return cloneEntity;
        }
        public AppMenuSettingEntity Clone(string deptId, string dpetName, string deptCode, List<AppSettingAssociationEntity> oldEntities,  List<AppSettingAssociationEntity> newAssociationEntities)
        {
            AppMenuSettingEntity cloneEntity = new AppMenuSettingEntity()
            {
                Id = Guid.NewGuid().ToString(),
                DeptId = deptId,
                DeptCode = deptCode,
                DeptName = dpetName,
                Name = this.Name,
                PlatformType = this.PlatformType,
                Remark = this.Remark,
                Sort = this.Sort,
                ThemeCode = this.ThemeCode
            };
            //找到当前栏目对应的菜单

            oldEntities.Where(p=>p.ColumnId==this.Id).ToList().ForEach(p =>
            {
                AppSettingAssociationEntity association = new AppSettingAssociationEntity()
                {
                    ColumnId = cloneEntity.Id,
                    ColumnName = cloneEntity.Name,
                    DeptId = deptId,
                    Id = Guid.NewGuid().ToString(),
                    ModuleId = p.ModuleId,
                    Sort = p.Sort
                };
                newAssociationEntities.Add(association);
            });
            return cloneEntity;
        }
    }
}
