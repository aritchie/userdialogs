using System;
using System.IO;

namespace Acr.UserDialogs
{
    public interface IResourceResolver
    {
        Stream FromName(string name);
    }
}
