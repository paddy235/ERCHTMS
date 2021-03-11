using ERCHTMS.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERCHTMS.Web.Areas.SystemManage.Model
{
    public class MenuCheckBoxModel
    {

        private int _checkstate;
        private bool _complete;
        private bool _hasChildren;
        private string _id;
        private bool _isexpand;
        private string _parentnodes;
        private bool _showcheck;
        private string _value;

        public MenuCheckBoxModel()
        {
            _checkstate = 0;
            _complete = true;
            _hasChildren = false;
            _id = "0";
            _isexpand = false;
            _parentnodes = "0";
            _showcheck = false;
            _value = "0";
        }
        public MenuCheckBoxModel(MenuConfigEntity item, string parentId = "0", bool showCheckBox = false) : this()
        {
            this.id = item.Id;
            this.text = item.ModuleName;
            this.value = item.Id;
            this.parentnodes = parentId;
            this.platformType = item.PaltformType;
            this.showcheck = showCheckBox;
        }

        public int checkstate
        {
            get { return _checkstate; }
            set
            {
                _checkstate = value;
            }
        }
        public bool complete
        {
            get { return _complete; }
            set
            {
                _complete = value;
            }
        }
        public bool hasChildren
        {
            get { return _hasChildren; }
            set
            {
                _hasChildren = value;
            }
        }
        public string id
        {
            get { return _id; }
            set
            {
                _id = value;
            }
        }
        public string img { get; set; }
        public bool isexpand
        {
            get { return _isexpand; }
            set
            {
                _isexpand = value;
            }
        }
        public string parentnodes
        {
            get { return _parentnodes; }
            set
            {
                _parentnodes = value;
            }
        }
        public bool showcheck
        {
            get { return _showcheck; }
            set
            {
                _showcheck = value;
            }
        }
        public string text { get; set; }
        public string value
        {
            get { return _value; }
            set
            {
                _value = value;
            }
        }
        public int? platformType { get; set; }
      
    }
}