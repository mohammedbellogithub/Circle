using OpenIddict.EntityFrameworkCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Shared.Models.OpenIddict
{

    public class CircleOpenIddictApplication : OpenIddictEntityFrameworkCoreApplication<Guid, CircleOpenIddictAuthorization, CircleOpenIddictToken>
    {
        public CircleOpenIddictApplication()
        {
            Id = Guid.NewGuid();

        }
        public string? AppId { get; set; }
        public string? Language { get; set; }
    }

    public class CircleOpenIddictAuthorization : OpenIddictEntityFrameworkCoreAuthorization<Guid, CircleOpenIddictApplication, CircleOpenIddictToken> { }
    public class CircleOpenIddictScope : OpenIddictEntityFrameworkCoreScope<Guid> { }
    public class CircleOpenIddictToken : OpenIddictEntityFrameworkCoreToken<Guid, CircleOpenIddictApplication, CircleOpenIddictAuthorization> { }

}
