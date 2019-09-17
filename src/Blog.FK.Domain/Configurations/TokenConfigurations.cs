namespace Blog.FK.Domain.Configurations
{
    /// <summary>
    /// Used to bind TokenConfigurations section on appsettings.json
    /// </summary>
    public class TokenConfigurations
    {
        /// <summary>
        /// Token expiration in seconds
        /// </summary>
        public int Seconds { get; set; }
    }
}
