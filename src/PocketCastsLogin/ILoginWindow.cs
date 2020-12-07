namespace PocketCastsLogin
{
    /// <summary>
    ///     Marker interface for command line login window
    /// </summary>
    public interface ILoginWindow
    {
        (string email, string password) Show();
    }
}