namespace Misa.AMIS.Auth.Model.DTO
{
    public class TokenResponse
    {        
        /// <summary>
        /// Thành công hay thất bại
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Dữ liệu đi kèm khi thành công hoặc thất bại
        /// </summary>
        public object Data { get; set; }
    }
}
