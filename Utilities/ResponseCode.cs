using System;

namespace Utilities
{
    /// <summary>
    /// basic api response code definitions
    /// </summary>
    public enum ResponseCode : int
    {
        OK = 200,

        BadRequest = 400 ,

        ServerError = 500
    }
}
