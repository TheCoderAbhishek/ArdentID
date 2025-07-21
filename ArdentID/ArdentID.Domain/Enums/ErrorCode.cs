namespace ArdentID.Domain.Enums
{
    public static class ErrorCode
    {
        #region Error Codes for Common from `ERR-0000-001` to `ERR-0000-999`

        public const string _internalServerError = "ERR-0000-001";

        #endregion

        #region Error Codes for Authentication Management from `ERR-1000-001` to `ERR-1000-999`

        public const string _duplicateUserError = "ERR-1000-001";

        #endregion
    }
}
