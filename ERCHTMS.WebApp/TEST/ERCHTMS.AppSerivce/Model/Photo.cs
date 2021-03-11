using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERCHTMS.AppSerivce.Model
{
    //文件实体
    public class Photo
    {
        public string id { get; set; }
        public string filename { get; set; }
        public string fileurl { get; set; }

        public string folderid { get; set; }
    }
}