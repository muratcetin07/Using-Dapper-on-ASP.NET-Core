using System;
using System.Collections.Generic;
using System.Text;
using Utilities;

namespace Data.Core
{
    /// <summary>
    /// Result of Data Operation with Generic parameter
    /// </summary>
    public class DataResult<T>
    {
        public DataResult(T result)
        {
            Code = ResponseCode.OK;
            Message = "";
            Result = result;
        }

        /// <summary>
        /// Is Operation Response Code
        /// </summary>
        public ResponseCode Code { get; set; }

        /// <summary>
        /// Error Message if IsSucceed false
        /// </summary>
        public string Message { get; set; }

        public T Result { get; set; }
    }


    public class DataResult
    {
        public DataResult(bool isSucceed, string message)
        {
            IsSucceed = isSucceed;
            Message = message;
        }

        /// <summary>
        /// Is Operation Succeed
        /// </summary>
        public bool IsSucceed { get; set; }

        /// <summary>
        /// Error Message if IsSucceed false
        /// </summary>
        public string Message { get; set; }
    }
}
