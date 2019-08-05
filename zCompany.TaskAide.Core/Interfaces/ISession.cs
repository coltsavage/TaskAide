using System.Collections.ObjectModel;
using zCompany.Utilities;

namespace zCompany.TaskAide
{
    public interface ISession
    {
        // Properties
        IInterval ActiveInterval { get; }

        IDateTimeZone DateTimeStart { get; }
    }
}