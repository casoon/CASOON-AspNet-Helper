using System.Threading.Tasks;
using DevExtreme.AspNet.Data.Helpers;
using Microsoft.AspNetCore.Http;

namespace DevExtreme.AspNet.Data;

public class DataSourceLoadOptions : DataSourceLoadOptionsBase {

    public static ValueTask<DataSourceLoadOptions> BindAsync(HttpContext httpContext)
    {
        var loadOptions = new DataSourceLoadOptions();
        DataSourceLoadOptionsParser.Parse(loadOptions, key => httpContext.Request.Query[key]);
        return ValueTask.FromResult(loadOptions);
    }
}