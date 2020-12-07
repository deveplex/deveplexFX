using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deveplex
{
    public class ErrorException : Exception
    {
        private int _Code = 0;
        /// <summary>
        /// 错误代码。
        /// </summary>
        public virtual int Code => _Code;
        /// <summary>
        /// 解释异常原因的错误消息或空字符串 ("")。
        /// </summary>
        public override string Message => base.Message;

        /// <summary>
        /// 初始化 ErrorException 类的新实例。
        /// </summary>
        public ErrorException() : base()
        {

        }
        /// <summary>
        /// 用指定的错误消息初始化 ErrorException 类的新实例
        /// </summary>
        /// <param name="code">错误代码。</param>
        /// <param name="message">描述错误的消息。</param>
        public ErrorException(int code, string message) : base(message)
        {
            _Code = code;
        }
        /// <summary>
        /// 使用指定的错误消息和对作为此异常原因的内部异常的引用来初始化 ErrorException 类的新实例
        /// </summary>
        /// <param name="code">错误代码。</param>
        /// <param name="message">描述错误的消息。</param>
        /// <param name="innerException">导致当前异常的异常；如果未指定内部异常，则是一个 null 引用（在 Visual Basic 中为 Nothing）</param>
        public ErrorException(int code, string message, Exception innerException) : base(message, innerException)
        {
            _Code = code;
        }
    }
}