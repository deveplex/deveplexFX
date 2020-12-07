using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace System
{
    public static class StringExtensions
    {
        /// <summary>
        /// 
        /// (?<=[\d]{3})\d(?=[\d]{3}) 手机
        /// (?<=[\d]{4})\d+(?=[\w]{3}) 身份证
        /// (?<=[\d]{3})\d+(?=[\d]{4}) 银行卡号
        /// (?<=.{2}).+(?=@.+) 电子邮箱
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string RegexReplace(this string source, string pattern)
        {
            return RegexReplace(source, pattern, "*");
        }

        public static string RegexReplace(this string source, string pattern, string replacement)
        {
            return Regex.Replace(source, pattern, replacement, RegexOptions.Singleline);
        }

        /*
        var reg = /(.{2}).+(.{2}@.+)/g;
        var str = "yongliesina1_.23@qq.com";
        console.log(str.replace(reg, "$1****$2"));
        */
        //public static string bbbbbbbbbbbb(this string source, string pattern, string replacement)
        //{
        //    return Regex.Replace(source, pattern, replacement, RegexOptions.Singleline);
        //}
    }
}
