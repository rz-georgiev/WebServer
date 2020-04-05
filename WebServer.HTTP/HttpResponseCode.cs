namespace WebServer.HTTP
{
    public enum HttpResponseCode
    {
        OK = 200,
        MovedPermanenty = 301,
        TermporaryRedirect = 307,
        Forbidden = 403,
        NotFound = 404,
        InternalServerError = 500
    }
}