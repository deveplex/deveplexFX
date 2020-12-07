using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace System.Net.Http
{
    /// <summary>
    /// 重命名上传的头像文件
    /// </summary>
    //public class RenameMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    //{
    //    public string FileName { get; set; }
    //    public string Extension { get; set; }

    //    public RenameMultipartFormDataStreamProvider(string root)
    //        : base(root)
    //    { }

    //    public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
    //    {
    //        string name = "";
    //        string exp = "";
    //        if (FileName == null)
    //            name = DateTime.Now.ToString("yyyyMMdd") + Guid.NewGuid().ToString("N");//base.GetLocalFileName(headers);
    //        else
    //            name = FileName;

    //        if (Extension == null)
    //            exp = Path.GetExtension(headers.ContentDisposition.FileName.TrimStart('\"').TrimEnd('\"'));
    //        else
    //            exp = Extension;

    //        return name + exp;
    //    }

    //}
}