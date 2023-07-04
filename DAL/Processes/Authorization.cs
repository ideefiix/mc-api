namespace Api.DAL.Processes;

public static class Authorization
{
    public static bool HasAccessToProfile(Guid profileId, string token)
    {
        return false;
    }
}